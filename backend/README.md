# **Backend-Focused Full-Stack Developer (.NET C#)**

## **Objective**

This take-home test evaluates your ability to develop and integrate a .NET Core (C#) backend with an Angular frontend, focusing on API design, database integration, and basic DevOps practices.

## **Task**

You will build a simple **Loan Management System** with a **.NET Core backend (C#)** exposing RESTful APIs and a **basic Angular frontend** consuming these APIs.

---

## **Requirements**

### **1. Backend (API) - .NET Core**

* Create a **RESTful API** in .NET Core to handle **loan applications**.
* Implement the following endpoints:
  * `POST /loans` → Create a new loan.
  * `GET /loans/{id}` → Retrieve loan details.
  * `GET /loans` → List all loans.
  * `POST /loans/{id}/payment` → Deduct from `currentBalance`.
* Loan example (feel free to improve it):

    ```json
    {
        "amount": 1500.00, // Amount requested
        "currentBalance": 500.00, // Remaining balance
        "applicantName": "Maria Silva", // User name
        "status": "active" // Status can be active or paid
    }
    ```

* Use **Entity Framework Core** with **SQL Server**.
* Create seed data to populate the loans (the frontend will consume this).
* Write **unit/integration tests for the API** (xUnit or NUnit).
* **Dockerize** the backend and create a **Docker Compose** file.
* Create a README with setup instructions.



# **Solution**

## **Assumptions**

Along the way I realized that the information given is extremely precarious, so to simplify things I worked with the following assumptions:

### **1 - Functional:**

**a -** There is no information on how payments are made, whether fixed amount, minimum amounts, total payment, or other, so I opted to work under the following assumptions:

* We have fixed payment rates of $50.
* When the balance is less than $50, the debt is considered settled, the fixed amount is not taken into account and no refund is calculated.

**b -** It is assumed that a client user can have multiple loans.

### **2 - Technical:**

* Configuration is required for linux based containers.
* You can use the base configuration from the official Microsoft images available on dockerHub.
* No custom container configuration is expected.


## **Environment**

In order to execute the project, the following dependencies must be installed:

* Visual Studio 2022 or VSCode
* Docker Desktop
* SQLServer docker image
* SQLSever Management Studio, HeidiSql or another Database Management tool for SQLServer.
* PostMan, Imsomnia or another API Development Software.

*Note: If you are using Windows 11, arm yourself with patience, make a heartfelt prayer to all the deities or supernatural entities of your choice, and pray that everything goes well in the shortest possible time, because Windows 11 is getting worse every day (06/05/2025).*


## **Configurations**

### **1 - JWT**

You may choose to generate your own seeds for JWT token generation, if that is the case, execute the following instructions:

```shell
dotnet user-secrets init
```

Now check the **Fundo.Applications.WebApi.csproj** file, and take the code that has been assigned in tag **UserSecretsId**, since it is necessary to register it in the server or machine where the application is executed.

To register the seed in your machine, use the previously generated key and run:

```shell
dotnet user-secrets set "JWT:secret" "your-long-super-strong-jwt-secret-key"
```

### **2 - Database**

Because of the characteristics of my local environment, everything is containerized as much as possible, and the database is no exception, you may choose to use a locally installed database, in which case, you need to update the configuration according to your environment.

To install the SQL Server 2022 database engine on Docker, run:

```shell
docker pull mcr.microsoft.com/mssql/server
```

In the project you will find the **docker-compose.yaml** file with the necessary information to create, configure and run a container based on the image that was installed in the previous step.

To start the database container, run the following command:

```shell
docker-compose up -d
```

It is important that you verify that you can connect to the database before proceeding to the next step, the connection data is in the **docker-compose** file.

The next step is to create the database and its structure, in this case we make use of the Entity Framework tools, which will take the migration information to recreate our database from scratch. For this, execute:

```shell
dotnet ef database update
```

It is important that you verify that the **FundoLoan** database has been created, and that it contains the following tables:

```text
__EFMigrationsHistory
AspNetRoleClaims
AspNetRoles
AspNetUserClaims
AspNetUserLogins
AspNetUserRoles
AspNetUsers
AspNetUserTokens
Loan
TokenInfos
```

You do not need to worry about populating the database with initial information, this task will be performed automatically when you run the application for the first time.

### **3 - Run the Project**

To run the web application, run:

```shell
dotnet run
```

To run the web application in debug mode, run:

```shell
dotnet watch run
```

## **API**

We have 3 APIs available in the solution:

#### **1 - Loan API**

This is the API that contains the resources to manage the loans, it is available by default at the following URL:

```text
https://localhost:7130/api/loan
```

#### **2 - Auth API**

This is the API that contains the resources to manage the JWT authentication mechanisms, it is available by default at the following URL:

```text
https://localhost:7130/api/auth/
```

#### **3 - OpenApi Swagger**

This service is only available in development mode, and provides us with a Swagger graphical interface, which allows us to obtain information about the APIs available in the application.

```text
https://localhost:7130/swagger/index.html
```