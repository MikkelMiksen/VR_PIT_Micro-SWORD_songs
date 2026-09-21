using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Unity.AI.Navigation;

public class TerrainGeneratorManager : MonoBehaviour
{
    public static TerrainGeneratorManager Instance { get; private set; }

    [Header("Generation Settings")]
    [SerializeField] private bool _runGenerationOnStart = false;
    [SerializeField] private int _size = 513;
    [SerializeField] private float _roughness = 1.3f;
    [SerializeField] private int _seed = 42;
    [SerializeField] private Vector3 _terrainSize = new Vector3(500, 100, 500);

    [Header("References")]
    [SerializeField] private Terrain _terrain;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private GameObject _loadingScreen;

    [Header("Events")]
    public UnityEvent OnGenerationStarted;
    public UnityEvent OnTerrainReady;
    public UnityEvent OnResourcesReady;
    public UnityEvent OnAllReady;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_terrain == null) _terrain = GetComponent<Terrain>();
    }

    private async void Start()
    {
        if (_runGenerationOnStart)
        {
            await GenerateWorldAsync();
        }
        else
        {
            Debug.Log("[TerrainManager] Generation skipped as per settings.");
            // Still bake NavMesh and spawn resources if requested? 
            // The user said they already ran Diamond Square, so maybe they just mean the terrain is there.
            // For now, let's just do nothing or just the NavMesh if it's missing.
        }
    }

    public async Task GenerateWorldAsync()
    {
        Debug.Log("[TerrainManager] Starting World Generation...");
        
        if (_loadingScreen != null) _loadingScreen.SetActive(true);
        OnGenerationStarted?.Invoke();

        // 1. Generate Heightmap on background thread
        DiamondSquareTerrainGeneration generator = new DiamondSquareTerrainGeneration();
        float[,] heights = await Task.Run(() => generator.Generate(_size, _roughness, _seed));

        // 2. Apply heights on main thread
        ApplyHeights(heights);
        OnTerrainReady?.Invoke();
        Debug.Log("[TerrainManager] Terrain Ready.");

        // 3. Spawn Resources
        ResourceSpawner spawner = FindFirstObjectByType<ResourceSpawner>();
        if (spawner != null)
        {
            await spawner.SpawnResourcesAsync(_terrain);
            OnResourcesReady?.Invoke();
            Debug.Log("[TerrainManager] Resources Ready.");
        }

        // 4. Bake NavMesh
        if (_navMeshSurface != null)
        {
            Debug.Log("[TerrainManager] Baking NavMesh...");
            _navMeshSurface.BuildNavMesh();
            Debug.Log("[TerrainManager] NavMesh Baked.");
        }

        if (_loadingScreen != null) _loadingScreen.SetActive(false);
        OnAllReady?.Invoke();
        Debug.Log("[TerrainManager] World Generation Complete.");
    }

    private void ApplyHeights(float[,] heights)
    {
        if (_terrain == null) return;

        TerrainData data = _terrain.terrainData;
        data.heightmapResolution = _size;
        data.size = _terrainSize;
        data.SetHeights(0, 0, heights);
    }
}
