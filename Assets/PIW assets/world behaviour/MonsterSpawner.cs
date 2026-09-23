using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Settings")]
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private int _maxMonsters = 5;
    [SerializeField] private float _spawnInterval = 30f;
    [SerializeField] private float _spawnDistance = 20f;

    [Header("Night Logic")]
    [SerializeField] private int _startHour = 0; // 12 AM
    [SerializeField] private int _endHour = 6;   // 6 AM

    private float _timer;
    private int _currentMonsterCount;

    private void Update()
    {
        if (DayNightCycle.Instance == null) return;

        int currentHour = DayNightCycle.Instance.currentTime.Hour;
        bool isNight = currentHour >= _startHour && currentHour < _endHour;

        if (isNight && _currentMonsterCount < _maxMonsters)
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval)
            {
                SpawnMonster();
                _timer = 0;
            }
        }
        else
        {
            _timer = 0;
        }
    }

    /// <summary>
    /// Spawn a new monster at a random position around the player.
    /// </summary>
    private void SpawnMonster()
    {
        if (_monsterPrefab == null) return;

        // Try to find a spawn position around the player
        Transform player = MJ_PlayerController.Instance != null ? MJ_PlayerController.Instance.transform : Camera.main.transform;
        if (player == null) return;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * _spawnDistance;
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        
        // Align with terrain
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            spawnPos.y = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;
        }

        GameObject monster = Instantiate(_monsterPrefab, spawnPos, Quaternion.identity);
        _currentMonsterCount++;

        // Ensure monster targets the player rig
        EnemyAI ai = monster.GetComponent<EnemyAI>();
        if (ai != null)
        {
            // The AI will find the player via Camera.main in its Start method
        }
    }

    /// <summary>
    /// Decrement the monster count.
    /// </summary>
    public void DecrementCount()
    {
        _currentMonsterCount = Mathf.Max(0, _currentMonsterCount - 1);
    }
}
