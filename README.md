# APIBaseTemplate
## Introduction
As written on my personale README page, APIBaseTemplate is a project that shows how I would go about structuring and managing a basic API for a project with no special requirements. So the project includes a little bit of all the common needs that I found myself dealing with over time, from database management, authentication, authorization, logging, and so on.

During development, I will document my decisions and try to understand the reasoning behind them.

The basic idea is to "replicate" a sky scanner where users can compare and book their flights. I will not go vertical by adding one feature after the other to our flights, but I will go horizontal to integrate different areas of the API, like the already mentioned authentication, authorization, but also security etc. etc.

## Tech stack
Let's start by talking about the technology stack that I want to integrate into the project - that is, into the API:
- .NET 7 as framework.
- SQL Server database with Entity Framework in code first.

## Data layer
Regarding the data layer, what I decided to do is to use the repository pattern in combination with unit of work. I know that some developers do not prefer this choice and prefer to use only Entity framework without adding additional abstraction, but personally I am comfortable with the two mentioned patterns that allow a greater separation of responsibilities making the code more modular, testable and easily replaceable.

So in the code inside the "Repositories" folder you will find:
- DataContext.cs (and its interface), as well as the class that uses the Entity Framework's DbContext to perform queries, mainly CRUD operations. The methods are implemented generically so that all the different entities can be handled without any problems.
- The base repository, BaseRepository.cs (and its interface), from which the others are derived.
- UnitOfWork.cs (and its interface), through which we can control the transactions and data context of each repository.
- Finally, a UnitOfWorkFactory.cs class (and its interface), which is a factory for retrieving the UnitOfWork we want.

Still on the data layer, we have the entities that are part of the project. In this case, the "Datamodel" folder contains all the classes that map the database tables and their DTOs. The mapping of the classes is not in the OnModelCreating event of the DbContext, but has been separated for clarity, each entity in fact has its own class dedicated to mapping.
