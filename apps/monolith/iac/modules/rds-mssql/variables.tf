
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

variable "db_instance_class" {
  description = "RDS instance size"
  type        = string
  default     = "db.t3.micro"
}

variable "db_username" {
  description = "Master username for RDS"
  type        = string
}

variable "db_password" {
  description = "Master password for RDS"
  type        = string
  sensitive   = true
}
