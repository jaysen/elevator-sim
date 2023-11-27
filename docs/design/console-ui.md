
# Console UI Design Notes

## Screen Layout and Organization

Organize the screen into sections: 
- one for status displays, 
- one for showing controls,
- one for logs, 
- and one for command input.

## UI Examples:

### Example Sim Setup
```
-------------------------------------------------
| Sim Setup Controls:                           |
|                                               |
| Floors: 10                                    |
| Elevators: 3                                  |
| Timing: 1000 (millisecs/step)                 |
| Sim: start                                    |
-------------------------------------------------
```

### Example Realtime Console Output - Approach 1
```
-------------------------------------------------
| Elevator Status:                              |
|                                               |
| [1] Floor 5 ↑ | [2] Floor 3 - | [3] Floor 9 ↓ |
|                                               |
|-----------------------------------------------|
| Controls:                                     |
|                                               |
| Floor y Call [Up/Down]                        |
| Floor y Cancel [Up/Down]                      |   
| Elevator x Press Floor y                      |
| Elevator x Cancel Floor y                     |
|                                               |
|-----------------------------------------------|
| Log:                                          |
|                                               |
| Floor 1: Up Call Requested                    |
| Elevator 1: Passenger Enters                  |
| Elevator 1: Passenger Requests Floor 6        |
| Elevator 2: arrived at Floor 3                |
| Elevator 1: moving to Floor 6                 |
-------------------------------------------------

Command > _

```
### Example Realtime Console Output - Approach 2

- Approach 2 Follows the spec more precisely:
- "Users should be able to call an elevator to a specific floor and indicate the number of passengers waiting on each floor." 

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


### Example Realtime Console Output - Sim Goal-Driven Approach 3
Process:
- Approach 3 allows the Sim to operate more smoothly
- User enters Floor Call giving Passengers and Destinations.
  - eg: Call: Floor 3, 2 People, Floor 5
- User can update Passenger Destinations by number of Passengers.
  - eg: Change: Elevator 1, Floors 5 to 6, 2 People
- The sim handles the Passenger behaviour and Elevator movement.
- The Sim will automatically load Passengers into Elevators and move them to their destination floors. 

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



## Other Standard Features:

### Error Handling
Display clear error messages if a user enters an invalid command.


## Additional Features:

### Advanced Error Handling Messages
Provide suggestions for correct commands when an error occurs.

### Interactive Menus
For settings or configurations, provide interactive menus that users can navigate using keyboard input.
Use numbers or key letters to select options from the menu.

### Color-Coded Output
Use different colors for different types of messages or elevator statuses 
Ensure that the color scheme is consistent and intuitive.

### Simulation Controls
Implement controls for starting, pausing, or stopping the simulation.
Allow users to step through the simulation one action at a time for detailed observation.

```
-------------------------------------------------
| Sim Start                                     |
| Sim Pause                                     |
| Sim Resume                                    |
| Sim Stop                                      |
| Sim Restart                                   |
| Sim Step                                      |
|                                               |
-------------------------------------------------
```
