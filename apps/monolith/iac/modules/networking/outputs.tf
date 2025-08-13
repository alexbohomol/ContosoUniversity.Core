output "vpc_id" {
    value = aws_vpc.main.id
}

output "sg_id" {
  value = aws_vpc.main.default_security_group_id
}

output "subnet_ids" {
    value = [aws_subnet.public_a.id, aws_subnet.public_b.id]
}
