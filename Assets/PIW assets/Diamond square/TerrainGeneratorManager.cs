using PIW_assets.Resources;
using UnityEngine;

namespace PIW_assets.worldBehaviour
{
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
            }
        }

        public async Task GenerateWorldAsync()
        {
            Debug.Log("[TerrainManager] Starting World Generation...");

            if (_loadingScreen != null) _loadingScreen.SetActive(true);

            DiamondSquareTerrainGeneration generator = new DiamondSquareTerrainGeneration();
            float[,] heights = await Task.Run(() => generator.Generate(_size, _roughness, _seed));

            ApplyHeights(heights);
            Debug.Log("[TerrainManager] Terrain Ready.");

            ResourceSpawner spawner = FindAnyObjectByType<ResourceSpawner>();
            if (spawner != null)
            {
                await spawner.SpawnResourcesAsync(_terrain);
                Debug.Log("[TerrainManager] Resources Ready.");
            }

            if (_navMeshSurface != null)
            {
                Debug.Log("[TerrainManager] Baking NavMesh...");
                _navMeshSurface.BuildNavMesh();
                Debug.Log("[TerrainManager] NavMesh Baked.");
            }

            if (_loadingScreen != null) _loadingScreen.SetActive(false);
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
}
