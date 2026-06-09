# User Management API

This is a robust, clean RESTful User Management API built using **ASP.NET Core (C#) on .NET 10.0**. 

It provides full CRUD (Create, Read, Update, Delete) capabilities for managing user records, validates all user inputs, includes a custom logging middleware, and secures endpoints using a custom API Key authentication middleware.

---

## Features

1. **Full CRUD Endpoints**:
   - `GET /api/users` - Retrieve all users.
   - `GET /api/users/{id}` - Retrieve a single user by ID.
   - `POST /api/users` - Create a new user.
   - `PUT /api/users/{id}` - Update an existing user.
   - `DELETE /api/users/{id}` - Delete a user.

2. **Data Validation**:
   - Automatic input validation using Data Annotations:
     - `Username`: Required, length must be between 3 and 50 characters.
     - `Email`: Required, must be a valid email address format.
     - `Age`: Required, must be between 18 and 120.
     - `Role`: Required, must be either `Admin`, `User`, or `Manager`.

3. **Custom Logging Middleware**:
   - Intercepts all incoming HTTP requests and outgoing HTTP responses.
   - Logs request method, request path, status code, and the total execution time (in milliseconds) using the built-in logger.

4. **Custom API Key Authentication Middleware**:
   - Secures API endpoints from unauthorized access.
   - Expects an `X-API-Key` header with the correct secret key.
   - **Default API Key**: `SecUserApi2026`
   - Bypasses OpenAPI/Swagger endpoints automatically for documentation inspection.

---

## How to Run the Project

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Run Command
In the root directory of the project, run:
```bash
dotnet run
```
By default, the server will start listening on http://localhost:5000 (or the port specified in `Properties/launchSettings.json`).

---

## API Documentation & Examples

> [!IMPORTANT]
> All requests to `/api/users/*` must include the header:
> `X-API-Key: SecUserApi2026`

### 1. Get All Users
- **Method**: `GET`
- **Path**: `/api/users`
- **Headers**:
  ```http
  X-API-Key: SecUserApi2026
  ```
- **Response** (`200 OK`):
  ```json
  [
    {
      "id": 1,
      "username": "alice",
      "email": "alice@example.com",
      "age": 28,
      "role": "Admin",
      "createdAt": "2026-06-09T13:23:13Z"
    },
    ...
  ]
  ```

### 2. Get User By ID
- **Method**: `GET`
- **Path**: `/api/users/1`
- **Headers**:
  ```http
  X-API-Key: SecUserApi2026
  ```
- **Response** (`200 OK`):
  ```json
  {
    "id": 1,
    "username": "alice",
    "email": "alice@example.com",
    "age": 28,
    "role": "Admin",
    "createdAt": "2026-06-09T13:23:13Z"
  }
  ```

### 3. Create User (POST)
- **Method**: `POST`
- **Path**: `/api/users`
- **Headers**:
  ```http
  X-API-Key: SecUserApi2026
  Content-Type: application/json
  ```
- **Request Body**:
  ```json
  {
    "username": "johndoe",
    "email": "john.doe@example.com",
    "age": 30,
    "role": "User"
  }
  ```
- **Response** (`201 Created`):
  ```json
  {
    "id": 4,
    "username": "johndoe",
    "email": "john.doe@example.com",
    "age": 30,
    "role": "User",
    "createdAt": "2026-06-09T13:25:00Z"
  }
  ```

### 4. Update User (PUT)
- **Method**: `PUT`
- **Path**: `/api/users/4`
- **Headers**:
  ```http
  X-API-Key: SecUserApi2026
  Content-Type: application/json
  ```
- **Request Body**:
  ```json
  {
    "username": "johnsmith",
    "email": "john.smith@example.com",
    "age": 31,
    "role": "Manager"
  }
  ```
- **Response** (`204 No Content`)

### 5. Delete User (DELETE)
- **Method**: `DELETE`
- **Path**: `/api/users/4`
- **Headers**:
  ```http
  X-API-Key: SecUserApi2026
  ```
- **Response** (`204 No Content`)

---

## Testing Middleware and Validation

### 1. Verification of API Key Middleware
Run a request without the `X-API-Key` header:
```bash
curl -i http://localhost:5000/api/users
```
**Expected Response** (`401 Unauthorized`):
```json
{
  "error": "Unauthorized. API Key is missing. Please provide X-API-Key header."
}
```

### 2. Verification of Input Validation
Post a payload with an invalid age (< 18) and a bad email format:
```bash
curl -i -X POST http://localhost:5000/api/users \
  -H "X-API-Key: SecUserApi2026" \
  -H "Content-Type: application/json" \
  -d '{"username": "jd", "email": "not-an-email", "age": 12, "role": "InvalidRole"}'
```
**Expected Response** (`400 Bad Request`):
```json
{
  "errors": {
    "Username": ["Username must be between 3 and 50 characters."],
    "Email": ["Invalid email address format."],
    "Age": ["Age must be between 18 and 120."],
    "Role": ["Role must be Admin, User, or Manager."]
  }
}
```

### 3. Verification of Custom Logging Middleware
When you perform any request, check the console output of your running API. You will see structured log messages printed for every transaction:
```text
info: UserManagementApi.Middleware.RequestResponseLoggingMiddleware[0]
      HTTP Request: GET /api/users received.
info: UserManagementApi.Middleware.RequestResponseLoggingMiddleware[0]
      HTTP Response: GET /api/users responded 200 in 15 ms.
```
