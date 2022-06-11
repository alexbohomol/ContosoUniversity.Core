FROM mcr.microsoft.com/mssql/server

COPY ["container", "scripts"]
COPY ["*.sql", "scripts"]

WORKDIR /scripts

# Elevate priviledges to enable run of 'chmod'. See the note here:
# https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-configure-docker?view=sql-server-ver15#buildnonrootcontainer
USER root
RUN chmod +x ./db-init.sh

CMD /bin/bash ./entrypoint.sh

USER mssql