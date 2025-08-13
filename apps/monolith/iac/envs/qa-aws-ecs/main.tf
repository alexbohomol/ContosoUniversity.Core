module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "cluster" {
  name = "${var.app_name}-cluster"
}

resource "aws_cloudwatch_log_group" "cw_lg" {
  name              = "/ecs/${var.app_name}-cw-lg"
  retention_in_days = 7
}

# IAM Role/Policy for CloudWatch logging

resource "aws_iam_role" "task_execution" {
  name = "${var.app_name}-exec-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action = "sts:AssumeRole"
      Principal = {
        Service = "ecs-tasks.amazonaws.com"
      }
      Effect = "Allow"
      Sid    = ""
    }]
  })
}

resource "aws_iam_role_policy_attachment" "task_execution_policy" {
  role       = aws_iam_role.task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

# Rules to attach to default VPC SG

resource "aws_security_group_rule" "mssql_internal" {
  description       = "Allow MSSQL traffic within the same SG"
  type              = "ingress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  self              = true
  security_group_id = module.networking.sg_id
}

resource "aws_security_group_rule" "web_internal" {
  description       = "Allow web traffic within ECS task"
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  self              = true
  security_group_id = module.networking.sg_id
}

resource "aws_security_group_rule" "web_public" {
  description       = "Allow incoming HTTP from the internet"
  type              = "ingress"
  from_port         = 80
  to_port           = 80
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = module.networking.sg_id
}

resource "aws_security_group_rule" "mssql_public" {
  description       = "Allow incoming MSSQL from the internet"
  type              = "ingress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  cidr_blocks       = ["0.0.0.0/0"]
  security_group_id = module.networking.sg_id
}

# ECS tasks definitions

resource "aws_ecs_task_definition" "web_task" {
  family                   = "${var.app_name}-web-tasks"
  cpu                      = "256"
  memory                   = "1024"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  execution_role_arn       = aws_iam_role.task_execution.arn
  # task_role_arn            = aws_iam_role.task_execution.arn

  container_definitions = jsonencode([
    {
      essential = true
      name      = "web"
      image     = "ghcr.io/alexbohomol/cuweb:latest"
      portMappings = [
        {
          containerPort = 80,
          hostPort      = 80
        }
      ]
      environment = [
        { name = "ASPNETCORE_ENVIRONMENT", value = var.environment },
        { name = "ASPNETCORE_URLS", value = "http://+:80" },
        { name = "SqlConnectionStringBuilder__DataSource", value = "localhost" }
      ]
      healthCheck = {
        command = [
          "CMD-SHELL",
          "curl --fail http://localhost:80/health/readiness || exit 1"
        ]
        interval    = 5
        timeout     = 5
        retries     = 10
        startPeriod = 5
      }
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.cw_lg.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "web"
        }
      }
    }
  ])
}

resource "aws_ecs_task_definition" "mssql_task" {
  family                   = "${var.app_name}-mssql-tasks"
  cpu                      = "1024"
  memory                   = "3072"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  execution_role_arn       = aws_iam_role.task_execution.arn
  # task_role_arn            = aws_iam_role.task_execution.arn

  container_definitions = jsonencode([
    {
      essential = true
      name      = "mssql"
      image     = "mcr.microsoft.com/mssql/server"
      portMappings = [
        {
          containerPort = 1433,
          hostPort      = 1433
        }
      ]
      environment = [
        { name = "ACCEPT_EULA", value = "Y" },
        { name = "SA_PASSWORD", value = var.db_password }
      ]
      healthCheck = {
        command = [
          "CMD",
          "/opt/mssql-tools18/bin/sqlcmd",
          "-S", "localhost",
          "-U", var.db_username,
          "-P", var.db_password,
          "-Q", "SELECT 1",
          "-C"
        ]
        interval    = 5
        timeout     = 5
        retries     = 10
        startPeriod = 5
      }
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.cw_lg.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "mssql"
        }
      }
    }
  ])
}

resource "aws_ecs_task_definition" "mssql_migrator_task" {
  family                   = "${var.app_name}-mssql-migrator-tasks"
  cpu                      = "256"
  memory                   = "512"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  execution_role_arn       = aws_iam_role.task_execution.arn
  # task_role_arn            = aws_iam_role.task_execution.arn

  container_definitions = jsonencode([
    {
      essential = true
      name      = "mssql-migrator"
      image     = "ghcr.io/alexbohomol/mssql-migrator:latest"
      environment = [
        { name = "DB_HOST", value = "mssql" },
        { name = "DB_USER", value = var.db_username },
        { name = "DB_PASSWORD", value = var.db_password },
        { name = "INIT_SCRIPT", value = var.db_init_script }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.cw_lg.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "mssql-migrator"
        }
      }
    }
  ])
}
