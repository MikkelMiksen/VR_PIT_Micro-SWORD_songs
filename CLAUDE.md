# Claude continuation plan for VR_PIT_Micro-SWORD_songs

## Project goal
Build a small playable Quest 3 vertical slice in Unity using lightweight UI and a simple survival/combat loop.

## Core constraints
- Target platform: Meta Quest 3.
- Project editor version: Unity `6000.6.0f1`.
- Use UI Toolkit `.uxml` + `.uss` with Unity 6.6 `PanelRenderer` for runtime UI; do not introduce new `UIDocument` components.
- Keep gameplay simple, performant, and VR-friendly.
- Use repo evidence over assumptions and prefer targeted edits.

## Current repo state
Gameplay systems include `MJ_PlayerController`, `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, `DayNightCycle`, `PlayerStats`, `MonsterSpawner`, `Furnace`, and `ResourceData`. The HUD/inventory UXML assets and `UIStyles.uss` are in `Assets/UI`.

## Continue in the same style
1. Reuse existing naming conventions and namespaces.
2. Validate changes against actual repo files before patching.
3. Prefer small, focused fixes over large rewrites.
4. Keep decisions Quest-friendly and low-overhead.
5. Search for existing types before creating replacements.
6. Use `PanelRenderer` reload callbacks when binding runtime UI, and validate against the Unity 6.6 API.

## Immediate objectives
### 1) Finish and wire UI Toolkit
- Use `PanelRenderer` components with the existing `Assets/UI/HUD.uxml`, `Assets/UI/Inventory.uxml`, and `Assets/UI/UIStyles.uss`.
- Bind the HUD's health, hunger, temperature, and timer fields to gameplay state.
- Expose an inventory `SlotGrid` and update it when inventory changes.
- Migrate remaining runtime UI scripts away from `UIDocument`.

### 2) Assemble the test scene
Build `Assets/Scenes/SurvivalSliceTest.unity` by duplicating `Assets/Scenes/SampleScene.unity`. Keep its existing Meta XR rig; do not add a second rig. Wire player stats/inventory/hunger/body temperature, the HUD and inventory PanelRenderers, game manager, gatherable resources, and campfire placement.

### 3) Validate the loop
Test gathering wood, spending it to place a campfire, warmth changing body temperature, live HUD/inventory updates, and winning/losing the two-minute survival test. Add one enemy only after that loop passes. Keep procedural generation and additional building/crafting systems out of the first validation pass.

### 4) Quest checks
Check controller inputs, ray/grab interactions, UI readability/interaction in-headset, and Quest performance. Verify in Unity; GitHub content alone cannot certify scene references or a built headset app.
