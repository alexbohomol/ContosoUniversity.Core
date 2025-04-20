
resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "contoso-db-subnet-group"
  subnet_ids = var.subnet_ids

  tags = {
    Name = "contoso-db-subnet-group"
  }
}

resource "aws_security_group" "db_sg" {
  name        = "contoso-db-sg"
  description = "Allow MSSQL access from Beanstalk"
  vpc_id      = var.vpc_id

  ingress {
    from_port       = 1433
    to_port         = 1433
    protocol        = "tcp"
    security_groups = [var.eb_security_group_id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "contoso-db-sg"
  }
}

resource "aws_db_instance" "contoso_db" {
  identifier              = "contoso-university-db"
  engine                  = "sqlserver-ex"
  instance_class          = "db.t3.micro"
  allocated_storage       = 20
  username                = var.db_username
  password                = var.db_password
  skip_final_snapshot     = true
  publicly_accessible     = true
  db_subnet_group_name    = aws_db_subnet_group.db_subnet_group.name
  vpc_security_group_ids  = [aws_security_group.db_sg.id]
  backup_retention_period = 0

  tags = {
    Name = "ContosoUniversity-RDS"
  }
}
