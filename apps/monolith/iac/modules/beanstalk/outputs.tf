output "beanstalk_env_url" {
  value = aws_elastic_beanstalk_environment.app_env.endpoint_url
  description = "Beanstalk environment URL"
}

output "beanstalk_app_cname" {
  value       = aws_elastic_beanstalk_environment.app_env.cname
  description = "Public friendly Beanstalk environment URL"
}
