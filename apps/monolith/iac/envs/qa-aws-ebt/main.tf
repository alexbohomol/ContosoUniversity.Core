
# define IAM

resource "aws_iam_role" "eb_instance_role" {
    name = "${var.app_name}-instance-role"

    assume_role_policy = jsonencode({
        Version = "2012-10-17"
        Statement = [{
            Effect = "Allow"
            Principal = {
                Service = "ec2.amazonaws.com"
            }
            Action = "sts:AssumeRole"
        }]
    })
}

resource "aws_iam_role_policy_attachment" "eb_instance_role_managed" {
    role       = aws_iam_role.eb_instance_role.name
    policy_arn = "arn:aws:iam::aws:policy/AWSElasticBeanstalkWebTier"
}

resource "aws_iam_instance_profile" "eb_instance_profile" {
    name = "${var.app_name}-instance-profile"
    role = aws_iam_role.eb_instance_role.name
}

# define database instance

resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "contoso-db-subnet-group"
  subnet_ids = [aws_subnet.public_a.id, aws_subnet.public_b.id]

  tags = {
    Name = "contoso-db-subnet-group"
  }
}

resource "aws_security_group" "db_sg" {
  name        = "contoso-db-sg"
  description = "Allow MSSQL access from Beanstalk"
  vpc_id      = aws_vpc.main.id

  ingress {
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    security_groups = [aws_security_group.eb_sg.id]
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
  publicly_accessible     = false
  db_subnet_group_name    = aws_db_subnet_group.db_subnet_group.name
  vpc_security_group_ids  = [aws_security_group.db_sg.id]
  backup_retention_period = 0

  tags = {
    Name = "ContosoUniversity-RDS"
  }
}

module "beanstalk" {
    source = "../../modules/beanstalk"
    app_name = var.app_name
    vpc_id = aws_vpc.main.id
    iam_instance_profile = aws_iam_instance_profile.eb_instance_profile.name
    subnet_ids = [aws_subnet.public_a.id, aws_subnet.public_b.id]
    eb_security_group_id = aws_security_group.eb_sg.id
    contoso_db_data_source = aws_db_instance.contoso_db.address
}
