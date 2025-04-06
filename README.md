# DOCO Users API
This project is a robust implementation of .NET 8, utilizing the latest features of C# 12 to showcase Domain-Driven Design (DDD), CQRS, and Clean Architecture principles. 
The application leverages ASP.NET Core Minimal APIs for efficient routing and handling, providing a scalable and maintainable codebase.

# Key Features:
**Vertical Slice Architecture:** Organized around business use cases, with clear boundaries between features.

**CQRS Implementation:** Separation of command (write) and query (read) models using MediatR library.

**CQRS Validation Pipeline:** Integrated with MediatR and FluentValidation for streamlined validation and command/query processing.

**Note :** 
This project runs Azure SQL Database


# Steps to Run the Project:
**Configure Azure SQL Database**
Step 1: Add Your Local IP to Azure SQL Server Firewall

Log in to the Azure Portal (will provide you the Azure credentials through email)

Navigate to Azure SQL Database → Networking

Under Firewall Rules, click "Add client IP" to allow your local machine to access the database

Step 2: Update Database Connection in appsettings.json
