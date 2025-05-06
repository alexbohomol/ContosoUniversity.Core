FROM mcr.microsoft.com/mssql-tools

WORKDIR /scripts

COPY *.sql .
COPY migrations/ migrations/

ARG DB_USER=sa
ARG DB_PASSWORD=<YourStrong!Passw0rd>
ARG INIT_SCRIPT=db-init.sql

ENV DB_USER=${DB_USER}
ENV DB_PASSWORD=${DB_PASSWORD}
ENV INIT_SCRIPT=${INIT_SCRIPT}

ENTRYPOINT [ "sh", "-c", "/opt/mssql-tools/bin/sqlcmd -S mssql -U $DB_USER -P $DB_PASSWORD -i $INIT_SCRIPT" ]
