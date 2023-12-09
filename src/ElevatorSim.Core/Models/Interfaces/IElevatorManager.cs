﻿using ElevatorSim.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.Core.Models.Interfaces;

/* Central Elevator Manager / Manager:
 *
 * Manages the state of all floors.
 * Manages the state of all elevators.
 * 
 * Adds passengers to floors.
 * Dispatches the elevators to floors when up/down requests are made - efficiently.
 * Manages Elevator movement between floors.
 * Handles Elevator stops at floors.
 * 
*/

public interface IElevatorManager
{
    List<IElevator> Elevators { get; }
    List<IFloor> Floors { get; }

    SortedSet<int> FloorsRequestingUp { get; }
    SortedSet<int> FloorsRequestingDown { get; }

    bool Setup(int floorCount, int elevatorCount, int defaultElevatorCapacity, int defaultElevatorSpeed);

    bool AddElevator(IElevator elevator);

    bool AddPassengerToFloor(int floor, int destinationFloor);

    Task<IElevator> DispatchElevatorToFloorAsync(int floor, Direction direction);

    Task<IAsyncResult> MoveElevatorToFloorAsync(IElevator elevator, int floorNum);

    Task<IAsyncResult> ProcessFloorStop(IElevator elevator, int floorNum);

    bool Reset();

}