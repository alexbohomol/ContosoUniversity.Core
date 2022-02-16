FROM mcr.microsoft.com/mssql/server

COPY . /

# Elevate priviledges to enable run of 'chmod'. See the note here:
# https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-configure-docker?view=sql-server-ver15#buildnonrootcontainer
USER root
RUN chmod +x ./container/db-init.sh

CMD /bin/bash ./container/entrypoint.sh

USER mssql