module "networking" {
  source   = "../../modules/networking"
  app_name = var.app_name
}

resource "aws_security_group" "eb_sg" {
  name   = "${var.app_name}-sg"
  vpc_id = module.networking.vpc_id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
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

module "rds" {
  source               = "../../modules/rds-mssql"
  vpc_id               = module.networking.vpc_id
  subnet_ids           = module.networking.subnet_ids
  eb_security_group_id = aws_security_group.eb_sg.id
  db_password          = var.db_password
  db_username          = var.db_username
}

module "beanstalk" {
  source                 = "../../modules/beanstalk"
  app_name               = var.app_name
  environment            = var.environment
  vpc_id                 = module.networking.vpc_id
  subnet_ids             = module.networking.subnet_ids
  eb_security_group_id   = aws_security_group.eb_sg.id
  contoso_db_data_source = module.rds.contoso_db_data_source
}
