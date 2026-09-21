
### Implementation Workflow

#### Phase 1: Core Integration & Meta SDK
1. **Meta SDK Setup**: Configure Meta Building Blocks for Quest 3 (Camera Rig, Controllers, Hands).
2. **Interaction Unification**: 
   - Wrap existing `Iinteractable.Interact()` calls into Meta `RayInteractable` and `GrabInteractable` events.
   - Replace `KeyCode.E` logic in `MJ_PlayerController` with Interaction SDK input events.
3. **VR UI Conversion**:
   - Move `InventoryUI` and `CraftingUI` to World Space Canvases.
   - Attach `PointableCanvas` to allow interaction via Ray Interactor.

#### Phase 2: Dynamic World Generation [COMPLETED]
1. **Async Terrain**: [DONE]
   - Optimized `DiamondSquareTerrainGeneration.cs` via `TerrainGeneratorManager.cs`.
   - Generation runs on background thread via `Task.Run`.
   - Added `_runGenerationOnStart` toggle to skip Diamond Square once terrain is ready.
2. **Runtime Resource Spawning**: [DONE]
   - Created `ResourceSpawner.cs` to place resources after terrain is ready.
   - Uses `SampleHeight` for placement.
3. **NavMesh Baking**: [DONE]
   - Integrated `NavMeshSurface.BuildNavMesh()` into the generation sequence.

#### Phase 3: Monster & Combat System [IN PROGRESS]
1. **Combat Adaptation**: [IN PROGRESS]
   - Converted AR Sword data to use Meta Interaction SDK `Grabbable` with physics-based hits.
   - Added `Sword` to `ResourceType` and implemented tool cycling in `MJ_PlayerInventory`.
   - *B* button triggers `CycleTools()` via `MJ_PlayerController`.
2. **Monster Spawning Logic**: [DONE]
   - Created `MonsterSpawner.cs` hooked into `DayNightCycle.cs`.
   - Spawning occurs between 12am and 6am.
   - Monsters target the VR Player Rig and respect the runtime-baked NavMesh.

### Phase 4: Player temperature and hunger [IN PROGRESS]
1. **Player Temperature**: [DONE]
   - Using `MJ_BodyTemp.cs` attached to the VR Player Rig.
   - Integrates with `DayNightCycle` and `Furnace`.
2. **Furnace & Heating**:
   - Heat functionality integrated with `DayNightCycle`.
   - Clothing and Campfire pending.
3. **Player Hunger**: [IN PROGRESS]
   - Created `MJ_PlayerHunger.cs` for depletion and starvation damage.
   - *Y* button triggers `CycleFood()` and basic eating logic.
   - Cooking recipes and campfire cooking pending.

### Phase 5: Building and construction
1. **Building**:
   - Two types should be buildable:
   -  - crafting station types
   -  - housing: Walls, floundations, rooftops, door.
   - a grid based building system should be added.
2.

#### Phase 4: Optimization & Testing
1. **Performance Profiling**: Use the Meta Quest Link Profiler to check CPU/GPU usage.
2. **VRC Verification**: Ensure stable 72/90 FPS and correct hand tracking latency.
