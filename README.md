# Service Requests

## General Information
This project is build on Visual Studio 2019, with ASPNET Core framework, using webapi to complete the challenge.
For portable and complexity reduction purposes, I will use SQLite database to store the information.

## General considerations
Since the interpretation of the challenge was on my side, and it was not possible to ask questions, here are some things to consider:

* Service Request Model will work as enum on database, but as string on API requests
* Swagger framework is added for testing and debugging purposes
* All requests will have a response with the CODE, and the result will also contain the involved object
* No view is involved on the project
* Emails will be sent after a PUT request updates to Enum Complete

// to complete