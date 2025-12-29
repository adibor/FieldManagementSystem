# Field Management System – Backend API

## Overview
Field Management System is a backend Web API built with ASP.NET Core for managing agricultural fields and controllers.
The system allows users to manage fields, assign controllers, and restrict access to data based on authentication and authorization rules.

The project was designed with clean architecture principles, Entity Framework Core, JWT-based authentication, and basic automated tests.

---

## Technology Stack
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Swagger / OpenAPI**
- **xUnit (Tests)**

---

## Core Entities

### User
Represents a system user (user or administrator).
- Identified by email
- Authenticated using JWT
- Can own fields
- Can be assigned to controllers

### Field
Represents an agricultural field.
- Owned by a specific user
- Supports CRUD operations
- Ownership is enforced by authorization rules

### Controller
Represents a device or automation unit.
- Can be assigned to multiple users
- User-controller relationship is modeled via a join table

### UserController
Join entity for a many-to-many relationship between users and controllers.
Includes an optional `Role` field reserved for future authorization extensions.

---

## Authentication & Authorization

### Authentication
- Implemented using **JWT Bearer Authentication**
- Login endpoint automatically creates a user if not found (simplified authentication model)
- Token includes user identifier claims

### Authorization
- API endpoints are protected using `[Authorize]`
- Ownership-based authorization is enforced in controllers
- Admin-only endpoints are restricted via role-based authorization
- Non-authorized access results in `403 Forbidden`

---

## API Capabilities

### Fields
- Get all fields owned by the current user
- Get field by ID (ownership enforced)
- Create field
- Update field (PUT)
- Partial update (PATCH)
- Delete field

### Controllers
- Create controllers
- List all controllers
- Update controller (PUT)
- Partial update (PATCH)
- Assign controller to a user- only by admin.
- Unassign controller from the current user

### Users
- Admin-only user listing
- Regular users are forbidden from accessing administrative endpoints

---

## Swagger
Swagger UI is enabled in development mode.

After running the application locally, navigate to:
https://localhost:<port>/swagger


Swagger supports JWT authentication using the **Authorize** button.

---

## Design Notes

- Authentication was implemented without passwords for simplicity and can be extended in the future.
- A role field exists for user-controller relationships but is reserved for future authorization use.
