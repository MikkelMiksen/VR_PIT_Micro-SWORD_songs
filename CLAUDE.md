# Claude continuation plan for VR_PIT_Micro-SWORD_songs

## Project goal
Build a small playable Quest 3 vertical slice in Unity using lightweight UI and a simple survival/combat loop.

## Core constraints
- Target platform: Meta Quest 3.
- Unity editor version: `6000.6.0f1`.
- Use UI Toolkit `.uxml` + `.uss` with Unity 6.6 runtime `PanelRenderer`; do not introduce new `UIDocument` components for runtime HUD/inventory work.
- Keep gameplay simple, performant, and VR-friendly.
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
- `Assets/UI/HUD.uxml`, `Assets/UI/Inventory.uxml`, and `Assets/UI/UIStyles.uss` for UI Toolkit work

## Runtime UI migration status
The project has moved to the Unity 6.6 runtime UI pattern:
- `PanelRenderer` is now the correct runtime UI component for this project.
- UI binding is done through `RegisterUIReloadCallback`.
- `UIDocument` remains legacy for this repo and should not be introduced for new runtime UI work.

## Continue in the same style as the previous work
1. Reuse existing naming conventions and namespaces.
2. Validate each change against actual repo files before patching.
3. Prefer small, focused fixes instead of rewriting systems.
4. Keep decisions Quest-friendly and low-overhead.
5. If a file or symbol is missing, search the repo before creating new code.
6. Use `PanelRenderer` and the actual Unity 6.6 API rather than older UI patterns.

## Immediate next objectives
### 1) Finish and wire the UI layer
- Use `PanelRenderer` on the HUD and inventory.
- Keep Class names and root element names aligned with the script selectors.
- Make sure the HUD includes health, hunger, temperature, and time.
- Inventory should expose a `SlotGrid` root element.
- Update any legacy runtime UI code that still depends on `UIDocument`.

### 2) Assemble the minimal Unity scene
Create a minimal gameplay scene with:
- Player rig / player object
- `Rigidbody`, `Collider`, and `MJ_PlayerController`
- `PlayerStats`, `MJ_PlayerHunger`, `MJ_BodyTemp`
- `DayNightCycle`
- `PanelRenderer` for HUD
- `PanelRenderer` for inventory
- `SurvivalSliceGameManager`
- `CampfirePlacement`
- `SimpleEnemySpawner` after the core loop is stable

### 3) Wire gameplay references
- Ensure the player object has required refs for campfire placement, resource pickup, stats access, and hunger/temperature reading.
- Ensure the spawner references the actual player transform.
- Ensure resource nodes use the real `ResourceType` enum values in the repo.
- Make sure interaction patterns follow the existing Meta/VR integration rather than keyboard-only assumptions.

### 4) Validate the first playable slice
Test the following loop:
- Gather wood/stone
- Place campfire with a controller button
- Survive hunger and temperature changes
- Survive two minutes
- Defeat or avoid enemies after the loop is stable
- Verify HUD values update live

### 5) Focus on Quest 3 performance and usability
- Avoid expensive UI rebuilds every frame.
- Keep UI simple and world-space-friendly.
- Prefer small prefabs and minimal scene complexity.
- Check that interaction patterns are VR-friendly and not mouse/keyboard-only.

## Implementation rules for the next agent
- Read the repo before editing, especially the files already connected to the gameplay loop.
- Search by symbol first: `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, `PlayerStats`, `ResourceType`, `DayNightCycle`, `UIStyles.uss`.
- Prefer to patch existing files instead of creating new systems unnecessarily.
- If uncertain about a missing file or enum, trace it in repo history and current tree before creating a replacement.
- Keep the project on the same direction as the earlier work: minimal, practical, Quest-first, and UI Toolkit-based.

## Completion bar for a good milestone
The task is complete when:
- the player can enter the scene and see the HUD
- hunger, temperature, and time values update
- inventory UI is visible and updates
- campfire placement works
- enemy spawns occur in a basic loop only after the core slice is stable
- the scene is simple enough to run on Quest 3 without heavy overhead

## Final reminder
The goal is not a huge production system. The goal is a small, working Quest 3 vertical slice that proves the core survival + resource + combat loop works with the repo’s actual Unity 6.6 runtime UI setup.
