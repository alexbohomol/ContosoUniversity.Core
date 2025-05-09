module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "cluster" {
  name = "${var.app_name}-cluster"
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
