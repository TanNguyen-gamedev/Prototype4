# Prototype 4: Sumo Battle

**Play the Game:** [Junior Programmer on Unity Play](https://play.unity.com/en/games/cc52519c-3a41-4a3b-a2c8-e76e0d26d5a2/junior-programmer)

## Gameplay Mechanic
A physics-based arena brawler. The player acts as a sumo wrestler trying to knock progressively harder waves of enemies off a floating island.

## Core Game Loop
1. **Wave Spawn**: `SpawnManager` drops a wave of enemies and powerups onto the island.
2. **Engage**: Enemies lock onto the player and apply forward forces. Player navigates and rams enemies.
3. **Powerups**: Player collects powerups to gain temporary mass or special abilities (like projectiles or shockwaves).
4. **Progression**: Clearing the island triggers the next, larger wave.

## Dataflow
- **Input**: Movement is mapped relative to the camera's rotation (`RotateCamera`), meaning "Forward" is always the direction the camera faces.
- **AI**: `Enemy` scripts calculate direction vectors toward the player and apply `Rigidbody.AddForce`.
- **Physics**: All interactions rely heavily on Unity's Rigidbody collision and mass properties.