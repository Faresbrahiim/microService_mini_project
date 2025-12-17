# Project Overview
This project is a microservices-based application built using ASP.NET Core Web API.
Each microservice is responsible for a specific business capability and exposes
REST endpoints that communicate using JSON.

The project is developed step by step to understand microservice architecture,
service boundaries, and best practices.

## What is a Microservice ?
A microservice is:

	A small, independent application that does one job, runs on its own,
	and communicates with other services using HTTP (APIs).

Instead of one big application, you build many small ones.

## Monolith vs Microservices (very important)
Monolithic application :
	One project
	One database
	One deployment
	Everything tightly connected
If one part breaks → whole app can fail

Microservices architecture
	Many small projects (services)
	Each service:	
	Has one responsibility
	Can be deployed independently	
	Can fail without killing everything
	Communicate via APIs (HTTP/REST)
## Why use microservices?
### Separation of responsibilities
	Each service does one thing well.
	This makes the code:
		cleaner 
		easier to understand
		easier to maintain
### Scalability 
	If orders increase:
	Scale only Order Service
	Not the whole system
	or add more functionality to one service without affecting others.
### Technology flexibility (later concept)
	Different services can use:
	Different databases (much better )
	Different technologies (later)
### Independent deployment
	Update one service without redeploying the entire application.
	Reduces downtime and risk.
	example: Fix a bug in Payment Service and deploy it without touching Order or User Services or deploying the whole app.
## When NOT to use microservices  ? 
	Project is very small
	No real need for scaling
	the owner is not ready to pay the complexity cost
## needed to build microservices (high level)
Building microservices requires designing small, independent services with clear
responsibilities. Each service runs separately, owns its own data, and
communicates with other services through HTTP-based APIs using JSON.

## What we haven’t covered yet (but will, later)
	How services communicate in detail (sync vs async, events, queues)
	Authentication & authorization between services
	API Gateway (optional, when multiple services exist)
	Database design per service
	Deployment strategies (containers, CI/CD, scaling)
	Error handling, logging, monitoring, tracing
## How to design a microservice
	ask urself  : What is one thing this service should do by itself?
	Rule of thumb: 1 service = 1 business capability.
	Keep services independent 
	Each service owns its own database or data storage.
	Keep services small and focused.
## Microservices in ASP.NET Core Web API

Each microservice will be implemented as a separate ASP.NET Core Web API project.
Services communicate via HTTP (RESTful APIs) and own their databases.
Advanced topics like RabbitMQ or Kafka can be added later for asynchronous communication.

## Project Idea
This project is a microservices-based application built with ASP.NET Core Web API.
It consists of two main microservices: **Student Service** and **Payment Service*



### Student Service
 - Handles student accounts and authentication
  - Register new students
  - Login / logout
- Stores student information in its own database
- Emits events for important actions (e.g., student registration) for other services

### Payment Service
-handles all payment-related operations
  - Make a payment
  - View payment history
- Stores payment data in its own database
- Listens to events from other services (e.g., StudentRegistered) and can emit events (e.g., PaymentMade)

### Communication
- Services communicate via **REST HTTP APIs**
- Event-driven communication will be implemented using **Kafka** for asynchronous messaging

### Goals
- Learn microservices architecture in ASP.NET Core
- Understand service boundaries and data ownership
- Implement communication between services via APIs and Kafka
- Practice database management using **Entity Framework Core**

## you might be confused about kafka  ?
Kafka is used as an event streaming platform for asynchronous communication
between microservices. Services can publish events (producers) and listen
to events (consumers) without directly calling each other, allowing for
decoupled and scalable architecture.
	thing about it as event listeners and event broadcasters in js but more robust

## why use it ? why not just http calls ?
### Decoupling
	Services don’t need to know about each other directly.
	They just listen for events they care about.
### Reliability / persistence
	Events are stored durably in Kafka.
	If a service is down, it can catch up later.
	won’t lose messages, unlike normal HTTP calls
### Scalability
	Services can scale independently.
### Asynchronous processing
	Some operations take time:
		Sending emails
		Generating reports
	Kafka allows these tasks to run in the background, so the main service is fast and responsive (dive later)
