# Customer Account Systems

![Frontend Flow](/docs/frontend-flow.gif)

The Customer Account Systems solution allows management of customer accounts and transactions. The project was built using .NET for the backend services and Angular for the frontend.

Test Site: http://customeraccountsystems.sekami.com/

# Services

## Cas.Customer

Represents the external service. That is why it's only read from and has no dependency on the Cas.Account and Cas.Transaction services. The data is stored in a JSON format and is read for each request.

Customer user data was generated using [JSON Generator](https://www.json-generator.com/)

<details>
<summary>Template Source</summary>

```js
[
  '{{repeat(10)}}',
  {
    id: '{{index()+1}}',
    firstName: '{{firstName()}}',
    lastName: '{{surname()}}',
    dateOfBirth: '{{date(new Date(1970, 0, 1), new Date(2008, 0, 1), "YYYY-MM-dd")}}',
    email: '{{email()}}',
    phone: '+1 {{phone()}}',
    address: '{{integer(100, 999)}} {{street()}}, {{city()}}, {{state()}}, {{integer(100, 10000)}}',
    company: '{{company().toUpperCase()}}',
    photo: 'https://randomuser.me/api/portraits/{{gender() == "male" ? "men" : "women"}}/{{integer(100)}}.jpg',
    gender: '{{gender()}}',
    registered: '{{date(new Date(2014, 0, 1), new Date(), "YYYY-MM-ddThh:mm:ss.fffZ")}}'
  }
]
```
</details>
<br />

***Note**: In case additional security would be required, an additional service would need to be placed in front of this which would filter out relevant data before sending it back.*

## Cas.Account

The account service allows the creation and retrieval of accounts. Each account is tied to a single user and can have zero or more available transactions.

## Cas.Transaction

Contains information endpoints the allow the user to retrieve and create new transactions.

## Cas.Gateway

The gateway uses Ocelot to route all the communication between services. This is the entry point in the system for the client.

## Cas.Client

Houses the client app built by the Cas-SPA project. The Angular SPA application displays the application. Uses customized Material Design components for the presentation. Communicates exclusively through the Gateway.


# Architecture

![Architecture Overview](/docs/architecture-overview.png)


The services were built using an onion architecture. Where the core is self-contained and all the other services only communicate with it using interfaces defined in the core.

Each of the main services (Account and Transaction) contain the following:
Api, Core, Data Access, and Tests.

![Service Architecture](/docs/service-architecture.png)


## API

Houses the startup application and available controllers. These controllers in turn call the Core layer using command and queries. This is done through the mediator pattern using MediatR.

## Core

Keeps the business logic (commands and queries), interfaces, models, and DTOs used by the other layers. Executes the business logic and invokes the functions defined in the interfaces. Has no knowledge of any other layer.

## Data Access

Houses the persistence and infrastructure code required to both save and read data to the database and communicate with external services. Implements the interfaces from the core layer.

***Note**: Ideally the Data Access layer should be split into Persistance and Infrastructure, where Persistance does communication with the database and the Infrastructure layer communicates with external systems. However because of the simplicity of these services (each of those projects would contain 1-2 class) this logic was placed in the Data Access layer.*

## Tests

Currently, each service only has a set of integration tests that run an in-memory database and mock the external communication services. The tests execute by invoking the endpoints defined in the API layer and assert the output from the service.

# Endpoints

There are two ways to access the services. One is through the individual service endpoints. The other is through the Gateway layer.

All service to service communication is done through the Gateway layer. This is also an entry point for the user from where the UI is accessible.

Cas.Customer.Api, Cas.Account.Api and Cas.Transaction.Api all have configured swagger and open API so their individual endpoint documentation can be visible by running the project.

Since the gateway wraps up these services it exposes endpoints in such a way as to mimic the services as if they were a single larger application to the outside world.

## Gateway Endpoints

| Endpoint | Service | Method | Description |
|---|---|---|---|
| /api/customers | Customer | Get | Gets all the customers |
| /api/customers/{id} | Customer | Get | Gets the customer with the specified ID |
| /api/customers/{customerId}/accounts | Account | Get | Get the accounts with the specified customer ID |
| /api/accounts | Account | Post | Creates a new account |
| /api/accounts/{id} | Account | Get | Get the account with the specified ID |
| /api/accounts/{accountId}/transactions | Transaction | Get | Get the transactions with the specified account ID |
| /api/transactions | Transaction | Post | Creates a new transaction |
| /{ui} | Client | Get | All other endpoints route to the UI |

<br />

# Build

Prerequisites:
- Node.js
- .NET 5.0 SDK

From the root of the project:

```
dotnet build CustomerAccountSystems.sln
cd client\Cas-Spa
ng build
```

# Running
From the root of the project:

```
dotnet run --project gateway\Cas.Gateway

dotnet run --project customer\Cas.Customer.Api

dotnet run --project account\Cas.Account.Api

dotnet run --project transaction\Cas.Transaction.Api

cd client\Cas-Spa
npm run build-windows // or build-linux
cd ..\..
dotnet run --project client\Cas.Client

```

Navigate to http://localhost:5000/ to access the application

# CI/CD

Opening a pull request to main the project will be built and tested using the [GitHub Actions Workflows](https://github.com/Mitars/CustomerAccountSystems/actions). An additional job will be run to execute CodeQL analysis.

Once new code is pushed to main the same pipeline will trigger however it will also create a publishable version of the projects, send them to the host server, and run the services.

The deployed version of this can be viewed here: http://customeraccountsystems.sekami.com/


Workflow Files:
[CI/CD](https://github.com/Mitars/CustomerAccountSystems/blob/main/.github/workflows/ci-cd.yml)
[CodeQL](https://github.com/Mitars/CustomerAccountSystems/blob/main/.github/workflows/codeql-analysis.yml)

