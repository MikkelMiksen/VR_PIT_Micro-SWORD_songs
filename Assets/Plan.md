The over all plan for this project is to merge three different projects into one.

First project was a survival crafter game.
This project brings interfaces for interacting with different objects, and a way to interact with them.
This can be e.g. picking up a resource from a resource node as well as using those resources to craft by a crafting bench.
there is also a temperature element, this means that the primary game loop in this first project is:
Boot game -> find resources -> craft -> getting cold -> put fuel in furnace -> keep temperature and food and clothing insulation.

along with this project i have brought in a bunch of 2D ui elements.

The second project is an AR experience where some monsteers can break into your room, and you beat them down with swords.
This includes sword data and animations for these monsters.

The idea is to combine those two projects into one where monsters can show up at night. 

Technical requirements for merge:
- Unify Interaction: Rebuild the legacy `Iinteractable` (Keyboard E) system to use Meta Interaction SDK (Hand/Controller Ray and Grab).
- Navigation: Monsters must use NavMesh Surface to bake navigation data at runtime after the terrain heightmap is generated.
- World Mode: Decide between VR (Full immersive terrain) or MR (Passthrough) spawning for monsters.

Then finally i have a Diamond square terrain generation.

from this the, what was before manual resource generation nodes placement should be changed to placed on the terrain after the diamond square has run.

Terrain & Resource logic:
- Terrain generation should be moved to an Asynchronous task or a Coroutine to prevent frame-hitch on Quest 3.
- Resource placement (trees/rocks) must use `Terrain.SampleHeight` to align with the generated Diamond Square surface.

finally some monsters should show up at night between 12am and 6am.