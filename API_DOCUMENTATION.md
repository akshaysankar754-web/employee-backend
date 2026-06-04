# Employee Portal API Documentation

## Overview

This document describes the API implemented in `employee-backend` for the Employee Portal project.

- API name: `EmployeePortalAPI`
- Framework: ASP.NET Core 8 Web API
- Authentication: JWT Bearer token
- Database: MySQL via Entity Framework Core
- Default local base URL: `http://localhost:5029`
- Default HTTPS base URL: `https://localhost:7115`
- Swagger UI in development: `/swagger`

## Runtime Configuration

### Local configuration

From `appsettings.json`:

- MySQL connection string:
  `server=localhost;port=3307;database=employeeportaldb;user=user;password=user`
- JWT issuer: `EmployeePortalAPI`
- JWT audience: `EmployeePortalUsers`

### Docker configuration

From `appsettings.Docker.json`:

- MySQL connection string:
  `server=app-mysql;port=3306;database=employeeportaldb;user=user;password=user`
- JWT issuer: `EmployeePortalAPI`
- JWT audience: `EmployeePortalUsers`

## Authentication

The API uses JWT Bearer authentication.

- Public endpoints:
  - `POST /api/Auth/register`
  - `POST /api/Auth/login`
  - `GET /api/Dashboard`
- Protected endpoints:
  - All `/api/Employee` endpoints

### Authorization header

For protected endpoints, send:

```http
Authorization: Bearer <your-jwt-token>
```

### Token contents

The generated JWT contains these claims:

- `name`
- `email`
- `role`

### Token expiry

- Token lifetime: 2 hours from login

## Data Models

### RegisterDto

```json
{
  "name": "string",
  "email": "string",
  "password": "string"
}
```

### LoginDto

```json
{
  "email": "string",
  "password": "string"
}
```

### Employee

```json
{
  "id": 0,
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phone": "string",
  "department": "string",
  "position": "string",
  "salary": 0,
  "dateOfJoining": "2026-06-04T00:00:00"
}
```

### User entity

Stored in the database:

```json
{
  "id": 0,
  "name": "string",
  "email": "string",
  "passwordHash": "string",
  "role": "User"
}
```

Notes:

- Passwords are never stored in plain text.
- Passwords are hashed using BCrypt.
- New users default to role `User`.

## API Endpoints

## 1. Auth API

Base route: `/api/Auth`

### 1.1 Register user

- Method: `POST`
- Route: `/api/Auth/register`
- Auth required: `No`

#### Request body

```json
{
  "name": "Anoop",
  "email": "anoop@example.com",
  "password": "Password123"
}
```

#### Success response

- Status: `200 OK`

```json
{
  "message": "User Registered Successfully"
}
```

#### Error responses

- Status: `400 Bad Request`

```json
"Email already exists"
```

#### Behavior

- Checks whether a user already exists with the same email.
- Hashes the password using BCrypt.
- Creates a new user record.

### 1.2 Login user

- Method: `POST`
- Route: `/api/Auth/login`
- Auth required: `No`

#### Request body

```json
{
  "email": "anoop@example.com",
  "password": "Password123"
}
```

#### Success response

- Status: `200 OK`

```json
{
  "message": "Login Successful",
  "token": "<jwt-token>"
}
```

#### Error responses

- Status: `401 Unauthorized`

```json
"Invalid Email"
```

- Status: `401 Unauthorized`

```json
"Invalid Password"
```

#### Behavior

- Validates the email against the `Users` table.
- Verifies the password using BCrypt.
- Returns a signed JWT token valid for 2 hours.

## 2. Employee API

Base route: `/api/Employee`

All endpoints in this controller require a valid Bearer token.

### 2.1 Get all employees

- Method: `GET`
- Route: `/api/Employee`
- Auth required: `Yes`

#### Success response

- Status: `200 OK`

```json
[
  {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phone": "9876543210",
    "department": "IT",
    "position": "Developer",
    "salary": 55000,
    "dateOfJoining": "2025-01-15T00:00:00"
  }
]
```

#### Behavior

- Returns all records from the `Employees` table.

### 2.2 Get employee by ID

- Method: `GET`
- Route: `/api/Employee/{id}`
- Auth required: `Yes`

#### Path parameter

- `id` - employee ID

#### Success response

- Status: `200 OK`

```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "9876543210",
  "department": "IT",
  "position": "Developer",
  "salary": 55000,
  "dateOfJoining": "2025-01-15T00:00:00"
}
```

#### Error response

- Status: `404 Not Found`

```json
"Employee not found"
```

### 2.3 Add employee

- Method: `POST`
- Route: `/api/Employee`
- Auth required: `Yes`

#### Request body

```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane.smith@example.com",
  "phone": "9123456789",
  "department": "HR",
  "position": "Manager",
  "salary": 65000,
  "dateOfJoining": "2026-01-10T00:00:00"
}
```

