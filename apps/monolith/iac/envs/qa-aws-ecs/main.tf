module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "cluster" {
  name = "${var.app_name}-cluster"
}

resource "aws_cloudwatch_log_group" "cw_web_lg" {
  name              = "/ecs/${var.app_name}-cw-web-lg"
  retention_in_days = 7
}

resource "aws_cloudwatch_log_group" "cw_mssql_lg" {
  name              = "/ecs/${var.app_name}-cw-mssql-lg"
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

# EFS for persistent storage

resource "aws_efs_file_system" "efs" {
  encrypted        = true
  performance_mode = "generalPurpose"
  creation_token   = "${var.app_name}-efs"
  tags = {
    Name = "${var.app_name}-efs"
  }
}

resource "aws_security_group_rule" "sg_efs_ingress" {
  from_port         = 2049
  protocol          = "tcp"
  security_group_id = module.networking.sg_id
  to_port           = 2049
  type              = "ingress"
  cidr_blocks       = ["0.0.0.0/0"]
}

# resource "aws_security_group_rule" "sg_efs_egress" {
#   from_port         = 0
#   protocol          = "-1"
#   security_group_id = module.networking.sg_id
#   to_port           = 0
#   type              = "egress"
#   cidr_blocks       = ["0.0.0.0/0"]
# }

resource "aws_efs_mount_target" "efs_mt" {
  for_each        = { for idx, subnet_id in module.networking.subnet_ids : idx => subnet_id }
  file_system_id  = aws_efs_file_system.efs.id
  subnet_id       = each.value
  security_groups = [module.networking.sg_id]
}

resource "aws_efs_access_point" "efs_ap" {
  file_system_id = aws_efs_file_system.efs.id
  posix_user {
    uid = 0
    gid = 0
  }
  root_directory {
    path = "/dpkeys"
    creation_info {
      owner_gid   = 0
      owner_uid   = 0
      permissions = "0770"
    }
  }
}

# ECS tasks definitions

resource "aws_ecs_task_definition" "web_task" {
  family                   = "${var.app_name}-web-tasks"
  cpu                      = "1024"
  memory                   = "2048"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  execution_role_arn       = aws_iam_role.task_execution.arn
  task_role_arn            = aws_iam_role.task_execution.arn

  volume {
    name = "dpkeys"
    efs_volume_configuration {
      file_system_id     = aws_efs_file_system.efs.id
      transit_encryption = "ENABLED"
      authorization_config {
        access_point_id = aws_efs_access_point.efs_ap.id
        iam             = "ENABLED"
      }
      root_directory = "/"
    }
  }

  container_definitions = jsonencode([
    {
      essential = true
      name      = "web"
      image     = "ghcr.io/alexbohomol/cuweb"
      portMappings = [
        {
          containerPort = 80,
          hostPort      = 80
        }
      ]
      mountPoints = [
        {
          sourceVolume = "dpkeys"
          # containerPath = "/app/DataProtectionKeys"
          containerPath = "/var/dpkeys"
          readOnly      = false
        }
      ]
      environment = [
        { name = "ENVIRONMENT", value = var.environment },
        { name = "DOTNET_ENVIRONMENT", value = var.environment },
        { name = "ASPNETCORE_ENVIRONMENT", value = var.environment }
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
          awslogs-group         = aws_cloudwatch_log_group.cw_web_lg.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "web"
        }
      }
    }
  ])
}

resource "aws_ecs_task_definition" "mssql_task" {
  family                   = "${var.app_name}-mssql-tasks"
  cpu                      = "512"
  memory                   = "2048"
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
          awslogs-group         = aws_cloudwatch_log_group.cw_mssql_lg.name
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
      image     = "ghcr.io/alexbohomol/mssql-migrator"
      environment = [
        { name = "DB_HOST", value = local.mssql_dns_name },
        { name = "DB_USER", value = var.db_username },
        { name = "DB_PASSWORD", value = var.db_password },
        { name = "INIT_SCRIPT", value = var.db_init_script }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.cw_mssql_lg.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "mssql-migrator"
        }
      }
    }
  ])
}

# Cloud Map Service Discovery

resource "aws_service_discovery_private_dns_namespace" "main" {
  name        = "contoso.local"
  description = "Private namespace for ECS services"
  vpc         = module.networking.vpc_id
}

resource "aws_service_discovery_service" "mssql" {
  name = "mssql"

  dns_config {
    namespace_id   = aws_service_discovery_private_dns_namespace.main.id
    routing_policy = "MULTIVALUE"

    dns_records {
      ttl  = 10
      type = "A"
    }
  }
}

locals {
  mssql_dns_name = "${aws_service_discovery_service.mssql.name}.${aws_service_discovery_private_dns_namespace.main.name}"
}

# ECS Services

resource "aws_ecs_service" "mssql_service" {
  name            = "${var.app_name}-mssql-service"
  cluster         = aws_ecs_cluster.cluster.id
  task_definition = aws_ecs_task_definition.mssql_task.arn
  # desired_count defaults to 0 to prevent service from starting
  launch_type = "FARGATE"

  network_configuration {
    subnets          = module.networking.subnet_ids
    security_groups  = [module.networking.sg_id]
    assign_public_ip = true
  }

  service_registries {
    registry_arn = aws_service_discovery_service.mssql.arn
  }

  depends_on = [aws_iam_role_policy_attachment.task_execution_policy]
}

resource "aws_ecs_service" "web_service" {
  name            = "${var.app_name}-web-service"
  cluster         = aws_ecs_cluster.cluster.id
  task_definition = aws_ecs_task_definition.web_task.arn
  # desired_count defaults to 0 to prevent service from starting
  launch_type = "FARGATE"

  network_configuration {
    subnets          = module.networking.subnet_ids
    security_groups  = [module.networking.sg_id]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.web_tg.arn
    container_name   = "web"
    container_port   = 80
  }

  depends_on = [aws_iam_role_policy_attachment.task_execution_policy]
}

# ALB and Target Group for web-service

resource "aws_lb" "web_alb" {
  name               = "${var.app_name}-alb"
  load_balancer_type = "application"
  security_groups    = [module.networking.sg_id]
  subnets            = module.networking.subnet_ids
}

resource "aws_lb_target_group" "web_tg" {
  name        = "${var.app_name}-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = module.networking.vpc_id
  target_type = "ip"
  health_check {
    path                = "/health/readiness"
    interval            = 15
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
  }
}

resource "aws_lb_listener" "web_listener" {
  load_balancer_arn = aws_lb.web_alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.web_tg.arn
  }
}
