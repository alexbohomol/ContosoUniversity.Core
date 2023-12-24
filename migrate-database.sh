#!/bin/bash

docker run --rm \
    --entrypoint /opt/mssql-tools/bin/sqlcmd \
    --workdir /scripts \
    --volume $(pwd)/database:/scripts \
    --network='cunetwork' \
    mcr.microsoft.com/mssql-tools \
    -S cudb -U SA -P '<YourStrong!Passw0rd>' -i 'db-init.sql'
