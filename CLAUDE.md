# Claude continuation plan for VR_PIT_Micro-SWORD_songs

## Project goal
Build a small playable Quest 3 vertical slice in Unity using modern, lightweight UI and simple survival/combat loop.

## Core constraints
- Target platform: Meta Quest 3.
- Do not rebuild around the old Unity UI stack.
- Prefer UI Toolkit (`.uxml` + `.uss`) and world-space HUD/inventory panels.
- Keep gameplay simple and performant.
- Use repo evidence over assumptions.
- Prefer targeted edits over large rewrites.

## Current repo state
Relevant moving parts already present in the repo:
- `MJ_PlayerController` for movement and input handling
- `MJ_PlayerInventory` for resource and tool state
- `MJ_PlayerHunger` for starvation loop
- `MJ_BodyTemp` for temperature logic
- `DayNightCycle` for time + environmental behavior
- `PlayerStats` for health/stamina/combat data
- `MonsterSpawner` for night enemy spawning
- `Furnace` and `ResourceData` for resource heating and fuel handling
- `UIStyles.uss` and existing UI Toolkit approach already partly implemented

## Continue in the same style as the previous work
1. Reuse existing naming conventions and namespaces.
2. Validate each change against actual repo files before patching.
3. Prefer small, focused fixes instead of rewriting systems.
4. Keep decisions Quest-friendly and low-overhead.
5. If a file or symbol is missing, search the repo before creating new code.
6. If a script depends on a missing type, trace the actual enum/class definition instead of guessing.

## Immediate next objectives
### 1) Finish the UI layer
- Create and validate `Assets/UI/HUD.uxml` and `Assets/UI/Inventory.uxml`.
- Keep Class names and `name` attributes aligned with the script selectors.
- Use `UIDocument` + `GameplayHudController` + `InventoryUiController`.
- Make sure the HUD includes:
  - Health bar
  - Hunger bar
  - Temperature bar
  - Time label
- Inventory should expose a `SlotGrid` root element.

### 2) Assemble a minimal Unity scene
Create a minimal gameplay scene with:
- Player rig / player object
- `Rigidbody`, `Collider`, and `MJ_PlayerController`
- `PlayerStats`, `MJ_PlayerHunger`, `MJ_BodyTemp`
- `DayNightCycle`
- `UIDocument` for HUD
- `UIDocument` for inventory
- `TwoMinuteGameManager`
- `CampfirePlacement`
- `SimpleEnemySpawner`

### 3) Wire gameplay references
- Ensure the player object has required refs for:
  - campfire placement
  - resource pickup
  - stats access
  - hunger/temperature reading
- Ensure the spawner references the actual player transform.
- Ensure resource nodes use the real `ResourceType` enum values in the repo.

### 4) Validate the first playable slice
Test the following loop:
- Gather wood/stone
- Place campfire with a key input
- Survive hunger and temperature changes
- Survive two minutes
- Defeat/avoid enemies
- Verify HUD values update live

### 5) Focus on Quest 3 performance and usability
- Avoid expensive UI rebuilds every frame.
- Keep UI simple and world-space.
- Prefer small prefabs and minimal scene complexity.
- Check that interaction patterns are VR-friendly and not keyboard/mouse-only.

## Implementation rules for the next agent
- Read the repo before editing, especially the files already connected to the gameplay loop.
- Search by symbol first: `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, `PlayerStats`, `ResourceType`, `DayNightCycle`, `UIStyles.uss`.
- Prefer to patch existing files instead of creating new systems unnecessarily.
- If uncertain about a missing file or enum, trace it in repo history and current tree before creating a replacement.
- Keep the project in the same direction as the earlier work: minimal, practical, Quest-first, and UI Toolkit-based.

## Completion bar for a good milestone
The task is complete when:
- the player can enter the scene and see the HUD
- hunger, temperature, and time values update
- inventory UI is visible and updates
- campfire placement works
- enemy spawns occur in a basic loop
- the scene is simple enough to run on Quest 3 without heavy overhead

## Final reminder
The goal is not a huge production system. The goal is a small, working Quest 3 vertical slice that proves the core survival + resource + combat loop works.