## Kafka and RabbitMQ
Technology	Type									Main Use
Kafka		Distributed event streaming platform	Event-driven architecture, data pipelines, high-throughput messaging
RabbitMQ	Message broker (queue system)			Task queues, asynchronous processing, reliable delivery of discrete messages

## Student Service

### Responsibilities:
- Register, login, logout
- Manage student information
- Produce `StudentRegistered` events via Kafka

### Database Table: Students
- Id, Name, Email, PasswordHash, CreatedAt

### API Endpoints:
- POST /api/students/register
- POST /api/students/login
- POST /api/students/logout
- GET /api/students/{id}
- PUT /api/students/{id}

### Kafka Events:
- StudentRegistered (produced)

## Payment Service

### Responsibilities:
- Make payments and view payment history
- Consume `StudentRegistered` events to initialize student accounts
- Produce `PaymentMade` events via Kafka

### Database Table: Payments
- Id, StudentId, Amount, Status, CreatedAt

### API Endpoints:
- POST /api/payments
- GET /api/payments/{studentId}
- GET /api/payments/{id}

### Kafka Events:
- Consumes: StudentRegistered
- Produces: PaymentMade


## Solution Structure

The solution `StudentPaymentMicroservices` contains two ASP.NET Core Web API projects:

1. StudentService
   - Handles student registration, login, logout
   - Produces `StudentRegistered` Kafka events

2. PaymentService
   - Handles payments and payment history
   - Consumes `StudentRegistered` events
   - Produces `PaymentMade` Kafka events

Folder structure:
- Controllers: API endpoints
- Models: EF Core entity classes
- Data: DbContext and database migrations
- Services: optional business logic

##  Workflow: Student + Payment microservices
```
Client
  │
  │ POST /register
  ▼
StudentService
  │
  │ Kafka: StudentRegistered
  ▼
PaymentService
  │
  │ Optional: Kafka PaymentAccountCreated
  ▼
Client can POST /payments
  │
  ▼
PaymentService
  │
  │ Kafka: PaymentMade
  ▼
  ```

# Steps to build the project ... 

# Step 1 : Define the Student entity
why we do this  ?
To represent student data in the Student Service database.
this  entity class only store data and define structure it does not talk to db directly or have kafka logic... for single responsibility principle
Future-proofing: Later, EF Core can automatically create the table from this entity (no need to write SQL manually) thanks to ORM
## step 2 : Define Service Interface (IStudentService) 
as we said it's for single responsibility principle and tight coupling
the contoller will call the service interface not the implementation directly
the iservice interface will define the business logic methods (register, login, logout, get student by id, update student).... 
## step 3 : Implement Service (StudentService)
the service implementation will contain the actual business logic for each method defined in the interface
it will use the repository interface to interact with the database
## step 4 : Define Repository Interface (IStudentRepository)
it will define data access methods (add student, get student by email, get student by id, update student)....
## step 5 : Implement Repository (StudentRepository)
the repository implementation will use EF Core DbContext to perform actual database operations 
will need appDbContext via DI as we said before
## step 6 : DI in program.cs needed to wire up interfaces to implementations
add scoped services for IStudentService and IStudentRepository
## step 7 : Define Controller (StudentController)
the controller will define the API endpoints (register, login, logout, get student by id, update student)
will call the service interface to perform operations
each endpoint will map to a method in the service
each method will its own service method then return appropriate HTTP responses
### note : 
make sure you have db migrations and update database to create the Students table
## step 8 : Implement Kafka Event Publisher  (optional)
## When to use an interface  ?
1 Multiple implementations possible
Example: IStudentRepository
	One implementation: EF Core (SQL Server)
	Another implementation: In-Memory (for tests)	
	Another implementation: Mock or future NoSQL DB
2 Dependency Inversion / decoupling 
3 Unit testing
4 Clear contract / documentation
## add the data context
class inherits from DbContext 
where we define DbSets for each entity (tables)
	options , tables, relationships , configurations ... , validations ...
### bofere moving you will notice that dbConext it not defined or could not found it ....
	To fix that you need to :
