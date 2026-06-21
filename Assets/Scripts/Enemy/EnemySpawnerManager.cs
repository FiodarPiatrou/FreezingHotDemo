using Enums;
using Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : BaseManager
    {
        [Header("Spawn Timing")]
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private int maxAttempts = 10;

        [Header("Spawn Distance From Player")]
        [SerializeField] private float minSpawnRadius = 10f;
        [SerializeField] private float maxSpawnRadius = 20f;

        [Header("Spawn Zone Bounds")]
        [SerializeField] private Vector2 spawnZoneMin = new Vector2(-50f, -50f);
        [SerializeField] private Vector2 spawnZoneMax = new Vector2(50f, 50f);

        [Header("Collision Check")]
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float collisionCheckRadius = 0.5f;

        [Header("Enemy Prefabs")]
        [SerializeField] private GameObject commonEnemyPrefab;
        [SerializeField] private GameObject fireEnemyPrefab;
        [SerializeField] private GameObject iceEnemyPrefab;

        [Header("Spawn Weights")]
        [SerializeField] private float commonWeight = 0.3f;

        [Header("Debug")]
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private Color spawnRadiusColor = new Color(0, 1, 0, 0.2f);
        [SerializeField] private Color spawnZoneColor = new Color(1, 0, 1, 0.3f);

        private Transform _player;
        private Camera _camera;
        private WeatherManager _weatherManager;
        private WeatherState _currentWeather;
        private Vector2 _lastSpawnAttempt;
        private bool _lastAttemptValid;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _weatherManager = ServiceLocator.Instance.Get<WeatherManager>();
            _weatherManager.WeatherStateChanged += OnWeatherChanged;

            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            
            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        }

        private void OnDestroy()
        {
            if (_weatherManager != null)
                _weatherManager.WeatherStateChanged -= OnWeatherChanged;
        }

        private void OnWeatherChanged(WeatherState newWeather)
        {
            _currentWeather = newWeather;
        }

        private void SpawnEnemy()
        {
            if (!_player)
                return;

            if (!TryGetSpawnPosition(out Vector2 spawnPosition))
                return;

            GameObject prefab = SelectEnemyPrefab();
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }

        private bool TryGetSpawnPosition(out Vector2 position)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector2 candidate = GenerateOffScreenPosition();
                _lastSpawnAttempt = candidate;

                // Проверяем, что позиция внутри зоны спавна
                bool inBounds = IsInSpawnZone(candidate);
                bool hasObstacle = Physics2D.OverlapCircle(candidate, collisionCheckRadius, obstacleMask);
                
                _lastAttemptValid = inBounds && !hasObstacle;

                if (_lastAttemptValid)
                {
                    position = candidate;
                    return true;
                }
            }

            position = Vector2.zero;
            return false;
        }

        private Vector2 GenerateOffScreenPosition()
        {
            Vector2 playerPos = _player.position;

            float camHeight = _camera.orthographicSize * 2f;
            float camWidth = camHeight * _camera.aspect;

            // Выбираем случайную сторону (0=верх, 1=право, 2=низ, 3=лево)
            int side = Random.Range(0, 4);

            Vector2 offset = side switch
            {
                0 => new Vector2(Random.Range(-camWidth / 2, camWidth / 2), camHeight / 2 + minSpawnRadius),
                1 => new Vector2(camWidth / 2 + minSpawnRadius, Random.Range(-camHeight / 2, camHeight / 2)),
                2 => new Vector2(Random.Range(-camWidth / 2, camWidth / 2), -camHeight / 2 - minSpawnRadius),
                _ => new Vector2(-camWidth / 2 - minSpawnRadius, Random.Range(-camHeight / 2, camHeight / 2))
            };

            Vector2 candidate = playerPos + offset;

            // Ограничиваем позицию границами зоны спавна
            candidate.x = Mathf.Clamp(candidate.x, spawnZoneMin.x, spawnZoneMax.x);
            candidate.y = Mathf.Clamp(candidate.y, spawnZoneMin.y, spawnZoneMax.y);

            return candidate;
        }

        private bool IsInSpawnZone(Vector2 position)
        {
            return position.x >= spawnZoneMin.x && position.x <= spawnZoneMax.x &&
                   position.y >= spawnZoneMin.y && position.y <= spawnZoneMax.y;
        }

        private GameObject SelectEnemyPrefab()
        {
            float roll = Random.value;

            if (roll < commonWeight)
            {
                return commonEnemyPrefab;
            }

            return _currentWeather switch
            {
                WeatherState.Hot => fireEnemyPrefab,
                WeatherState.Cold => iceEnemyPrefab,
                _ => commonEnemyPrefab
            };
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos)
                return;

            // Рисуем границы зоны спавна
            Gizmos.color = spawnZoneColor;
            Vector3 zoneTopLeft = new Vector3(spawnZoneMin.x, spawnZoneMax.y, 0);
            Vector3 zoneTopRight = new Vector3(spawnZoneMax.x, spawnZoneMax.y, 0);
            Vector3 zoneBottomRight = new Vector3(spawnZoneMax.x, spawnZoneMin.y, 0);
            Vector3 zoneBottomLeft = new Vector3(spawnZoneMin.x, spawnZoneMin.y, 0);

            Gizmos.DrawLine(zoneTopLeft, zoneTopRight);
            Gizmos.DrawLine(zoneTopRight, zoneBottomRight);
            Gizmos.DrawLine(zoneBottomRight, zoneBottomLeft);
            Gizmos.DrawLine(zoneBottomLeft, zoneTopLeft);

            if (!_player)
                return;

            Vector2 playerPos = _player.position;

            // Рисуем минимальный и максимальный радиусы спавна
            Gizmos.color = spawnRadiusColor;
            DrawCircle(playerPos, minSpawnRadius, 64);
            DrawCircle(playerPos, maxSpawnRadius, 64);

            // Рисуем границы камеры
            if (_camera)
            {
                float camHeight = _camera.orthographicSize * 2f;
                float camWidth = camHeight * _camera.aspect;

                Gizmos.color = Color.cyan;
                Vector3 topLeft = playerPos + new Vector2(-camWidth / 2, camHeight / 2);
                Vector3 topRight = playerPos + new Vector2(camWidth / 2, camHeight / 2);
                Vector3 bottomRight = playerPos + new Vector2(camWidth / 2, -camHeight / 2);
                Vector3 bottomLeft = playerPos + new Vector2(-camWidth / 2, -camHeight / 2);

                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
                Gizmos.DrawLine(bottomLeft, topLeft);
            }

            // Рисуем последнюю попытку спавна
            if (_lastSpawnAttempt != Vector2.zero)
            {
                Gizmos.color = _lastAttemptValid ? Color.green : Color.red;
                Gizmos.DrawWireSphere(_lastSpawnAttempt, collisionCheckRadius);
                Gizmos.DrawLine(playerPos, _lastSpawnAttempt);
            }
        }

        private void DrawCircle(Vector2 center, float radius, int segments)
        {
            float angleStep = 360f / segments;
            Vector3 prevPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 newPoint = center + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
        }
    }
}
