echo "wait 25s for the SQL Server to come up"
sleep 25s

echo "1st step - run the init scripts to create database, schemas, etc."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -d master -i db-init.sql

# 2nd step - migrate database just created
echo "Placeholder: migrate database [NOACTION]"

# 3rd step - seed data to database just created and migrated
echo "Placeholder: seed database [NOACTION]"
