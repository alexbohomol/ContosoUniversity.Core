
# define IAM

resource "aws_iam_role" "eb_instance_role" {
    name = "${var.app_name}-instance-role"

    assume_role_policy = jsonencode({
        Version = "2012-10-17"
        Statement = [{
            Effect = "Allow"
            Principal = {
                Service = "ec2.amazonaws.com"
            }
            Action = "sts:AssumeRole"
        }]
    })
}

resource "aws_iam_role_policy_attachment" "eb_instance_role_managed" {
    role       = aws_iam_role.eb_instance_role.name
    policy_arn = "arn:aws:iam::aws:policy/AWSElasticBeanstalkWebTier"
}

resource "aws_iam_instance_profile" "eb_instance_profile" {
    name = "${var.app_name}-instance-profile"
    role = aws_iam_role.eb_instance_role.name
}

module "rds" {
    source = "../../modules/rds-mssql"
    vpc_id = aws_vpc.main.id
    subnet_ids = [aws_subnet.public_a.id, aws_subnet.public_b.id]
    eb_security_group_id = aws_security_group.eb_sg.id
    db_password = var.db_password
    db_username = var.db_username
}

module "beanstalk" {
    source = "../../modules/beanstalk"
    app_name = var.app_name
    vpc_id = aws_vpc.main.id
    iam_instance_profile = aws_iam_instance_profile.eb_instance_profile.name
    subnet_ids = [aws_subnet.public_a.id, aws_subnet.public_b.id]
    eb_security_group_id = aws_security_group.eb_sg.id
    contoso_db_data_source = module.rds.contoso_db_data_source
}
