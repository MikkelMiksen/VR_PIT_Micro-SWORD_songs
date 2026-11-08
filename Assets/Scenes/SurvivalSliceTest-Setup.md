# Quest 3 Survival Slice Setup

This branch adds small runtime pieces for a two-minute survival test. The project is Unity `6000.6.0f1`; use Unity 6.6's UI Toolkit `PanelRenderer` for runtime UI. Do not add `UIDocument` components. Scene references, XR bindings, and Quest builds still need to be tested in the Unity Editor/headset.

## Test scene to build
Create `Assets/Scenes/SurvivalSliceTest.unity` by duplicating `Assets/Scenes/SampleScene.unity` in Unity. Keep its existing Meta XR comprehensive interaction rig and EventSystem (the sample scene already contains them). Do not add a second camera rig.

In the duplicated scene:

1. Keep the existing XR rig and ground/terrain. Move the rig to a clear start area and orient it toward the test area.
2. Add a `Game Systems` empty object at the scene root. Add `MJ_PlayerInventory`, `MJ_PlayerHunger`, `MJ_BodyTemp`, and `PlayerStats` to the appropriate player object(s), ensuring only one active instance of each singleton. Attach `CampfirePlacement` to the player origin and set `placementOrigin` to the head/camera transform. Set wood cost to 3.
3. Add `SurvivalSliceGameManager` to `Game Systems` and set duration to 120 seconds.
4. Add a `PanelRenderer` for the HUD and another for the inventory. In each renderer's Inspector, assign the project's `PanelSettings` asset and the matching `VisualTreeAsset` (`Assets/UI/HUD.uxml` or `Assets/UI/Inventory.uxml`). Keep them on separate GameObjects unless deliberately nesting PanelRenderers. Add `SurvivalSliceUIController` to a separate `Survival UI` GameObject and assign both renderer fields. Use the same `PanelSettings` if the UI should share rendering configuration. Configure UI input for XR panel interaction using the project/Unity 6.6 input components; keep the HUD non-interactive and make the inventory interactive only if needed.
5. Add 3–5 visible resource props near spawn. Add a collider and `ResourceNode` to each; set type to `Wood`, amount to at least 3, and collection enabled. Add `MetaInteractionBridge` and connect the object's Meta selection event to `OnInteract`, or use the existing `VR_InteractionHandler` with its action, origin, distance, and layer mask assigned. Do not wire both interaction paths to the same object or a selection could collect twice.
6. Assign a ground-only mask to `CampfirePlacement` so the fire does not snap to resource colliders. Assign a campfire prefab if available; otherwise the script creates a lightweight fallback log/light object.
7. Optional, after the basic loop works: add one `MonsterSpawner`, assign an enemy prefab, then validate navigation and combat. The initial test must not depend on procedural terrain or enemy pathfinding.

## PanelRenderer setup details
- Add it through **GameObject → UI Toolkit → Panel Renderer** or **Add Component → PanelRenderer**. Unity's menu may also create/configure `PanelSettings`.
- Assign `PanelSettings` and UXML asset in the `PanelRenderer` Inspector. Use its world-space rendering mode and set a comfortable scale/size for Quest viewing; if screen-space HUD is desired for the first check, use screen-space settings before adding world-space interaction.
- The C# controller registers `RegisterUIReloadCallback` and queries the tree after Unity builds it, so it remains valid after UXML reloads.
- Keep `UIDocument` out of this project setup. Existing legacy UI scripts are being migrated to `PanelRenderer`; don't introduce a new `UIDocument` dependency when wiring this slice.

## Input bindings
Use the project's Input System / Meta XR controller actions. Bind `MJ_PlayerController.moveAction` and `turnAction` only if using that controller, then bind `CampfirePlacement.placeAction` to a deliberate controller button and `SurvivalSliceUIController.toggleInventoryAction` to a separate button. Assign the actual actions in the Inspector and avoid conflicts with XR locomotion. Use either the Meta ray-selection bridge or `VR_InteractionHandler` for resource interaction, not both on the same pickup.

## First test pass
1. Enter Play Mode with a headset or XR simulator and confirm the existing rig/tracking works.
2. Select a wood node. Verify inventory gains the configured amount.
3. Place a campfire after collecting at least 3 wood. Confirm wood decreases and the fire appears on the ground.
4. Stand near the fire and verify body temperature responds; move away and confirm warmth falls off.
5. Confirm health, hunger, temperature, and the 02:00 countdown update; open/close inventory and verify counts.
6. Survive to win, then repeat and allow health to reach zero to verify the loss result.
7. Only then add one enemy and test the combat loop, then profile on Quest 3.

## Known limitation to resolve in Editor
The existing `DayNightCycle` requires assigned sun/moon references, animation curves, a non-empty seasons array, and `Furnace.Instance`. Configure them from project assets or harden the script before using a stripped-down scene. The GitHub files alone cannot validate serialized scene references or guarantee a Quest build.
