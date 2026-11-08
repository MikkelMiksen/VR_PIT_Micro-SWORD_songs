# What to Do Next: Quest 3 Survival Slice

This is the hands-on Unity Editor checklist for turning the current repository work into a testable slice. The project version is Unity `6000.6.0f1`; runtime UI uses Unity 6.6 `PanelRenderer`, not `UIDocument`.

The repository changes are on branch `copilot/quest3-survival-slice-setup`. Pull that branch before following these steps. The guide does not claim the scene has already been assembled or run; scene references and controller bindings have to be set in Unity and verified there.

## Goal for this first test

In one small test scene, the player should be able to:
1. Start in the existing Meta XR rig.
2. Select a wood pickup and gain wood.
3. Spend three wood to place a campfire.
4. See health, hunger, temperature, and a two-minute timer on the HUD.
5. See warmth help body temperature near the fire.
6. Reach a win when the timer expires, or a loss when health reaches zero.

Do not add procedural world generation, crafting recipes, housing, or an enemy until this basic loop works.

## A. Open the correct project and branch

1. Open **GitHub Desktop**.
2. Choose **File → Open Repository…** and select `VR_PIT_Micro-SWORD_songs` if it is not already open.
3. Click **Current Branch** and choose `copilot/quest3-survival-slice-setup`.
4. Click **Fetch origin**. If **Pull origin** appears, click it.
5. Open **Unity Hub** and open this project with Unity `6000.6.0f1`.
6. Wait for import and script compilation to finish.
7. In Unity, choose **Window → General → Console**. If there are red compile errors, stop and fix them before building the test scene.

## B. Create the test scene without losing the XR rig

1. In Unity's **Project** window, open `Assets`, then `Scenes`.
2. Click `SampleScene.unity` once and press **Ctrl+D** (Windows) or **Cmd+D** (Mac).
3. Select the duplicate and press **F2**. Type `SurvivalSliceTest` and press **Enter**.
4. Double-click `SurvivalSliceTest.unity` to open it.
5. Keep the existing Meta XR rig and EventSystem. Do not add a second XR rig or camera.
6. In the **Hierarchy**, expand the rig. Select its existing rig/origin object and use the Inspector **Transform** fields to place it at an open, flat test area. Keep the existing floor/terrain. Do not guess or replace the rig's locomotion setup.
7. Save with **Ctrl+S** / **Cmd+S**.

## C. Add player systems and the two-minute manager

1. In the **Hierarchy**, click the **+** button (or right-click empty space) and choose **Create Empty**.
2. Name the new object `Game Systems`.
3. Select it, click **Add Component** in the Inspector, type `SurvivalSliceGameManager`, and select that component.
4. Set **Survival Duration Seconds** to `120`.
5. Find the existing player/rig origin GameObject in the Hierarchy. Add these components to the appropriate player object(s), using **Add Component** and each exact type name:
   - `MJ_PlayerInventory`
   - `MJ_PlayerHunger`
   - `MJ_BodyTemp`
   - `PlayerStats`
   - `CampfirePlacement`
6. Search the Hierarchy and make sure only one active instance of each singleton is present: inventory, hunger, body temperature, and player stats. If a component is already present, do not add a duplicate.
7. On `CampfirePlacement`, set **Placement Origin** to the existing head/camera transform if the field is available. Set **Wood Cost** to `3` and **Placement Distance** to `2`.
8. Leave **Campfire Prefab** empty for the first check; the script creates a simple log/light fallback. Later you can assign a fire prefab.
9. In the **Ground Layers** mask, select only the layer used by the floor/terrain. Do not use a mask that includes pickup colliders. If the floor has no dedicated layer yet, create one through **Edit → Project Settings → Tags and Layers**, assign it to the floor, then select that layer in the mask.

## D. Create the HUD PanelRenderer

Use one PanelRenderer per UXML panel. The UI controller expects separate HUD and inventory renderers.

1. In the Hierarchy, click **+ → Create Empty** and name it `Survival HUD`.
2. Select it and choose **Add Component → PanelRenderer**. If the component is not immediately visible, search `PanelRenderer` in the Add Component field.
3. In the PanelRenderer Inspector, assign:
   - **Panel Settings**: use the existing `Assets/PIW assets/crafting ui_document/RunTimePanelSettings.asset` if it is compatible with the HUD display you want. Otherwise create a Panel Settings asset via **Assets → Create → UI Toolkit → Panel Settings Asset** and save it under `Assets/UI` as `SurvivalSlicePanelSettings`.
   - **Visual Tree Asset** (may be labelled **Visual Tree Asset** or **UXML** in the Inspector): `Assets/UI/HUD.uxml`.
