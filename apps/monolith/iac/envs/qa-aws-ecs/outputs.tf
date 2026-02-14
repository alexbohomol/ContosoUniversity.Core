output "vpc_id" {
  value = module.networking.vpc_id
}

output "sg_id" {
  value = module.networking.sg_id
}

output "subnet_ids" {
  value = module.networking.subnet_ids
}

output "alb_dns_name" {
  description = "DNS name of the Application Load Balancer"
  value       = aws_lb.web_alb.dns_name
}

output "custom_domain_name" {
  description = "Custom domain name pointing to the ALB"
  value       = var.domain_name
}

output "application_url" {
  description = "URL to access the application"
  value       = "http://${var.domain_name}"
}
