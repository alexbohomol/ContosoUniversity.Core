echo "wait 25s for the SQL Server to come up"
sleep 25s

echo "Run the init scripts for database"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -d master -i db-init.sql
