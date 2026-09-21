using UnityEngine;

[System.Obsolete("Use TerrainGeneratorManager for async generation.")]
public class TerrainCalculation : MonoBehaviour
{
    DiamondSquareTerrainGeneration terrainGen = new DiamondSquareTerrainGeneration();

    [SerializeField] private int size = 513;
    private float roughness = 1.3f;
    public int seed = 42;
    
    void Awake()
    {
        DiamondSquareTerrainGeneration generator = new DiamondSquareTerrainGeneration();

        float[,] heights = generator.Generate(size, roughness, seed);

        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        data.heightmapResolution = size;
        data.size = new Vector3(500, 100, 500);

        data.SetHeights(0, 0, heights);
    }
}
