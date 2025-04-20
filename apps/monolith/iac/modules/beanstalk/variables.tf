variable "app_name" {
  type        = string
  description = "Application Name"
  default     = "contoso-monolith"
}

variable "environment" {
  type        = string
  description = "Specify ASPNETCORE_ENVIRONMENT"
  default     = "Development"
}

variable "vpc_id" {
  type        = string
  description = "VPC for Beanstalk environment"
}

variable "subnet_ids" {
  type        = list(string)
  description = "Subnets for Beanstalk environment"
}

variable "eb_security_group_id" {
  type        = string
  description = "Security group to attach to Beanstalk instances"
}

variable "contoso_db_data_source" {
  type        = string
  description = "Connection string for Beanstalk environment (DataSource)"
}
