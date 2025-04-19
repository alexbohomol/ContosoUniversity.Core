
# define S3

resource "aws_s3_bucket" "deploy_artifacts" {
    bucket = "${var.app_name}-eb-artifacts"
}

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

# define application instance

resource "aws_elastic_beanstalk_application" "app" {
    name        = var.app_name
    description = "Elastic Beanstalk app for monolith project"
}

resource "aws_elastic_beanstalk_environment" "app_env" {
    name                = "${var.app_name}-env"
    application         = aws_elastic_beanstalk_application.app.name
    solution_stack_name = "64bit Amazon Linux 2023 v3.4.0 running .NET 9"

    setting {
        namespace = "aws:autoscaling:launchconfiguration"
        name      = "IamInstanceProfile"
        value     = aws_iam_instance_profile.eb_instance_profile.name
    }

    setting {
        namespace = "aws:elasticbeanstalk:application:environment"
        name      = "ASPNETCORE_ENVIRONMENT"
        value     = "Development"
    }

    setting {
        namespace = "aws:ec2:vpc"
        name      = "VPCId"
        value     = aws_vpc.main.id
    }

    setting {
        namespace = "aws:ec2:vpc"
        name      = "Subnets"
        value     = join(",", [aws_subnet.public_a.id, aws_subnet.public_b.id])
    }

    setting {
        namespace = "aws:autoscaling:launchconfiguration"
        name      = "SecurityGroups"
        value     = aws_security_group.eb_sg.id
    }

    setting {
        name      = "SqlConnectionStringBuilder__DataSource"
        namespace = "aws:elasticbeanstalk:application:environment"
        value     = "${aws_db_instance.contoso_db.address},1433"
    }

    version_label = aws_elastic_beanstalk_application_version.app_version.name
}

resource "aws_elastic_beanstalk_application_version" "app_version" {
    name        = "v1"
    application = aws_elastic_beanstalk_application.app.name
    bucket      = aws_s3_bucket.deploy_artifacts.id
    key         = aws_s3_object.app_zip.key
}

resource "aws_s3_object" "app_zip" {
    bucket = aws_s3_bucket.deploy_artifacts.id
    key    = "app.zip"
    source = "../app.zip"
    etag   = filemd5("../app.zip")
}
