# Survival Chaos Unity Starter

This repository provides a lightweight Unity framework inspired by the custom Warcraft III map *Survival Chaos*.  It is meant as a learning playground or starting point for a more complete strategy game.  All Unity content lives in **calmproject2/** and the project targets **Unity&nbsp;2022.3 LTS** using the Universal Render Pipeline (URP).

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
| **MatchmakingManager** | Example implementation of a simple Elo-based 4-player queue. |

### Multiplayer Matchmaking

The sample project includes a very small **MatchmakingManager** and helper
classes that implement an Elo-based queue for four-player matches. Every second
the queue is sorted by rating, and groups whose rating spread falls within the
smallest tolerance of the waiting players are popped and handed off to
`GameManager`. Each ticket widens its own tolerance over time so longer-waiting
players match more quickly without forcing global settings.

Matchmaking supports two **match types**:
 - **Normal** – each player keeps their chosen race.
 - **Chaos** – races are randomly assigned when the match forms.


### Player Profiles & Statistics

Persistent data for each player lives in the `User` class. It exposes methods
to record match results, units produced or destroyed, gold spending and more.
The class keeps running totals of gold and crystal earned, calculates win rate,
tracks which races a player uses most often and stores damage or unit-count
metrics for richer post-game analytics.


---

## Getting Started

1. Clone the repo and open **calmproject2/** with Unity 2022.3 or newer.  
2. Open the **Bootstrap** scene and press **Play** – the stub managers should initialise without errors.  
3. Extend the scripts in **Assets/_Project/Scripts** to flesh out gameplay features such as wave spawning, upgrades, AI, and hero abilities.

### Unit Prefabs & Combat

Basic unit combat has been implemented in `UnitController`. Attach the script to a prefab and assign a `UnitData` asset with `baseHP`, `baseAttack`, `attackCooldown`, and `attackRange` values. When spawned, units will look for the closest enemy within range and automatically deal damage every `attackCooldown` seconds. Units are destroyed when their health reaches zero.

Enjoy experimenting and expanding upon the foundation provided here!
