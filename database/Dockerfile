FROM mcr.microsoft.com/mssql-tools

ARG DB_HOST=mssql
ARG DB_USER=sa
ARG DB_PASSWORD=<YourStrong!Passw0rd>
ARG INIT_SCRIPT=db-init.sql

ENV DB_HOST=${DB_HOST}
ENV DB_USER=${DB_USER}
ENV DB_PASSWORD=${DB_PASSWORD}
ENV INIT_SCRIPT=${INIT_SCRIPT}

WORKDIR /scripts

COPY *.sql .
COPY migrations/ migrations/

ENTRYPOINT [ "sh", "-c", "/opt/mssql-tools/bin/sqlcmd -S $DB_HOST -U $DB_USER -P $DB_PASSWORD -i $INIT_SCRIPT" ]
