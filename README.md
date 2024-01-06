# Elevator Simulation

## Project Overview

The Elevator Simulation System is a console application developed in C# that simulates the movement of elevators within a large building. The primary goal is to optimize passenger transportation efficiently while adhering to Object-Oriented Programming (OOP) principles for modularity and maintainability.

### Required Features:
The console application will include the following key features:

1. Real-Time Elevator Status:

    Display the real-time status of each elevator, including its current floor, direction of movement, whether it's in motion or stationary, and the number of passengers it is carrying.
    
2. Interactive Elevator Control:

    Allow users to interact with the elevators through the console application. Users should be able
    to call an elevator to a specific floor and indicate the number of passengers waiting on each floor.

3. Multiple Floors and Elevators Support
    
    Design the application to accommodate buildings with multiple floors and multiple elevators.
    Ensure that elevators can efficiently move between different floors.

4. Efficient Elevator Dispatching:

    Implement an algorithm that efficiently directs the nearest available elevator to respond to an
    elevator request. Minimize wait times for passengers and optimize elevator usage.

5. Passenger Limit Handling:

    Consider the maximum passenger limit for each elevator. Prevent the elevator from becoming
    overloaded and handle scenarios where additional elevators might be required.

6. Consideration for Different Elevator Types:

    Although the challenge focuses on passenger elevators, consider the existence of other elevator
    types, such as high-speed elevators, glass elevators, and freight elevators. Plan the application's
    architecture to accommodate future extension for these types.

7. Real-Time Operation:

    Ensure that the console application operates in real-time, providing immediate responses to user
    interactions and accurately reflecting elevator movements and status.


## Console Application Instructions:

### Simulation setup:
On startup, the application will prompt the user to setup the simulation, as below:

```
Simulation setup:
Enter the number of floors:
Enter the number of elevators
Enter the default elevator capacity
Enter the default elevator speed (millisecs/step):
```

### Simulation Display:
Once the simulation is setup, the application will display the following information:


```
-----------------------------------------------------------------
Elevator Status:
[Elevator 1] Floor 3 ↑ | Passengers: 4 | Destinations: 5,6      
[Elevator 2] Floor 2 - | Passengers: 0 | Destinations: None
[Elevator 3] Floor 1 - | Passengers: 0 | Destinations: None
-----------------------------------------------------------------
Actions:
- Press 'a' to add a passenger to the simulation
- Press 'm' to add multiple passengers to the simulation
- Press 'q' to exit the simulation
-----------------------------------------------------------------
Log:

-----------------------------------------------------------------
```

### Simulation Actions:
The user can interact with the simulation by pressing the following keys:
- Press 'a' to add a passenger to the simulation
- Press 'm' to add multiple passengers to the simulation
- Press 'q' to exit the simulation

#### Adding a passenger:
When the user presses 'a' to add a passenger, the application will prompt the user to enter the passenger's starting floor and destination floor, as below:

```
Enter the passenger's origin floor number:
Enter the destination floor number:
```

#### Adding multiple passengers:
When the user presses 'm' to add multiple passengers, the application will prompt the user to enter the number of passengers to add, then prompt the user to enter the passenger's starting floor and destination floor for each passenger:

```
Enter the number of passengers to add:
Enter the passenger's origin floor number:
Enter the destination floor number:
```

#### Exiting the simulation:
Pressing 'q' exits the simulation.


### Simulation Log:
The simulation log displays the following information:

- When a passenger or passengers are added to the simulation. 
- When an elevator is dispatched to that floor
- When an elevator stops at a floor
- When passengers are loaded onto an elevator
- When passengers are unloaded onto a floor
- When the elevator is full and another elevator needs to be dispatched

Example log:

```
> *** Added 20 passengers to floor 2 going Up to floor 30 ****
> Elevator 3 dispatched to floor 2 going Up
> Elevator 3 stopped at floor 2
> Elevator 3 loaded 10 passengers from floor 2 going Up
> Elevator 3 couldn't take everyone. 10 still left on floor 2 going Up
> Elevator 4 dispatched to floor 2 going Up
> Elevator 4 stopped at floor 2
> Elevator 4 loaded 10 passengers from floor 2 going Up
> Elevator 3 stopped at floor 30
> Elevator 3 unloading 10 passengers onto floor 30
> Elevator 4 stopped at floor 30
> Elevator 4 unloading 10 passengers onto floor 30

```

## Console Application Constraints, Limitations and Known Issues:

### Display Size Issue:
The console application attempts to show a dynamic display of status with a rolling log of events. 
To do this, the application redraws the console window dynamically.

**Because of this as of this version, the console application is limited by the console window size.**
If the console window is too small for the required display, the app will . 
Because of this, the console application is best viewed in a maximized console window.

With a large number of floors and elevators, a maximized console window may still not be large enough to display the entire simulation.
This is a known issue and will be addressed in future versions.


## Dispatching Strategies:
As of now there is only one dispatching strategy implemented: **Nearest Elevator Dispatching**.
Future versions will include additional dispatching strategies.

