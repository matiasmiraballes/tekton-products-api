## About The Project

Products api for a Tekton Labs challenge.

## Getting Started

### Prerequisites

This application doesn't rely on any external dependencies. Just ensure you have the following installed on your machine:

- [.NET SDK 7](https://dotnet.microsoft.com/download/dotnet/7.0)

# Brief explanation about the application architecture

The architecture of the application is based on the N-Tier architecture and the CQRS pattern. It also uses the Aggregate Root pattern to keep the business rules clustered in a single place and ensure that the data remains consistent between each data update.

### N-Tier architecture

N-tier architecture, also known as multi-tier architecture, is a design pattern commonly used in software development to organize code into distinct layers or tiers. The primary purpose of this architecture is to separate concerns and improve the modularity, maintainability, and scalability of the application. In the case of this application, we will find the following layers:
1. **Presentation Layer (TektonProductsApi project):**
-	Responsible for handling user interactions and displaying information to users.
-	In a .NET Core application, this layer often includes the web application or user interface components.
2. **Application Layer:**
-	Enforces business rules, processes data, and orchestrates interactions between other layers.
-	Independent of the specific user interface or data access mechanisms.
3. **Domain Layer:**
-	Represents the heart of the application and encapsulates the core business entities, logic, and rules.
-	Contains domain models, entities, value objects, and the behavior that directly corresponds to the real-world problem the application is solving.
-	Should be independent of any infrastructure or application-specific details.
4. **Infrastructure Layer + Data Access Layer:**
-	Deals with lower-level concerns such managing interactions with the data storage (database, file system, external APIs, etc.).
-	Responsible for executing database queries, handling transactions, and mapping data between the application and the storage medium.
-	Contains entities, repositories, and data access logic.
-	Supports the overall application but is not directly involved in business logic.

### CQRS pattern

CQRS, which stands for Command Query Responsibility Segregation, is a design pattern that separates the responsibilities of handling commands (write operations) and queries (read operations) in a system. It suggests that the components used to read data should be different from those used to write data, providing a clear distinction between the two operations. This pattern helps to simplify and scale the architecture by tailoring the data model for either reading or writing.
The CQRS pattern enables several optimizations that can enhance the performance and scalability of an application. Some of the key optimizations allowed by CQRS are:
1.	**Separated Databases for Read and Write:**
-	One of the primary optimizations offered by CQRS is the ability to use separate databases for the read and write models.
-	The write database is optimized for transactional operations and may use a traditional relational database.
-	The read database is designed for efficient querying and can be denormalized to support specific read requirements.
-	This separation allows each database to be tailored to its specific workload, improving performance for both read and write operations.
2.	**Eventual Consistency:**
-	CQRS often embraces the concept of eventual consistency, which means that the state of the read model might not be immediately updated after a write operation.
-	Instead of synchronous updates, the write model publishes events indicating changes.
-	The read model subscribes to these events and updates its state asynchronously, achieving eventual consistency.
-	This approach is especially beneficial for scenarios where strict consistency is not a primary requirement, and a small delay in reflecting changes is acceptable.
3.	**Skipping Validations for Queries:**
-	Commands, which represent write operations, often require validation to ensure data integrity and business rule adherence.
-	In contrast, queries, representing read operations, may not require the same level of validation.
-	CQRS allows skipping certain validations for queries, reducing the processing overhead associated with read operations.
-	This can result in faster response times for queries, as they don't need to go through the same validation logic as commands.
4.	**Optimized Data Structures for Queries:**
-	The read model in CQRS can be optimized for specific query requirements.
-	Instead of relying on a single, normalized data structure, the read model can be denormalized and structured to support efficient querying.
-	This allows the application to quickly retrieve and present data in a format tailored to the needs of the user interface or reporting requirements.
5.	**Separate Scaling for Read and Write Components:**
-	Since read and write operations are handled by separate components in CQRS, each can be scaled independently based on its workload.
-	The read side can be scaled horizontally to handle increased query loads, while the write side can be optimized for transactional consistency.

### Aggregate Root pattern

The Aggregate Root pattern is a concept in Domain-Driven Design (DDD) that defines a specific entity within an aggregate as the root and entry point for operations on the aggregate. In DDD, an aggregate is a cluster of domain objects that are treated as a single unit for data changes. The Aggregate Root is responsible for ensuring the consistency and integrity of the aggregate.

## Extra project requirements

#### Document the application using Swagger
- The default Swagger url is `https://localhost:7208/swagger/index.html`

#### Implement the solution using TDD
- The application has integration testing in place to guide the development of the endpoints.

#### Use any kind of persistence to store products information locally
- I decided to use EF Core In-Memory to persist the data in order to reduce the dependencies needed to run the application.

#### Log the response time of each request in a flat file.
- Serilog is used to log information of each request processed by the application.
