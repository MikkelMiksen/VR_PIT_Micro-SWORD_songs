using UnityEngine;
using PIW_assets.player_inventory;

namespace PIW_assets.worldBehaviour
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Monster Settings")]
        [SerializeField] private GameObject _monsterPrefab;

        [SerializeField] private int _maxMonsters = 5;
        [SerializeField] private float _spawnInterval = 30f;
        [SerializeField] private float _spawnDistance = 20f;

        [Header("Night Logic")]
        [SerializeField] private int _startHour = 0;
        [SerializeField] private int _endHour = 6;

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

        private void SpawnMonster()
        {
            if (_monsterPrefab == null) return;

            Transform player = MJ_PlayerController.Instance != null
                ? MJ_PlayerController.Instance.transform
                : Camera.main != null ? Camera.main.transform : null;
            if (player == null) return;

            Vector2 randomCircle = Random.insideUnitCircle.normalized * _spawnDistance;
            Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                spawnPos.y = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;
            }

            GameObject monster = Instantiate(_monsterPrefab, spawnPos, Quaternion.identity);
            _currentMonsterCount++;
        }

        public void DecrementCount()
        {
            _currentMonsterCount = Mathf.Max(0, _currentMonsterCount - 1);
        }
    }
}
