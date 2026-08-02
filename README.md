# Chronicles of the Lost Dungeon

Chronicles of the Lost Dungeon is a third-person Unity dungeon crawler built around combat, progression, loot, save data, and multi-platform presentation. The game includes five levels, a boss encounter, level unlocks, a JSON save system, and a modular codebase designed to be easy to extend.

## Overview

The project focuses on building a complete dungeon-crawler experience with reusable gameplay systems. The architecture separates player control, enemy AI, UI, saving, level flow, inventory, and audio so each part can evolve without forcing large changes elsewhere.

## Features

- Five-level progression system
- Main menu, settings menu, and level select flow
- Third-person movement and combat
- Melee, ranged, and special abilities
- Enemy AI with state-based behaviour
- Boss UI and defeat handling
- Event-driven HUD updates
- JSON save/load persistence
- Inventory sorting with Quick Sort
- Object pooling for reusable gameplay objects
- Platform-specific UI for mobile builds
- NUnit tests for core gameplay systems

## Controls

### PC / Web

- Move: `W`, `A`, `S`, `D`
- Attack: left mouse button
- Block: right mouse button
- Special attack: `E`
- Ranged attack: `Q`
- Sprint: left shift
- Jump: space
- Dodge: left alt

### Mobile

Mobile builds use on-screen controls that are enabled automatically through platform-specific UI logic.

## Setup

1. Open the project in Unity.
2. Let Unity import the assets and generate the project files.
3. Open the main scene or the menu scene from `Assets/_Scenes`.
4. Press Play in the Unity Editor to test gameplay.

The project was built with Unity 6000.4.6f1.

## Build Targets

The project is intended to support:

- PC builds
- Android or iOS mobile builds
- WebGL builds

For presentation purposes, make sure each build demonstrates the same core gameplay and menu flow.

## Testing

The repository includes NUnit tests for core systems, including:

- Object pooling
- Player damage handling
- Health upgrades
- Damage upgrades
- Gauntlet behaviour

## Architecture Notes

The project uses several reusable programming patterns and techniques:

- Singleton managers for persistent services
- Interfaces for abilities and damageable objects
- Delegates and events for decoupled UI updates
- JSON serialization for save data
- State-based enemy AI
- Quick Sort for loot ordering
- Object pooling for repeated objects
