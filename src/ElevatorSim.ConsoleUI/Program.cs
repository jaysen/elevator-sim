using Microsoft.Extensions.DependencyInjection;
using ElevatorSim.Core.Models;
using ElevatorSim.Core.Models.Interfaces;
using ElevatorSim.Core.Services;
using ElevatorSim.Core.Services.Interfaces;


// Setup DI
//TODO: add factory for multiple elevator types
//TODO: add DI for Dispatch Strategies
var serviceProvider = new ServiceCollection()
    .AddSingleton<IBuildingSimFactory, StdBuildingSimFactory>()
    .AddTransient<ConsoleApplication>() 
    .BuildServiceProvider();


// Resolve and run the ConsoleApplication
using var scope = serviceProvider.CreateScope();
var consoleApp = scope.ServiceProvider.GetService<ConsoleApplication>();
consoleApp?.Run(args);

