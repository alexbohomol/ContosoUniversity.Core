output "vpc_id" {
    value = aws_vpc.main.id
}

output "subnets" {
    value = [aws_subnet.public_a.id, aws_subnet.public_b.id]
}

output "rds_endpoint" {
  value = aws_db_instance.contoso_db.endpoint
}

output "rds_address" {
  value = aws_db_instance.contoso_db.address
}
