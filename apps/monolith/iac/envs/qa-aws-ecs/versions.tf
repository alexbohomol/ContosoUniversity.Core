terraform {
  required_version = ">= 1.5.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "5.94.0"
    }
  }

  backend "s3" {
    bucket         = "contoso-tf-state-1ed3a58b-c137-4a46-b4a2-6e9265c0e719"
    key            = "terraform.tfstate"
    region         = "eu-central-1"
    dynamodb_table = "tfstate-locks"
    encrypt        = true
  }
}

provider "aws" {
  region = var.aws_region
}
