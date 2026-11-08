# Quest 3 Survival Slice Setup

This branch adds the small runtime pieces for a two-minute survival test. It does not claim that a Quest build was run: Unity scene/prefab references and Input System bindings must be assigned and validated in the Unity Editor.

## Test scene to build
Create `Assets/Scenes/SurvivalSliceTest.unity` by duplicating `Assets/Scenes/SampleScene.unity` in Unity. Keep its existing Meta XR comprehensive interaction rig and EventSystem (the sample scene already contains these). Do not test by starting in an arbitrary asset demo scene.

In the duplicated scene:

1. Keep the existing XR rig and ground/terrain. Move the rig to a clear start area and orient it toward the test area. Avoid adding a second camera rig.
2. Add a `Game Systems` empty object at the scene root. Add `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, and `PlayerStats` to the appropriate player object(s), following existing components and ensuring only one instance of each singleton is active. Attach `CampfirePlacement` to the player origin and set `placementOrigin` to the head/camera transform. Set wood cost to 3.
3. Set up `SurvivalSliceGameManager` on `Game Systems` (duration 120 seconds).
4. Add `UIDocument` + `SurvivalSliceUIController` for the HUD. Assign `Assets/UI/HUD.uxml` as the document source. Add a second `UIDocument` using `Assets/UI/Inventory.uxml` and assign it to the UI controller. Assign a controller Input Action to `toggleInventoryAction`.
5. Add `DayNightCycle` to `Game Systems`; assign the sun and moon lights plus its animation curves and season entries. These are required by the existing script. Ensure the `Furnace` singleton is in the scene, or update its null handling before play, because the current DayNightCycle reads `Furnace.Instance`.
6. Add 3–5 visible resource props near spawn. Add a collider and `ResourceNode` to each; set type to `Wood`, amount to 3 or more, and collection enabled. Add `MetaInteractionBridge` and connect the object's Meta selection event to `OnInteract`, or use the existing `VR_InteractionHandler` with its action, origin, distance, and layer mask assigned. Avoid wiring both interaction paths to the same object, or one selection could collect twice.
7. Place a campfire prefab reference in `CampfirePlacement` if you have one. Leaving it empty uses the script's lightweight fallback log/light object. Assign a ground-only mask to avoid snapping the fire to a resource collider.
8. Optionally add one `MonsterSpawner` only after movement, gathering, warmth, and HUD work. Assign an existing enemy prefab and validate its navigation/combat setup. The first test should not depend on procedural terrain or enemy pathfinding.

## Input bindings
Use the project's Input System / Meta XR controller actions. Bind `MJ_PlayerController.moveAction` and `turnAction`, and its tool/food actions if used. Bind `CampfirePlacement.placeAction` to a deliberate controller button (for example a secondary face button); bind `SurvivalSliceUIController.toggleInventoryAction` separately. Set actions in Inspector, and ensure the rig's locomotion does not conflict with those bindings. The code only responds when an action is assigned.

## First test pass
1. Enter Play Mode with a headset or XR simulator and confirm the existing rig/tracking works.
2. Select a wood node. Verify inventory gains the configured amount.
3. Place a campfire after collecting at least 3 wood. Confirm wood decreases and the fire appears on the ground.
4. Stand near the fire and verify the temperature bar/body temperature responds; move away and confirm warmth falls off.
5. Confirm health, hunger, temperature, and the 02:00 countdown update; open/close inventory and verify counts.
6. Survive to win, then repeat and allow health to reach zero to verify the loss result.
7. Only then add one enemy and test the combat loop, then profile on Quest 3.

## Known limitation to resolve in Editor
The current `DayNightCycle` implementation assumes sun/moon references, animation curves, a non-empty seasons array, and `Furnace.Instance` are present. Configure those from the project assets in the test scene, or harden that existing script before using a stripped-down scene. Verify the scene in Unity after each setup step; the GitHub file changes cannot establish serialized scene references or guarantee a successful Quest build.
