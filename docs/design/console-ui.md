
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