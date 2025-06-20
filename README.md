# Survival Chaos Unity Starter

This repository contains a bare-bones Unity project structure intended to recreate the core loop of *Survival Chaos*.

The project lives under `calmproject2/` and uses Unity's Universal Render Pipeline.  The new `_Project` folder under `Assets` organizes scenes, scripts and data in a modular way.

## Folder Layout
- **Assets/_Project/Scenes** – `Bootstrap`, `MainMenu` and `Gameplay` scenes.
- **Assets/_Project/Scripts** – C# scripts grouped under `Core`, `Data`, `Units`, `UI` and `AI`.
- **Assets/_Project/ScriptableObjects** – placeholder directories for races, units and upgrades.
- **Assets/_Project/Prefabs** – units, heroes, buildings and effects.
- **Assets/_Project/Art** and **Audio** – for future assets.

All directories include a `.gitkeep` so the structure is tracked even before content is added.

## Getting Started
Open the `calmproject2` folder with Unity 2022.3 or newer.  You can extend the stub scripts in `Assets/_Project/Scripts` to build out gameplay features such as wave spawning, upgrades and hero management.


## Core Manager
The project now includes simple implementations of several gameplay managers:
- **GameManager** – tracks active players and overall game state.
- **LaneManager** and **SpawnSystem** – coordinate lane wave timers and unit spawning.
- **EconomyManager** – grants periodic gold income to all players.
- **UpgradeManager**, **HeroManager**, and **SpecialWeaponManager** – handle upgrades, hero summons, and global weapons.
These components communicate via a lightweight `EventBus` utility under `Scripts/Core`.

