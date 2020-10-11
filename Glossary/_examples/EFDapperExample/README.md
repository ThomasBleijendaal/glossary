# Entity Framework Core + Dapper CQRS example

Important stuff:

- Specify MaxLengths on strings for EFCore, otherwise nvarchar(max) columns will hurt performance.
- Use Design Time DbContext Factory to simplify migration creation.
- Use DbConnection from DbContext.
- 