1. Install the required NuGet packages:
	- Microsoft.EntityFrameworkCore
	- Microsoft.EntityFrameworkCore.SqlServer
	- Microsoft.EntityFrameworkCore.Tools
## what is Entity Framework Core (EF Core) ?
	EF Core is an Object-Relational Mapper (ORM) for .NET.
	It lets you work with a database using C# classes instead of writing raw SQL
	Translates your C# code into SQL commands automatically
### Why EF Core is useful ?
	1 Productivity
	2 security 
	3 flexibility 
	4 testability
### What is DbContext ?
	DbContext is the main class that EF Core uses to interact with the database
	Think of it as a session or bridge between your C# code and the database
	It tracks your entities, allows queries, and saves changes
### example 
```
using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet represents a table
        public DbSet<Student> Students { get; set; } = null!;
    }
}
```
What each part does ? 
Part							Meaning
DbContextOptions<AppDbContext>	Pass configuration (like which database to use)
DbSet<Student>					Students	Represents Students table in DB
: base(options)					Passes options to the EF Core base class
### AppDbContext can have :
AppDbContext = “everything EF Core needs to know about the database structure and rules”
Tables → DbSets
Constraints / Relationships / Config → OnModelCreating + Fluent API
Connection info → DbContextOptions (SQL Server, SQLite, In-Memory…)
PS : the constructor itself does not  define the databse - it receive it from the dbContext options 
### Where you define the actual database : 
In Program.cs (or Startup.cs), when you register the DbContext with dependency injection
for configuration it better to make it inside  appsettings.json (simpler for dev)
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=StudentDb;Trusted_Connection=True;"
  }
}
```
Then in DI
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
### what happend behind scene here  ?
	whenever you ask for AppDbContext in a constructor (like StudentRepository), DI injects it automatically
	Configures EF Core to use SQL Server as the database provider
	Reads the connection string from appsettings.json (or other configuration sources)
or in other word it gives u configured instance of AppDbContext

## add Infrastructure folder 
why we do this  ?
To separate implementation details (data access, Kafka) from core business logic.
	 -> Core logic (Service layer) = “what should happen”
	 -> Infrastructure layer = “how it happens technically”
but why accually ?
	 -> decoupling , testing, ... again



## About kafka :
Apache Kafka is a message broker used to build event-driven systems .
### in other word 
	Kafka is a system that lets services send messages (events) without knowing who will receive them.
	Instead of one service calling another directly, Kafka sits in the middle and delivers events.
### example 
    StudentService → “StudentRegistered” event → Kafka
	Other services can listen when they want (consume it )
### kafa is : 
	Fast
	Scalable
	Used in real production systems (Netflix, LinkedIn, Uber)
### Why use Kafka in microservices ?
    if we don't use kafka  ?
	Services call each other directly (HTTP calls)
	tight coupling
	if one service is down → whole system can break
	hard to scale
	with kafka  ?
	Services are decoupled / no one depends on another directly
	StudentService doesn’t care who listens
	Other services react independently
	System becomes event-driven

### Example use case
When a student registers:
1 StudentService creates the student
2 StudentService publishes a "StudentRegistered" event to Kafka
3 PaymentService listens for "StudentRegistered" events
  4 PaymentService creates a payment account for the new student

## What is “Real Kafka”?
  Real Kafka means:
Kafka server is actually running
Requires:
	Java
	Kafka broker
	Zookeeper
Events are really sent and consumed
This is what runs in production

## What is “Mock Kafka”?
Fake Kafka is a temporary replacement used during development.
Instead of:
	Running a real Kafka server
	Sending real events
We:
	Log the event
	Or print it to the console
Important :
    Architecture stays the same
	Interfaces stay the same
	No business logic changes

## In this project where going to use mock kafka for these reasons :

	professional and common approach.
	Fast development
	No environment blocking
	Easy switch to real Kafka later

## workflow (personal)
	Start with the interface ,,, (STUdent service example)
	Write the Service (implementation)
	Write the Repository interface and implementation
	Write the Controller
	Write Event Publisher (optional)

Think contracts first → interfaces
Business logic next → service
Data access last → repository
Controller last → orchestrate and call service
Infrastructure / events → plug in at the very last step