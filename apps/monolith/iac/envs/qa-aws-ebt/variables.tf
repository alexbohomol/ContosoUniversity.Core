variable "aws_region" {
  type        = string
  description = "AWS region for deployment"
  default     = "eu-central-1"
}

variable "app_name" {
  type        = string
  description = "Application name"
  default     = "contoso-monolith"
}

variable "environment" {
  type        = string
  description = "Specify ASPNETCORE_ENVIRONMENT"
  default     = "Development"
}

variable "db_username" {
  description = "Master username for RDS"
  type        = string
  default     = "admin"
}

variable "db_password" {
  description = "Master password for RDS"
  type        = string
  sensitive   = true
}