4. For a HUD that floats in the VR world, set the Panel Settings render mode to **World Space** and position/scale the `Survival HUD` object so it is readable and comfortably in view. For an initial desktop/Game view check, use a screen-space mode if the PanelRenderer settings expose it; do not assume world-space layout is comfortable until checked in-headset.
5. If Unity offers **GameObject → UI Toolkit → Panel Renderer**, that menu item can create/configure a PanelRenderer object; still verify the Panel Settings and UXML references in the Inspector.

## E. Create the inventory PanelRenderer and hook the UI controller

1. In the Hierarchy, create another empty object named `Survival Inventory`.
2. Add **PanelRenderer** to it.
3. Assign the same Panel Settings asset as the HUD unless you intentionally want different rendering settings.
4. Assign `Assets/UI/Inventory.uxml` as its Visual Tree Asset/UXML.
5. Set its Transform where the player can comfortably see it; it can start near the HUD and be hidden by the controller.
6. Create a separate empty object named `Survival UI Controller`.
7. Add `SurvivalSliceUIController` to it.
8. Drag the `Survival HUD` PanelRenderer from the Hierarchy into **Hud Panel Renderer** in the Inspector.
9. Drag the `Survival Inventory` PanelRenderer into **Inventory Panel Renderer**.
10. Set **Refresh Interval** to `0.15` seconds (or leave the default).
11. Save the scene.

The callback on each PanelRenderer finds the named UXML elements after Unity creates or reloads the visual tree. If a fill/label stays unchanged, verify its UXML `name` exactly matches the controller query: `HealthBarFill`, `HungerBarFill`, `TempBarFill`, `HealthLabel`, `HungerLabel`, `TempLabel`, `TimeLabel`, `ResultLabel`, `InventoryContainer`, `SlotGrid`, and `CloseButton`.

## F. Add visible wood pickups and VR interaction

Make one pickup first, test it, then duplicate it.

1. In the Hierarchy, choose **+ → 3D Object → Cube**. Name it `Wood Pickup 1`.
2. Move it a few meters in front of the player and place it above the floor so it is visible.
3. In its **Box Collider**, make sure **Is Trigger** is off.
4. Click **Add Component**, search `ResourceNode`, and add it.
5. Set **Resource Type** to `Wood`, **Resource Amount** to `3`, and turn on **Should Destroy Resource On Collection**.
6. To use the Meta selection bridge, add `MetaInteractionBridge` and wire the pickup's Meta selection event to `OnInteract`. Use the same event-wrapper pattern already present on the scene's interactive furnace.
7. Use either this Meta selection bridge or the existing `VR_InteractionHandler` raycast path, but not both on one pickup. Double-wiring can collect twice.
8. If using `VR_InteractionHandler`, add it to the relevant controller/hand ray origin. Assign an **Interact Action**, set **Interact Distance** around `3`, and set **Interactable Layer** to include the pickup. The ray direction is the handler Transform's forward direction, so the component must be on the ray origin.
9. Duplicate the pickup twice or three times with **Ctrl+D** / **Cmd+D**, and move each copy to a different nearby position.

The `ResourceNode` code awards resources through `MJ_PlayerInventory`. The current scene action bindings may not include the needed XR binding, so verify the interaction in Play Mode rather than assuming it is wired.

## G. Assign controller actions

The new scripts expose `InputActionProperty` fields. They do nothing until actions are assigned.

