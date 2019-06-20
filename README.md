# Driver Reports
This solution was developed in C# with .NET Core 2.2
After installing [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)
You can build, run, and test the project from the command line using the `dotnet` command

## Building and running the project
From within the `src\DriverReports\` directory you can:
* run `dotnet build`
* run `dotnet run input.txt` (`input.txt` contains sample input from the problem statement)

## Running tests
From within the `test\DriverReportsTests\` directory you can:
* run `dotnet test` to run all included unit tests

## Program Design
The main goal of the program architecture is to enable as much extensibility as possible. The requirement to handle multiple commands stood out as a clear point where we may want to extend functionality later on. I wanted to try and make it as easy as possible to be able to add new Commands for the program to handle. My approach to this was to use the [Chain of Responsibility](https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern) pattern, in doing so, it allows for very loose coupling between all components and makes it very simple to add functionality later on. In this case, we have a pretty short "chain" since our Commands do not handle any further dependencies themselves. Each concrete `Command` will perform a simple task and the job of determining which `ICommand` to use is left to another entity further up the chain. The `CommandService` is the "orchestrator" of the commands, it will have the responsibility of determining the proper `ICommand` to run. The `CommandService` does not need to know any of the specific details for each of its commands, therefore, it can depend on the more abstract `ICommand` for each of them, rather than having to know the specific type for each one. Through this use of [Inversion of Control](https://en.wikipedia.org/wiki/Inversion_of_control) we can easily inject all of our dependencies. I am using the [Dependency Injection Container](https://msdn.microsoft.com/en-us/magazine/mt707534.aspx) that comes with the .NET Core Framework here. With the help of abstract classes, I can avoid some duplication of code by pulling all common `Command` logic into the abstract `Command` class, leaving both `DriverCommand` and `TripCommand` with only the essential implementation details needed. Adding a new `Command` is as simple at creating a new class that inherits from `Command`, implementing the required overrides and placing the logic within the `Run` method. Since our Dependency Injection container autmatically register all implementations of `ICommand` within the assembly, there is nothing else to do.

I wanted to keep as much of the business logic out of `Program`'s `Main()` method, since its responsibility should be to serve as an entry-point into the application and as a means to output data. The entry-point to the application is also where we will need to register all of our dependencies so they can be injected later on.

My general philosophy when writing business logic is fail often and fail early. I tend to always create some extension methods in C# that help me do that in a nice way. The thinking here is that `Program`'s `Main()` will take the input from the user, pass it to the business logic and either wait for failures or generate a successful output. 

## Testing Design
I generally like to have a one-to-one mapping of namespaces in the test project to the services in the source project (DriverReports.Services.CommandService --> DriverReportsTests.Services.CommanServiceTests)