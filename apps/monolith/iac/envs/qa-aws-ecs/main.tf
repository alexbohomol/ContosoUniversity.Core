module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_ecs_cluster" "this" {
  name = "${var.app_name}-cluster"
}

resource "aws_cloudwatch_log_group" "this" {
  name              = "/ecs/${var.app_name}-cloudwatch-log-group"
  retention_in_days = 7
}

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

resource "aws_iam_role_policy_attachment" "task_exec_policy" {
  role       = aws_iam_role.task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_ecs_task_definition" "this" {
  family                   = "${var.app_name}-task-definition"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "512"
  memory                   = "1024"
  network_mode             = "awsvpc"
  execution_role_arn       = aws_iam_role.task_execution.arn
  container_definitions = templatefile("${path.module}/task-definition.json", {
    web_image : "ghcr.io/alexbohomol/cuweb:latest"
    awslogs-group : aws_cloudwatch_log_group.this.name
    awslogs-region : var.aws_region
  })
}

resource "aws_ecs_service" "web" {
  name            = "${var.app_name}-web"
  cluster         = aws_ecs_cluster.this.id
  task_definition = aws_ecs_task_definition.this.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = module.networking.subnet_ids
    assign_public_ip = true
    security_groups  = [aws_security_group.containers_sg.id]
  }

  lifecycle {
    ignore_changes = [task_definition]
  }
}
