#!/bin/bash

dotnet format whitespace \
    --no-restore \
    --exclude ./src/*/Migrations

dotnet format style \
    --no-restore \
    --exclude ./src/*/Migrations

dotnet format analyzers \
    --no-restore \
    --exclude ./src/*/Migrations
