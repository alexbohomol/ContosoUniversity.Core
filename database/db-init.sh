echo "wait 25s for the SQL Server to come up"
sleep 25s

echo "running set up script"
#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -d master -i db-init.sql