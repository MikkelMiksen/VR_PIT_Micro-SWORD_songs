# Quest 3 Vertical Slice Guide

## Project goal
Build a small playable Quest 3 survival slice in Unity using lightweight UI Toolkit, a simple survival loop, and minimal combat validation. The goal is not a full survival game—just one working vertical slice that proves the loop works on Meta Quest 3.

## Core constraints
- Target platform: Meta Quest 3.
- Unity editor version: `6000.6.0f1`.
- Use UI Toolkit with Unity 6.6 runtime `PanelRenderer` instead of `UIDocument` for runtime UI.
- Keep gameplay simple, performant, and VR-friendly.
- Prefer repo evidence over assumptions.
- Prefer small, targeted edits over broad rewrites.

## Updated repo reality
The repo already contains the main gameplay systems to build around:
- `MJ_PlayerController` for movement and input handling
- `MJ_PlayerInventory` for resource, tool, and item state
- `MJ_PlayerHunger` for starvation loop
- `MJ_BodyTemp` for temperature logic
- `DayNightCycle` for time and environmental effects
- `PlayerStats` for health/stamina/combat values
- `MonsterSpawner` for night enemy spawning
- `Furnace` and `ResourceData` for heating/fuel handling
- `Assets/UI/HUD.uxml`, `Assets/UI/Inventory.uxml`, and `Assets/UI/UIStyles.uss` for UI Toolkit styling

## New runtime UI migration rules
The project is not using the older Unity 5-style runtime UI pattern. Use the Unity 6.6 runtime API:
- Add `PanelRenderer` to runtime UI objects.
- Assign `PanelSettings` and the proper UXML asset in the Inspector.
- Bind UI references inside `RegisterUIReloadCallback(...)`.
- Never introduce a new `UIDocument` dependency for runtime HUD/inventory setup.

This is now the supported pattern for the project and should be used for:
- HUD panel
- Inventory panel
- Crafting UI
- Any other runtime interface built from UXML/USS

## Current workstream
The active work is to complete the first playable survival slice and then validate it before expanding the building/crafting systems.

### 1) Finish the UI layer
- Use `PanelRenderer` on the HUD and inventory.
- Keep the HUD minimal and readable in VR.
- HUD should include:
  - health bar
  - hunger bar
  - temperature bar
  - time label
- Inventory should expose a visible `SlotGrid` element.
- Add a simple toggle for inventory open/close tied to a controller action.

### 2) Assemble the survival test scene
Create a minimal test scene based on the existing sample scene:
- Keep the existing XR rig and ground/terrain.
- Add player object(s) with the required components and setup.
- Add `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, and `PlayerStats`.
- Add `DayNightCycle`.
- Add the HUD panel and inventory panel as `PanelRenderer` objects.
- Add `SurvivalSliceGameManager` for the two-minute loop.
- Add `CampfirePlacement` and `ResourceNode` collectibles.
- Add a simple enemy spawn hook only after the core loop works.

### 3) Wire gameplay references
- Ensure the player object and campfire system get real references to the stats and hunger/temp systems.
- Ensure the spawner references the actual player transform.
- Ensure resource nodes use the repo’s real `ResourceType` enum values.
- Use existing interaction components or Meta bridge wrappers instead of inventing new control patterns.

### 4) Validate the first playable slice
The first validation loop should be:
1. Gather wood and stone.
2. Place a campfire with a key or controller input.
3. Confirm body temperature changes with proximity to warmth.
4. Confirm hunger drains over time and the HUD updates.
5. Survive the two-minute timer.
6. Confirm win/loss states update correctly.
7. Defeat or avoid enemies only after the survival loop is stable.

### 5) Focus on Quest 3 performance and usability
- Keep UI simple and world-space friendly.
- Avoid expensive UI rebuilds every frame.
- Keep the test scene light enough for Quest performance.
- Confirm interaction patterns are VR-friendly and not keyboard-only.
- Prefer a minimal scene with a clear gameplay loop over a larger sandbox.

## Implementation rules for the next agent
- Read the repo before editing, especially files connected to the gameplay loop.
- Search by symbol first: `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, `PlayerStats`, `ResourceType`, `DayNightCycle`, `UIStyles.uss`.
- Prefer small patches over wide rewrites.
- If a file or type is missing, trace the actual repo definitions before creating new code.
- Keep the project on the same direction as earlier work: minimal, practical, Quest-first, and UI Toolkit-based.

## Runtime UI checklist
- `PanelRenderer` component added
- `PanelSettings` assigned
- UXML asset assigned
- `RegisterUIReloadCallback` used for element lookup
- `PanelRenderer` used instead of `UIDocument` for runtime UI
- HUD updates from gameplay state without repeated rebuilds

## Completion bar
The milestone is complete when:
- the player can enter the scene and see the HUD
- hunger, temperature, and timer values update live
- inventory is visible and updates correctly
- campfire placement works
- enemies spawn in a minimal loop after the core survival test passes
- the scene remains simple and runnable on Quest 3 without heavy overhead

## Final reminder
The goal is not a huge production system. The goal is a small, working Quest 3 vertical slice that proves the core survival + resource + combat loop works in the repo’s current Unity setup.
