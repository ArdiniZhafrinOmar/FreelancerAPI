# Freelancer API  

## Features:
- User Registration & Authentication (JWT)
- CRUD Operations with SQL Server
- Error Handling, Pagination, Caching
- Unit Testing with xUnit & Moq

---

## Prerequisites:
- .NET 8 SDK (or latest) → [Download Here](https://dotnet.microsoft.com/en-us/download)
- SQL Server & SSMS (for database) → [Download Here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Postman (for testing) → [Download Here](https://www.postman.com/downloads/)
- Git (for cloning repo) → [Download Here](https://git-scm.com/)


---

## How to Run:
1. Clone the repo  
   ```bash
   	git clone https://github.com/ArdiniZhafrinOmar/FreelancerAPI.git
	cd FreelancerAPI

2. Set Up the Database
   - Open SQL Server Management Studio (SSMS)
   - Create a new database:
   ```sql
	CREATE DATABASE FreelancerDB;

   - Open appsettings.json and update the connection string:
   ```json
	"ConnectionStrings": {
    		"DefaultConnection": "Server=localhost;Database=FreelancerDB;Trusted_Connection=True;TrustServerCertificate=True"
	}

   - Run the database migrations:
   ```bash
	dotnet ef database update

## Run the API:
   ```bash
	dotnet run

## Swagger UI:
1. Open Swagger UI:
	https://localhost:<port>/swagger/index.html

2. Test API Endpoints Directly in Swagger UI

## API Endpoint:
Method	Endpoint	Description
GET	/api/users	Get all users
POST	/api/users	Add new user
PUT	/api/users/{id}	Update user
DELETE	/api/users/{id}	Delete user
POST	/api/auth/login	Authenticate user & get JWT token

## Running Test
1. Uses xUnit and Moq for unit testing.
   ```bash
	dotnet test

## Contact
Developed by Ardini Zhafrin Binti Omar
📧 Email: ardinizhafrin@gmail.com
🔗 GitHub: https://github.com/ArdiniZhafrinOmar
