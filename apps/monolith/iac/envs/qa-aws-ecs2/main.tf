module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "cluster" {
  name = "${var.app_name}-cluster"
}

resource "aws_security_group" "ecs_sec_grp" {
  name        = "${var.app_name}-sec-grp"
  vpc_id      = module.networking.vpc_id
  description = "Allow ECS containers to talk internally"

  ingress {
    description = "Allow MSSQL traffic within the same SG"
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    self        = true
  }

  ingress {
    description = "Allow web traffic within ECS task"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    self        = true
  }

  ingress {
    description = "Allow incoming HTTP from the internet"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "Allow incoming MSSQL from the internet"
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # ingress {
  #   from_port   = 1433
  #   to_port     = 1433
  #   protocol    = "tcp"
  #   cidr_blocks = ["${var.local_ip}/32"]
  #   description = "Allow MSSQL from local IP"
  # }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.app_name}-sg"
  }
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