#### Success response

- Status: `200 OK`

```json
{
  "message": "Employee Added Successfully",
  "employee": {
    "id": 2,
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@example.com",
    "phone": "9123456789",
    "department": "HR",
    "position": "Manager",
    "salary": 65000,
    "dateOfJoining": "2026-01-10T00:00:00"
  }
}
```

### 2.4 Update employee

- Method: `PUT`
- Route: `/api/Employee/{id}`
- Auth required: `Yes`

#### Path parameter

- `id` - employee ID

#### Request body

```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane.smith@example.com",
  "phone": "9000000000",
  "department": "HR",
  "position": "Senior Manager",
  "salary": 70000,
  "dateOfJoining": "2026-01-10T00:00:00"
}
```

#### Success response

- Status: `200 OK`

```json
{
  "message": "Employee Updated Successfully",
  "employee": {
    "id": 2,
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@example.com",
    "phone": "9000000000",
    "department": "HR",
    "position": "Senior Manager",
    "salary": 70000,
    "dateOfJoining": "2026-01-10T00:00:00"
  }
}
```

#### Error response

- Status: `404 Not Found`

```json
"Employee not found"
```

#### Behavior

- Finds the employee by ID.
- Updates all editable fields.
- Saves changes to the database.

### 2.5 Delete employee

- Method: `DELETE`
- Route: `/api/Employee/{id}`
- Auth required: `Yes`

#### Path parameter

- `id` - employee ID

#### Success response

- Status: `200 OK`

```json
{
  "message": "Employee Deleted Successfully"
}
```

#### Error response

- Status: `404 Not Found`

```json
"Employee not found"
```

## 3. Dashboard API

Base route: `/api/Dashboard`

### 3.1 Get dashboard summary

- Method: `GET`
- Route: `/api/Dashboard`
- Auth required: `No`

#### Success response

- Status: `200 OK`

```json
{
  "totalEmployees": 20,
  "totalDepartments": 4,
  "latestEmployees": [
    {
      "id": 20,
      "firstName": "Ravi",
      "lastName": "Kumar",
      "email": "ravi@example.com",
      "phone": "9988776655",
      "department": "Finance",
      "position": "Analyst",
      "salary": 48000,
      "dateOfJoining": "2026-05-10T00:00:00"
    }
  ]
}
```

#### Behavior

- Counts all employees.
- Counts distinct departments from employees.
- Returns the latest 5 employees by descending `Id`.

## HTTP Status Summary

- `200 OK` for successful requests
- `400 Bad Request` when registration uses an existing email
- `401 Unauthorized` when login credentials are invalid or a protected endpoint is called without a valid token
- `404 Not Found` when an employee does not exist

## Swagger

Swagger is enabled only in the `Development` environment.

Typical local Swagger URLs:

- `http://localhost:5029/swagger`
- `https://localhost:7115/swagger`

Swagger includes Bearer token support. After login:

1. Copy the JWT token from the login response.
2. Open Swagger UI.
3. Click `Authorize`.
4. Enter `Bearer <token>`.
5. Call protected employee endpoints.

## Example API Flow

### Register

```http
POST /api/Auth/register
Content-Type: application/json
```

```json
{
  "name": "Anoop",
  "email": "anoop@example.com",
  "password": "Password123"
}
```

### Login

```http
POST /api/Auth/login
Content-Type: application/json
```

```json
{
  "email": "anoop@example.com",
  "password": "Password123"
}
```

### Use token for employee APIs

```http
GET /api/Employee
Authorization: Bearer <jwt-token>
```

## Important Implementation Notes

- JSON responses use camelCase because JSON serialization is configured with camel-case naming.
- The dashboard endpoint returns anonymous-object properties declared in PascalCase in C#, but they are serialized as camelCase in JSON.
- No explicit model validation attributes are defined on DTOs or entities.
- No pagination, filtering, or sorting exists on the employee list endpoint.
- Employee create and update currently accept the entity model directly.
- Role-based authorization is not enforced even though the JWT includes a role claim.

## Source Reference

Main backend files used for this documentation:

- [Program.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Program.cs)
- [AuthController.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Controllers/AuthController.cs)
- [EmployeeController.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Controllers/EmployeeController.cs)
- [DashboardController.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Controllers/DashboardController.cs)
- [RegisterDto.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/DTOs/RegisterDto.cs)
- [LoginDto.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/DTOs/LoginDto.cs)
- [Employee.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Models/Employee.cs)
- [User.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Models/User.cs)
- [ApplicationDbContext.cs](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Data/ApplicationDbContext.cs)
- [appsettings.json](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/appsettings.json)
- [appsettings.Docker.json](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/appsettings.Docker.json)
- [launchSettings.json](/c:/Users/Anoop.K/Downloads/Angular%20Apps/Employee/employee-backend/Properties/launchSettings.json)
