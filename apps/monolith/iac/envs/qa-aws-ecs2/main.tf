module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "cluster" {
  name = "${var.app_name}-cluster"
}

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

resource "aws_ecs_task_definition" "web_task" {
  family                   = "${var.app_name}-web-tasks"
  cpu                      = "256"
  memory                   = "1024"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
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
        { name = "ASPNETCORE_ENVIRONMENT", value = "Development" },
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
    }
  ])
}

resource "aws_ecs_task_definition" "mssql_task" {
  family                   = "${var.app_name}-mssql-tasks"
  cpu                      = "1024"
  memory                   = "3072"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
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
        { name = "SA_PASSWORD", value = "<YourStrong!Passw0rd>" }
      ]
      healthCheck = {
        command = [
          "CMD-SHELL",
          "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -Q \"SELECT 1\" -C"
        ]
        interval    = 5
        timeout     = 5
        retries     = 10
        startPeriod = 5
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
  container_definitions = jsonencode([
    {
      essential = true
      name      = "mssql-migrator"
      image     = "ghcr.io/alexbohomol/mssql-migrator:latest"
      environment = [
        { name = "DB_HOST", value = "mssql" },
        { name = "DB_USER", value = "sa" },
        { name = "DB_PASSWORD", value = "<YourStrong!Passw0rd>" },
        { name = "INIT_SCRIPT", value = "db-init.sql" }
      ]
    }
  ])
}
