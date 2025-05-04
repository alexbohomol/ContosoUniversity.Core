resource "aws_security_group" "containers_sg" {
  name        = "${var.app_name}-ecs-sg"
  vpc_id      = module.networking.vpc_id
  description = "Allow ECS containers to talk internally"

  ingress {
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    self        = true
    description = "Allow MSSQL traffic within the same SG"
  }

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    self        = true
    description = "Allow web traffic within ECS task"
  }

  ingress {
    description = "Allow incoming HTTP from the internet"
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    cidr_blocks = ["${var.local_ip}/32"]
    description = "Allow MSSQL from local IP"
  }

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
