output "beanstalk_env_url" {
  value       = module.beanstalk.beanstalk_env_url
  description = "Beanstalk environment URL"
}

output "beanstalk_app_cname" {
  value       = module.beanstalk.beanstalk_app_cname
  description = "Public friendly Beanstalk environment URL"
}

output "contoso_db_data_source" {
  value       = module.rds.contoso_db_data_source
  description = "Connection string for Beanstalk environment (DataSource)"
}
