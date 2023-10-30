# Stored Procedure Automated Testing Proof-of-concept

This is a pair of C# projects that demonstrate how to accomplish putting a
SQL stored procedure under automated test. Key elements that are included:

* Execution against a real instance of SQL Server - 
  These tests use a testing framework called `testcontainers`. This creates
  and uses a Docker container automatically so that each test has its own
  clean database. This is a _real instance of SQL Server_ for each individual test.
* Self-contained setup - 
  The tests initialize the schema by running the included SQL files to set up
  tables, procedures, etc.
* Fast feedback - 
  Use `dotnet watch test` inside the `DataLayer.Tests` folder to automatically
  re-run tests as changes are made to C# and SQL files.

## Pre-Requisites

* .Net 7.0 SDK
* Docker

## To Run Tests

Use `dotnet test` either in the root of the repository or in the `DataLayer.Tests` folder.

## To Run Tests in Watch Mode

To automatically re-run tests anytime a C# or SQL file changes, use `dotnet watch test` in the `DataLayer.Tests` folder.
