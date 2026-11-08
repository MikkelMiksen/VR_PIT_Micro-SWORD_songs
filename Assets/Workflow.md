### Implementation Workflow

#### Phase 1: Core Integration & Meta SDK
1. **Meta SDK Setup**: Configure Meta Building Blocks for Quest 3 (Camera Rig, Controllers, Hands).
2. **Interaction Unification**:
   - Wrap existing `Iinteractable.Interact()` calls into Meta `RayInteractable` and `GrabInteractable` events.
   - Replace keyboard-only interactions with Interaction SDK input events.
3. **Runtime UI Toolkit**:
   - Use Unity 6.6 `PanelRenderer` for runtime UXML/USS UI; do not add `UIDocument` components.
   - Configure PanelRenderer/PanelSettings and ray interaction for world-space UI where needed.

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

#### Phase 4: Player temperature and hunger [IN PROGRESS]
1. **Player Temperature**: [DONE]
   - Using `MJ_BodyTemp.cs` attached to the VR Player Rig.
   - Integrates with `DayNightCycle` and campfire warmth.
2. **Furnace & Heating**: Furnace heating is available; crafting and clothing are outside the first survival test.
3. **Player Hunger**: [IN PROGRESS]
   - `MJ_PlayerHunger.cs` handles hunger depletion and starvation damage.

#### Phase 5: Building and construction
Deferred until after the first playable survival slice: crafting stations, housing pieces, and grid-based building.

#### Optimization & Testing
1. Profile on Quest 3 with Meta Quest Link Profiler.
2. Verify stable frame rate, controller/hand interaction, UI readability, and comfort in-headset.
