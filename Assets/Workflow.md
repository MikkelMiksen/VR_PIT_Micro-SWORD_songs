
### Implementation Workflow

#### Phase 1: Core Integration & Meta SDK
1. **Meta SDK Setup**: Configure Meta Building Blocks for Quest 3 (Camera Rig, Controllers, Hands).
2. **Interaction Unification**: 
   - Wrap existing `Iinteractable.Interact()` calls into Meta `RayInteractable` and `GrabInteractable` events.
   - Replace `KeyCode.E` logic in `MJ_PlayerController` with Interaction SDK input events.
3. **VR UI Conversion**:
   - Move `InventoryUI` and `CraftingUI` to World Space Canvases.
   - Attach `PointableCanvas` to allow interaction via Ray Interactor.

#### Phase 2: Dynamic World Generation
1. **Async Terrain**: 
   - Optimize `DiamondSquareTerrainGeneration.cs` to run without blocking the main thread.
   - Trigger generation via a Loading Screen.
2. **Runtime Resource Spawning**:
   - Create a `ResourceSpawner` that iterates through terrain coordinates.
   - Use `SampleHeight` to place `WoodPile` and `CoalPile` prefabs.
3. **NavMesh Baking**: 
   - Attach a `NavMeshSurface` to the Terrain.
   - Call `navMeshSurface.BuildNavMesh()` immediately after terrain and resource generation.

#### Phase 3: Monster & Combat System
1. **Combat Adaptation**: 
   - Convert AR Sword data to use Meta Interaction SDK `Grabbable` with physics-based hits.
2. **Monster Spawning Logic**:
   - Hook into `DayNightCycle.cs` to trigger monster prefab spawning between 12am and 6am.
   - Ensure monsters target the VR Player Rig using the runtime-baked NavMesh.

#### Phase 4: Optimization & Testing
1. **Performance Profiling**: Use the Meta Quest Link Profiler to check CPU/GPU usage.
2. **VRC Verification**: Ensure stable 72/90 FPS and correct hand tracking latency.
