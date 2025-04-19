
resource "aws_s3_bucket" "deploy_artifacts" {
    bucket = "${var.app_name}-eb-artifacts"
}

resource "aws_elastic_beanstalk_application" "app" {
    name        = var.app_name
    description = "Elastic Beanstalk app for monolith project"
}

resource "aws_s3_object" "app_zip" {
    bucket = aws_s3_bucket.deploy_artifacts.id
    key    = "app.zip"
    source = "../../../app.zip"
    etag   = filemd5("../../../app.zip")
}
