
output "contoso_db_data_source" {
    value = aws_db_instance.contoso_db.address
    description = "Connection string for Beanstalk environment (DataSource)"
}
