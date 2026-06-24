# Prototype 4: Architecture & Lessons Learned

## What We Learnt
- **Camera-Relative Movement**: Separated the input vector mapping from global world space, translating input through the `RotateCamera`'s transform.
- **Physics-Driven Logic**: Entirely reliant on zero-allocation `Physics` interactions within `FixedUpdate`. Avoids kinematic transformations to ensure realistic mass-based collisions.
- **Zero-Allocation Physics & AI**: Utilized `Physics.OverlapSphereNonAlloc` for powerup detection and simple vector math for AI pathing to ensure maximum performance without GC spikes.
- **Advanced State Tracking**: Implemented a Dictionary to track active Coroutines in `PowerUpManager`, allowing multiple powerups to run concurrently and handle duration resets safely.
- **Event-Driven Wave Management**: Replaced expensive `FindObjectsOfType` checks with static events (`Enemy.OnDeath`) so the `SpawnManager` efficiently tracks when a wave is cleared.