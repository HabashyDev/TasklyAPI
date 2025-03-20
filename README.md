# **To-Do Application API - README**

![GitHub repo size](https://img.shields.io/github/repo-size/yourusername/todo-api)
![GitHub stars](https://img.shields.io/github/stars/yourusername/todo-api?style=social)
![GitHub license](https://img.shields.io/github/license/yourusername/todo-api)

## **Overview**
The **To-Do Application API** is a RESTful API that allows users to manage their daily tasks efficiently. It supports authentication, task creation, updates, and filtering, providing a robust backend solution for a task management system.

---

## **Features**
✅ User authentication and JWT-based authorization  
✅ CRUD operations for tasks (Create, Read, Update, Delete)  
✅ Task filtering by status  
✅ Secure API endpoints with authentication  
✅ Optional admin role for managing users and tasks  
✅ API documentation with Swagger UI  

---

## **Tech Stack**
- **Backend:** ASP.NET Core Web API
- **Database:** SQL Server (or PostgreSQL, MySQL)
- **ORM:** Entity Framework Core
- **Authentication:** JWT (JSON Web Token)
- **Deployment:** Azure (Optional)

---

## **API Endpoints**

### **1️⃣ Authentication & User Management**
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Authenticate user and return JWT token |

### **2️⃣ Task Management**
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/tasks` | Create a new task (Authenticated user only) |
| `GET` | `/api/tasks` | Get all tasks (Optional filtering by status) |
| `GET` | `/api/tasks/{id}` | Get details of a specific task |
| `PUT` | `/api/tasks/{id}` | Update task details or status |
| `DELETE` | `/api/tasks/{id}` | Delete a task |

---

## **Installation & Setup**
### **Prerequisites**
- .NET SDK installed
- SQL Server running
- Postman or Swagger UI for testing API

### **Steps to Run the API**
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/todo-api.git
   ```
2. Navigate to the project directory:
   ```sh
   cd todo-api
   ```
3. Install dependencies:
   ```sh
   dotnet restore
   ```
4. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
5. Run the API:
   ```sh
   dotnet run
   ```
6. Open `https://localhost:5001/swagger` to test endpoints using Swagger UI.

---

## **Example Requests**
### **User Registration**
```sh
POST /api/auth/register
Content-Type: application/json
{
  "username": "testuser",
  "password": "securepassword"
}
```

### **Login and Receive JWT Token**
```sh
POST /api/auth/login
Content-Type: application/json
{
  "username": "testuser",
  "password": "securepassword"
}
```
_Response:_
```json
{
  "token": "your-jwt-token-here"
}
```

### **Create a Task**
```sh
POST /api/tasks
Authorization: Bearer {token}
Content-Type: application/json
{
  "title": "Finish API project",
  "description": "Complete the final API features and test",
  "status": "Pending"
}
```

_Response:_
```json
{
  "id": 1,
  "title": "Finish API project",
  "description": "Complete the final API features and test",
  "status": "Pending",
  "createdAt": "2025-03-10T12:00:00Z"
}
```

---

## **Future Enhancements**
🚀 Task deadlines and reminders  
🚀 Task prioritization (High, Medium, Low)  
🚀 Assign tasks to multiple users  
🚀 Recurring tasks  
🚀 WebSocket notifications for real-time updates  

---

## **Contributing**
Feel free to contribute to this project! Follow these steps:
1. Fork the repository.
2. Create a new branch (`feature/your-feature`).
3. Commit your changes.
4. Push to your branch and create a pull request.

---

## **License**
This project is licensed under the MIT License.

---

### 🚀 **Happy Coding!** 🚀

