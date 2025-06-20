# Survival Chaos Unity Starter

This repository contains a bare-bones Unity project structure intended to recreate the core loop of *Survival Chaos*.  
The project lives in **calmproject2/** and targets **Unity 2022.3 LTS** with the Universal Render Pipeline (URP).

---

## Folder Layout

| Path | Purpose |
|------|---------|
| **Assets/_Project/Scenes** | `Bootstrap`, `MainMenu`, and `Gameplay` scenes |
| **Assets/_Project/Scripts** | C# sources grouped under `Core`, `Data`, `Units`, `UI`, and `AI` |
| **Assets/_Project/ScriptableObjects** | Placeholder assets for races, units, upgrades, bonuses, etc. |
| **Assets/_Project/Prefabs** | Unit, hero, building, and VFX prefabs |
| **Assets/_Project/Art** / **Audio** | Import pipelines for future visual and sound assets |

> Every directory includes a `.gitkeep` so the structure is tracked even before content is added.

---

## Core Managers

The starter includes minimal implementations of several gameplay systems:

| Manager | Responsibility |
|---------|----------------|
| **GameManager** | Tracks active players and overall game state (Pregame → Running → Victory/Loss). |
| **LaneManager** | Holds references to lanes and applies the default wave interval. |
| **SpawnSystem** | Listens for `WaveSpawnEvent` and instantiates units at the proper spawn points. |
| **EconomyManager** | Grants periodic gold income to all active players. |
| **UpgradeManager** | Stores per-player upgrade levels and applies modifiers. |
| **HeroManager** | Handles hero-summon cooldowns and instantiation. |
| **SpecialWeaponManager** | Manages global-weapon cooldowns and firing. |
| **EventBus** | Lightweight publish/subscribe utility that decouples all of the above systems. |

---

## Getting Started

1. Clone the repo and open **calmproject2/** with Unity 2022.3 or newer.  
2. Open the **Bootstrap** scene and press **Play** – the stub managers should initialise without errors.  
3. Extend the scripts in **Assets/_Project/Scripts** to flesh out gameplay features such as wave spawning, upgrades, AI, and hero abilities.

### Unit Prefabs & Combat

Basic unit combat has been implemented in `UnitController`. Attach the script to a prefab and assign a `UnitData` asset with `baseHP`, `baseAttack`, `attackCooldown`, and `attackRange` values. When spawned, units will look for the closest enemy within range and automatically deal damage every `attackCooldown` seconds. Units are destroyed when their health reaches zero.

Happy modding!
