# Introduction
Web API for the International Business Men executive team that provides sales data and global currency conversion. Simply legen.. wait for it.. dary!

# Build and Run
1. git clone https://github.com/brugner/ibm-currency-converter.git
2. Open the solution with Visual Studio
3. Update the database connection string in appsettings.Development.json
4. Run the project using either the Http or Https profile
5. Use swagger or the Postman collection to test the API

# Running the Database
Feel free to use the relational database of your preference but if you want to go with the default option, install Docker, open a terminal and run this command:
`docker run --name ibm_postgresql -e POSTGRES_USER=za -e POSTGRES_PASSWORD=only_for_dev -p 5432:5432 -v /data:/var/lib/postgresql/data -d postgres`

# Adding Migrations
Database changes are managed through EF migrations. In order to add a migration:
1. Open a terminal
2. Run: `cd ~/src/IBM.Infrastructure`
3. Run: `dotnet ef --startup-project ../IBM.API/ migrations add Migration_Name -o Data\Migrations`

# Code Coverage
The code coverage is generated using the dotnet test command. In order to generate the report go to the root folder of the test project and run:
`dotnet test --collect:"XPlat Code Coverage"`
Then, use a visualizer to see the report. I recommend using the Visual Studio extension [Fine Code Coverage](https://marketplace.visualstudio.com/items?itemName=FortuneNgwenya.FineCodeCoverage2022).

# Postman Collection
The Postman collection is located in the root folder of the solution. It contains all the endpoints and examples of the requests and responses.

# Swagger
The swagger documentation is available at the root of the API. For example, if you are running the API locally, the swagger documentation will be available at https://localhost:5001/swagger/index.html

# Helpers
Inside the helpers folder there is a web api that simulates the external data providers for rates and transactions. It is not necessary to run it in order to test the API but it is useful to have it running in order to test the API without having to wait for the external data providers to respond.
In fact, the urls provided for this technical test were never up so, this was the first thing I had to develop :)