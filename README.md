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


## Console UI Design

### Sim Setup
```
-------------------------------------------------
| Sim Setup Controls:                           |
|                                               |
| Floors: 10                                    |
| Elevators: 3                                  |
| Timing: 1000 (millisecs/step)                 |
| Sim: start                                    |
|                                               |
-------------------------------------------------
```

### Example Realtime Console Output - Standard Approach
- This approach follows the spec closely: "Users should be able to call an elevator to a specific floor and indicate the number of passengers waiting on each floor." 

Process: 
  - User enters floor number and direction button requested (up/down) with number of passengers waiting on that floor.
  - Lift arrives at floor, going (up/down) and user enters number of passengers entering the lift.
  - User enters destination floor numbers by number of passengers.
    - eg for Elevator 1, 2 passengers want to go to floor 5
    - User enters: Load: Elevator 1, Floor 5, 2 People 
    - User repeats Load for each destination floor.
  - User enters Close: Elevator 1 to close the doors.
  - People get off at their destination floor automatically.
  - User can describe an update to destination floor in an Elevator by number of Passengers.
    - Change: Elevator 1, Floors 5 to 6, 2 People

Output:
```
-----------------------------------------------------------------
| Elevator Status:                                              |
|                                                               |
| [1] Floor 3 ↓ | Passengers: 4 | Destinations: 5 (2), 6 (2)    |
| [2] Floor 6 - | Passengers: 0 | Destinations: None            |
| [3] Floor 1 - | Passengers: 0 | Destinations: None            |
|                                                               |
|---------------------------------------------------------------|
| Elevator Controls:                                            |
|                                                               |
| Command Format: [Action] [Details]                            |
|                                                               |
| Actions:                                                      |
| - Call: floor <floor>, <up/down>, <number> people             |
| - Load: elevator <el>, floor <dest>, <number> people          |
| - Close: elevator <el>                                        |
| - Change: elevator <el>, floors <orig> to <new>, <num> people |
|                                                               |
|---------------------------------------------------------------|
| Log:                                                          |
|                                                               |
| > Floor 3: Down Call - 4 passengers waiting                   |
| > Elevator 1: moving to Floor 3                               |
| > Elevator 1: arrived at Floor 3                              |
| > Floor 3: Elevator 1: 2 passengers exited                    |
| > Floor 3: Elevator 1: 2 entered for Floor 5                  |
| > Floor 3: Elevator 1: 2 entered for Floor 6                  |
| > Elevator 1: moving to Floor 5                               |
-----------------------------------------------------------------
Command > _

```
The Commands above are: 
- Call: floor 3, down, 4 people
- Load: elevator 1, floor 5, 2 people 
- Load: elevator 1, floor 6, 2 people
- Close: elevator 1


### Example Realtime Console Output - Sim Goal-Driven Approach
Process:
- This approach allows the Sim to operate more smoothly using less user input.
- User enters Floor Call giving Passengers and Destinations.
  - eg: Call: Floor 3, 2 People, Floor 5
- User can update Passenger Destinations by number of Passengers.
  - eg: Change: Elevator 1, Floors 5 to 6, 2 People
- The sim handles the Passenger behaviour and Elevator movement.
- The Sim will automatically load Passengers into Elevators and move them to their destination floors. 
- This does not follow the letter of the spec, so will use it as an alternative approach, while keeping the first approach as the standard.

Output:
```
-----------------------------------------------------------------
| Elevator Status:                                              |
|                                                               |
| [1] Floor 3 ↓ | Passengers: 4 | Destinations: 5 (2), 6 (2)    |
| [2] Floor 6 - | Passengers: 0 | Destinations: None            |
| [3] Floor 1 - | Passengers: 0 | Destinations: None            |
|                                                               |
|---------------------------------------------------------------|
| Elevator Controls:                                            |
|                                                               |
| Command Format: [Action] [Details]                            |
|                                                               |
| Actions:                                                      |
| - Call: floor <floor>, <number> people, floor <dest>          |
| - Cancel: floor <floor> <number> people, floor <dest>         |
| - Change: elevator <el>, floors <orig> to <new>, <num> people |
|                                                               |
|---------------------------------------------------------------|
| Log:                                                          |
|                                                               |
| > Floor 3: Down Call - 4 passengers waiting                   |
| > Elevator 1: moving to Floor 3                               |
| > Elevator 1: arrived at Floor 3                              |
| > Floor 3: Elevator 1: 2 passengers exited                    |
| > Floor 3: Elevator 1: 2 entered for Floor 5                  |
| > Floor 3: Elevator 1: 2 entered for Floor 6                  |
| > Elevator 1: moving to Floor 5                               |
-----------------------------------------------------------------
Command > _

```
The Commands above are: 
- Call: floor 3, 2 people, floor 5
- Call: floor 3, 2 people, floor 6

