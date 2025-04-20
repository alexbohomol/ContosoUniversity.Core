
module "rds" {
  source               = "../../modules/rds-mssql"
  vpc_id               = aws_vpc.main.id
  subnet_ids           = [aws_subnet.public_a.id, aws_subnet.public_b.id]
  eb_security_group_id = aws_security_group.eb_sg.id
  db_password          = var.db_password
  db_username          = var.db_username
}

module "beanstalk" {
  source                 = "../../modules/beanstalk"
  app_name               = var.app_name
  environment            = var.environment
  vpc_id                 = aws_vpc.main.id
  subnet_ids             = [aws_subnet.public_a.id, aws_subnet.public_b.id]
  eb_security_group_id   = aws_security_group.eb_sg.id
  contoso_db_data_source = module.rds.contoso_db_data_source
}
