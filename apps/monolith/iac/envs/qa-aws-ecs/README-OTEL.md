# OTEL Collector Sidecar для ECS

## Що було додано

1. **CloudWatch Log Group** (`cw_otel_lg`) - для логів OTEL Collector
2. **EFS Access Point** (`efs_ap_otel`) - для зберігання конфігурації OTEL Collector
3. **Volume** в `web_task` - для монтування конфігурації OTEL
4. **OTEL Collector Container** - як sidecar контейнер у `web_task`

## Архітектура

```
┌─────────────────────────────────────────┐
│         ECS Task (web_task)             │
│                                         │
│  ┌──────────┐         ┌──────────────┐  │
│  │   Web    │ OTLP    │     OTEL     │  │
│  │Container │────────▶│  Collector   │  │
│  │          │:4318    │              │  │
│  └──────────┘         └──────┬───────┘  │
│                              │          │
└──────────────────────────────┼──────────┘
                               │
                               │ OTLP
                               ▼
                      External Endpoint
                      (Grafana Cloud)
```

## Конфігурація

### Web Container
- Відправляє метрики/traces на `http://localhost:4318` (OTLP HTTP)
- Використовує протокол `http/protobuf`
- Інтервал експорту метрик: `var.otel_metric_export_interval` (за замовчуванням 15000ms)

### OTEL Collector Container
- **Image**: `otel/opentelemetry-collector-contrib:latest`
- **Essential**: `false` (не блокує запуск web контейнера)
- **Config**: `/etc/otel-collector-config.yaml` (монтується з EFS)
- **Receivers**:
  - OTLP HTTP на порту 4318
  - OTLP gRPC на порту 4317
  - Prometheus scraper (скрейпить `localhost:10000/metrics`)
- **Exporters**:
  - OTLP до зовнішнього endpoint (Grafana Cloud)
  - Logging (для дебагу)

## Deployment

### Перед першим деплоєм

1. Необхідно закинути конфігураційний файл на EFS:
   ```bash
   # Скопіювати otel-collector-config.yaml на EFS в директорію /otel-config/
   # Це можна зробити через тимчасовий ECS task або EC2 instance
   ```

2. Переконайтеся, що змінні встановлені:
   - `otel_exporter_otlp_endpoint` - endpoint Grafana Cloud
   - `otel_exporter_otlp_headers` - Authorization header з API ключем

### Terraform Apply

```bash
cd iac/envs/qa-aws-ecs
terraform init
terraform plan
terraform apply
```

## Моніторинг

### CloudWatch Logs
- Web logs: `/ecs/${var.app_name}-cw-web-lg`
- OTEL Collector logs: `/ecs/${var.app_name}-cw-otel-lg`

### Troubleshooting

1. Перевірити логи OTEL Collector:
   ```bash
   aws logs tail /ecs/contoso-mnlth-cw-otel-lg --follow
   ```

2. Перевірити чи працює OTLP receiver:
   ```bash
   # У контейнері web повинно працювати підключення до localhost:4318
   ```

## Налаштування конфігурації OTEL Collector

Файл конфігурації: `otel-collector-config.yaml`

Щоб оновити конфігурацію:
1. Відредагуйте файл локально
2. Закиньте оновлений файл на EFS (через тимчасовий task)
3. Перезапустіть ECS service:
   ```bash
   aws ecs update-service --cluster ${cluster_name} \
     --service ${service_name} --force-new-deployment
   ```

## Примітки

- OTEL Collector використовує спільну мережу з web контейнером (`awsvpc` network mode)
- Контейнери можуть комунікувати через `localhost`
- Конфігурація зберігається на EFS для персистентності
- OTEL Collector маркований як `essential = false`, тому якщо він впаде, web контейнер продовжить працювати

