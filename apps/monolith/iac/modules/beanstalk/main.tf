
resource "aws_elastic_beanstalk_environment" "app_env" {
  name                = "${var.app_name}-env"
  application         = aws_elastic_beanstalk_application.app.name
  solution_stack_name = "64bit Amazon Linux 2023 v3.4.0 running .NET 9"

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "IamInstanceProfile"
    value     = var.iam_instance_profile
  }

  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "ASPNETCORE_ENVIRONMENT"
    value     = "Development"
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "VPCId"
    value     = var.vpc_id
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "Subnets"
    value     = join(",", var.subnet_ids)
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "SecurityGroups"
    value     = var.eb_security_group_id
  }

  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "SqlConnectionStringBuilder__DataSource"
    value     = "${var.contoso_db_data_source},1433"
  }

  version_label = aws_elastic_beanstalk_application_version.app_version.name
}

resource "aws_elastic_beanstalk_application_version" "app_version" {
  name        = "v1"
  application = aws_elastic_beanstalk_application.app.name
  bucket      = aws_s3_bucket.deploy_artifacts.id
  key         = aws_s3_object.app_zip.key
}
