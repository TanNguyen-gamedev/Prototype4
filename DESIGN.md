# Prototype 4: Architecture & Lessons Learned

## Overview
Prototype 4 is a physics-based sumo brawler. The architectural focus is heavily centered around rigid body physics, zero-allocation spatial queries, and managing complex concurrent state via Coroutine dictionaries.

## Architecture & Implementation Details

### 1. Camera-Relative Movement
- **Input Mapping**: Player input vectors are separated from global world space. In `PlayerController.cs`, movement is applied using `_focalPoint.transform.forward`, meaning the "Forward" input always matches the direction the camera (`RotateCamera.cs`) is currently facing.

### 2. Rigidbody Physics-Driven Logic
- **FixedUpdate Mandate**: All movement logic for the Player and Enemies strictly resides in `FixedUpdate` using `Rigidbody.AddForce(direction, ForceMode.Impulse)`. This completely avoids kinematic `transform.Translate` modifications, ensuring realistic mass-based collisions and knockbacks.
- **Enhanced Gravity**: Like Prototype 3, falling speed is enhanced using `ForceMode.Acceleration` to create a heavier, more impactful feel for the sumo wrestler.

### 3. Advanced State Tracking (Coroutines & Dictionaries)
- **Concurrent Powerups**: In `PowerUpManager.cs`, a `Dictionary<PowerUpType, Coroutine>` is used to track active power-up states. 
- **State Reset Safety**: If a player collects the same power-up twice, the dictionary safely stops the existing Coroutine and starts a new one, resetting the timer without causing conflicting behavior or overlapping effects.

### 4. Event-Driven Wave Management (`SpawnManager.cs`)
- **Static Events**: Instead of using expensive `FindObjectsOfType<Enemy>()` in `Update` to check if a wave is cleared, `SpawnManager` maintains a simple `List<Enemy> _activeEnemies`.
- **Decoupled Reporting**: The `Enemy` base class raises a static event (`public static event Action<Enemy> OnDeath`) when it falls below a certain Y-axis threshold. `SpawnManager` listens to this event and simply removes the instance from its list, efficiently triggering the next wave when the list is empty.

## Lessons Learned
- **Time.deltaTime in FixedUpdate**: Redundant `Time.deltaTime` multipliers were stripped from `AddForce` calls inside `FixedUpdate()`. Since `FixedUpdate` operates on a fixed timestep, using `Time.deltaTime` makes the physics engine apply force inconsistently based on the rendering framerate rather than the physics timestep.
- **Zero-Allocation Physics**: Utilizing `Physics.OverlapSphereNonAlloc` (for effects like SmashDown) and relying on static event tracking rather than array-allocating functions (`FindObjectsOfType`) ensures maximum performance without GC spikes during intense waves.