echo
echo "------------------------"
echo "Migrate 'Courses' schema"
echo "------------------------"
dotnet ef database update --project ../../src/ContosoUniversity.Data.Courses.Writes/ContosoUniversity.Data.Courses.Writes.csproj

echo
echo "----------------------------"
echo "Migrate 'Departments' schema"
echo "----------------------------"
dotnet ef database update --project ../../src/ContosoUniversity.Data.Departments.Writes/ContosoUniversity.Data.Departments.Writes.csproj

echo
echo "-------------------------"
echo "Migrate 'Students' schema"
echo "-------------------------"
dotnet ef database update --project ../../src/ContosoUniversity.Data.Students.Writes/ContosoUniversity.Data.Students.Writes.csproj
