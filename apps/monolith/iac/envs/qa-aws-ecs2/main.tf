module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "cluster" {
  name = "${var.app_name}-cluster"
}

resource "aws_ecs_task_definition" "web_task" {
  family                   = "${var.app_name}-tasks-family"
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
