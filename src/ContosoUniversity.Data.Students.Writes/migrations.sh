dotnet ef migrations script 0 init --no-transactions --output ../../database/migrations/20211215202621_init.sql
dotnet ef migrations script init enable_grade_nullability --no-transactions --output ../../database/migrations/20211216181657_enable_grade_nullability.sql
