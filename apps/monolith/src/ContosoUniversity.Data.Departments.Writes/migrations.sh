dotnet ef migrations script 0 init --no-transactions --output ../../database/migrations/20211210234830_init.sql
dotnet ef migrations script init remove_pk_courseassignments --no-transactions --output ../../database/migrations/20211211114553_remove_pk_courseassignments.sql
dotnet ef migrations script remove_pk_courseassignments add_view_departmentprojection --no-transactions --output ../../database/migrations/20211224221137_add_view_departmentprojection.sql
