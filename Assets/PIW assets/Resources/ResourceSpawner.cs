using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Spawns resources on the terrain.
/// </summary>
public class ResourceSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _woodPilePrefab;
    [SerializeField] private GameObject _coalPilePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int _woodCount = 50;
    [SerializeField] private int _coalCount = 30;
    [SerializeField] private float _spawnRadius = 200f;
    [SerializeField] private LayerMask _groundLayer;

    /// <summary>
    /// Spawns resources on the terrain.
    /// </summary>
    /// <param name="terrain"></param>
    public async Task SpawnResourcesAsync(Terrain terrain)
    {
        if (terrain == null) return;
        
        // Spawning must happen on main thread for Instantiate, but we can space it out if needed.
        // For now, let's do it in one go after terrain is ready.
        
        SpawnResourceGroup(_woodPilePrefab, _woodCount, terrain);
        SpawnResourceGroup(_coalPilePrefab, _coalCount, terrain);
        
        await Task.Yield(); 
    }

    /// <summary>
    /// Spawns a group of resources on the terrain.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="count"></param>
    /// <param name="terrain"></param>
    private void SpawnResourceGroup(GameObject prefab, int count, Terrain terrain)
    {
        if (prefab == null) return;

        Vector3 terrainPos = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);
            
            float worldX = terrainPos.x + randomX;
            float worldZ = terrainPos.z + randomZ;
            float worldY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrainPos.y;

            Vector3 spawnPos = new Vector3(worldX, worldY, worldZ);
            
            GameObject instance = Instantiate(prefab, spawnPos, Quaternion.Euler(0, Random.Range(0, 360f), 0));
            instance.transform.SetParent(this.transform);
        }
    }
}
