variable "aws_region" {
  type        = string
  description = "AWS region for deployment"
  default     = "eu-central-1"
}

variable "app_name" {
  type        = string
  description = "Application name"
  default     = "contoso-mnlth"
}

variable "environment" {
  type        = string
  description = "Specify ASPNETCORE_ENVIRONMENT"
  default     = "Development"
}

variable "db_username" {
  description = "Master username for RDS"
  type        = string
  sensitive   = true
}

variable "db_password" {
  description = "Master password for RDS"
  type        = string
  sensitive   = true
}

variable "db_init_script" {
  description = "Script to run by migrator"
  type        = string
}

variable "local_ip" {
  description = "Your public IP address for MSSQL access"
  type        = string
  sensitive   = true
}

variable "otel_exporter_otlp_endpoint" {
  description = "Value for OTEL_EXPORTER_OTLP_ENDPOINT"
  type        = string
  default     = "https://otlp-gateway-prod-eu-west-2.grafana.net/otlp"
}

variable "otel_exporter_otlp_protocol" {
  description = "Value for OTEL_EXPORTER_OTLP_PROTOCOL"
  type        = string
  default     = "http/protobuf"
}

variable "otel_exporter_otlp_headers" {
  description = "Value for OTEL_EXPORTER_OTLP_HEADERS"
  type        = string
  sensitive   = true
}

variable "otel_metric_export_interval" {
  description = "Value for OTEL_METRIC_EXPORT_INTERVAL"
  type        = number
  default     = 15000
}

variable "domain_name" {
  description = "Custom domain name for the application"
  type        = string
  default     = "qa-aws-ecs.mnlth.university.contoso.name"
}

variable "route53_zone_id" {
  description = "Route 53 hosted zone ID for contoso.name domain"
  type        = string
}
