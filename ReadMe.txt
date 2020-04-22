To start web application at the first time

1. Keep connection string if you want to use local database or update file (appsettings.json)
EX: "MTMContext": "Server=localhost\\instancesql;Database=MTM;User Id=username;Password=password;"

2. Open Package Manager Console and execute: dotnet ef database update
OR
	a. Open command prompt
	b. Go to project folder
	c. Execute: dotnet ef database update