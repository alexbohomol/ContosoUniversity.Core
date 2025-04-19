variable "app_name" {
  type = string
  description = "Application Name"
  default = "contoso-monolith"
}

variable "iam_instance_profile" {
  type = string
  description = "IAM instance profile"
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
