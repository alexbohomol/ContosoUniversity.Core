output "vpc_id" {
  value = module.networking.vpc_id
}

output "sg_id" {
  value = module.networking.sg_id
}

output "subnet_ids" {
  value = module.networking.subnet_ids
}
