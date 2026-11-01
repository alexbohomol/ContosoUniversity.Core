locals {
  web_scaling = {
    min_capacity        = 2
    max_capacity        = 10
    requests_per_second = 50
    cpu_target          = 60
    scale_out_cooldown  = 60
    scale_in_cooldown   = 300
  }
}

resource "aws_appautoscaling_target" "web" {
  min_capacity       = local.web_scaling.min_capacity
  max_capacity       = local.web_scaling.max_capacity
  resource_id        = "service/${aws_ecs_cluster.cluster.name}/${aws_ecs_service.web_service.name}"
  scalable_dimension = "ecs:service:DesiredCount"
  service_namespace  = "ecs"
}

resource "aws_appautoscaling_policy" "web_requests" {
  name               = "${var.app_name}-web-requests"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.web.resource_id
  scalable_dimension = aws_appautoscaling_target.web.scalable_dimension
  service_namespace  = aws_appautoscaling_target.web.service_namespace

  target_tracking_scaling_policy_configuration {
    # ALB target tracking measures requests per target over one minute.
    target_value       = local.web_scaling.requests_per_second * 60
    scale_out_cooldown = local.web_scaling.scale_out_cooldown
    scale_in_cooldown  = local.web_scaling.scale_in_cooldown

    predefined_metric_specification {
      predefined_metric_type = "ALBRequestCountPerTarget"
      resource_label         = "${aws_lb.web_alb.arn_suffix}/${aws_lb_target_group.web_tg.arn_suffix}"
    }
  }
}

resource "aws_appautoscaling_policy" "web_cpu" {
  name               = "${var.app_name}-web-cpu"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.web.resource_id
  scalable_dimension = aws_appautoscaling_target.web.scalable_dimension
  service_namespace  = aws_appautoscaling_target.web.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value       = local.web_scaling.cpu_target
    scale_out_cooldown = local.web_scaling.scale_out_cooldown
    scale_in_cooldown  = local.web_scaling.scale_in_cooldown

    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }
  }
}
