echo "wait 25s for the SQL Server to come up"
sleep 25s

echo "running set up script"
# 1st step - run the setup script to create the DB and the schema in the DB
# TODO: create a combined script to run all steps in one shot
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -d master -i db-create.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -d master -i db-login.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '<YourStrong!Passw0rd>' -d master -i db-user.sql

# 2nd step - migrate database just created
echo "Placeholder: migrate database [NOACTION]"

# 3rd step - seed data to database just created and migrated
echo "Placeholder: seed database [NOACTION]"