1. In the Project window, double-click `Assets/InputSystem_Actions.inputactions` to open the Input Actions editor.
2. Select the `Player` action map. Check the existing `Interact` action and its bindings. For a controller button not already bound, click **+** under its bindings, add a **Binding**, and use the binding picker to select the control path that matches your active Meta XR controller. Do not copy an assumed controller path if the device layout in your rig differs.
3. Save the Input Actions asset (the editor's **Save Asset** button, or **Ctrl+S** / **Cmd+S**).
4. Select the player object with `CampfirePlacement`. Assign a deliberate, distinct controller action to **Place Action**.
5. Select `Survival UI Controller`. Assign a different action to **Toggle Inventory Action**. Avoid using the same button for campfire placement and inventory toggle.
6. If the action picker does not show your action, check that the action exists in the correct map and save the asset. Do not proceed until it appears in the Inspector.
7. If using the `MJ_PlayerController` script rather than the rig's own locomotion, bind **Move Action** and **Turn Action** and assign **Camera Transform**. Do not add or enable a second locomotion driver if the Meta rig already moves the player.

## H. Check player health, hunger, and warmth

1. Confirm the player object has one `PlayerStats`, `MJ_PlayerHunger`, and `MJ_BodyTemp`.
2. Leave their defaults for the first pass, unless your project already has intended tuned values.
3. Ensure `CampfirePlacement` is on the player object/origin where the input action can run and `placementOrigin` points forward from the player's view.
4. The `CampfireHeatSource` raises the local environmental temperature near a placed campfire. Confirm the player's `MJ_BodyTemp` uses the updated branch version so it reads that local warmth.
5. Keep `DayNightCycle` and `Furnace` out of this first scene pass if they are not already correctly wired. The existing day/night script assumes non-null sun/moon lights, curves, a non-empty season array, and `Furnace.Instance`; an incomplete setup can throw errors. Add them only after the simple slice works, or assign all required Inspector references first.

## I. First Play Mode test

1. Save all with **Ctrl+S** / **Cmd+S**.
2. Open **Window → General → Console**. Clear it, then look for new red errors.
3. Start the project's XR simulator/headset using the Meta XR setup already in the scene. Press **Play**.
4. Confirm the existing rig/tracking works and the HUD appears.
5. Select a wood pickup using the configured ray/Meta interaction. Confirm the pickup disappears and inventory count increases.
6. Use the campfire placement button. Confirm three wood are consumed and a fire appears in front of the player on the floor.
7. Stand near the fire and watch the temperature value. Move away and verify warmth fades. If no visible effect occurs, check the Console, `MJ_BodyTemp` placement, and campfire trigger execution before changing the temperature tuning.
8. Confirm the two-minute timer counts down, hunger falls gradually, and the health/hunger/temp bars update.
9. Use the inventory-toggle action and verify the inventory opens, shows resource counts, and closes with the close button or toggle action.
10. Let the timer reach zero and confirm `You survived!` appears.
11. Restart Play Mode. For loss behavior, temporarily set hunger depletion/starvation damage to an easy-to-test value in the Inspector or let the player take damage until health reaches zero; confirm the loss result.
12. Check the Console again. Fix errors before proceeding.

## J. Only after the basic loop passes

Add one existing `MonsterSpawner` and one enemy prefab. Confirm the prefab has its required health/combat and navigation setup, assign it in the Inspector, then verify it can spawn and be defeated or avoided. Do not block the first milestone on NavMesh, procedural terrain, a full sword system, crafting recipes, or building.

## K. Quest 3 build and headset verification

A successful Editor Play Mode check does not prove a Quest build works. After the above loop passes:
1. Open **File → Build Profiles** (or the build window available in this Unity version) and select the Android/Quest profile.
2. Confirm Android is the active platform; install/switch platform if Unity requests it.
3. Check XR Plug-in Management/OpenXR and Meta XR project settings for the Quest target, then build to the device.
4. Test the actual headset for tracking, locomotion comfort, ray interaction, UI visibility, campfire placement, and performance.
5. Profile on device and tune only after confirming the complete loop.

## If something does not work
- **No PanelRenderer in Add Component:** check Unity is actually running `6000.6.0f1`, let scripts finish compiling, and confirm the project is not in an incomplete import state.
- **Panel is blank:** confirm Panel Settings and UXML are assigned, and check Console for UXML/style errors.
- **Controller cannot assign a field:** first verify the action exists and is saved in `InputSystem_Actions.inputactions`; then reopen the Inspector. Do not guess at input-control paths.
- **Pickup does nothing:** check for a collider, `ResourceNode`, exactly one interaction route, the proper layer mask if raycasting, and a valid `MJ_PlayerInventory.Instance`.
- **Campfire will not place:** collect at least three wood, ensure Place Action is assigned/enabled, and check the Console and ground mask.
- **Body temperature crashes:** ensure there is exactly one `MJ_BodyTemp` on the player and verify the branch's campfire-heat implementation compiled.
- **Day/night throws null errors:** disable it for the first loop or assign all sun, moon, curve, season, and furnace references.

## Completion bar
The first survival slice is done when:
- player enters the test scene with the existing XR rig
- HUD renders through PanelRenderer
- wood can be gathered in VR
- wood can be spent to place a warming campfire
- temperature responds to fire proximity
- hunger and timer update live
- inventory UI shows counts
- both timer win and player death loss can be observed
- one simple enemy can be added afterward without destabilizing the basic loop

Keep it small. Validate each checkpoint before adding the next system.
