variable "aws_region" {
  default = "eu-central-1"
}

variable "app_name" {
  default = "contoso-monolith"
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
