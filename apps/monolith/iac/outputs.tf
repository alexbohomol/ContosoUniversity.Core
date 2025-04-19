output "vpc_id" {
    value = aws_vpc.main.id
}

output "subnets" {
    value = [aws_subnet.public_a.id, aws_subnet.public_b.id]
}

output "beanstalk_env_url" {
    value = aws_elastic_beanstalk_environment.app_env.endpoint_url
    description = "Beanstalk environment URL"
}

output "beanstalk_app_cname" {
  value       = aws_elastic_beanstalk_environment.app_env.cname
  description = "Public friendly Beanstalk environment URL"
}

output "rds_endpoint" {
  value = aws_db_instance.contoso_db.endpoint
}

output "rds_address" {
  value = aws_db_instance.contoso_db.address
}
