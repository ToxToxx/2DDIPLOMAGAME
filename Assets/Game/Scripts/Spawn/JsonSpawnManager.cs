namespace SpawnLogic
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    using System.Collections;
    using UnityEngine.Networking;

    public class JsonSpawnManager : MonoBehaviour
    {

        [Header("JSON Settings")]
        [Tooltip("Имя файла внутри StreamingAssets")]
        [SerializeField] private string _jsonFileName = "spawnpoints.json";

        [Header("Prefab Pools")]
        [Tooltip("Настройте ключи и их префабы + начальный размер пула")]
        [SerializeField] private List<PrefabPoolEntry> _pools = new List<PrefabPoolEntry>();

        [System.Serializable]
        public class PrefabPoolEntry
        {
            public string key;             // совпадает с prefabKeys в JSON
            public GameObject prefab;         // сам префаб
            public int initialSize = 5; // сколько закешировать сразу
        }

        private Dictionary<string, PrefabPool> _poolDict;

        private void Awake()
        {
            // Построить пулы
            _poolDict = new Dictionary<string, PrefabPool>();
            foreach (var entry in _pools)
            {
                if (string.IsNullOrEmpty(entry.key) || entry.prefab == null) continue;
                var pool = new PrefabPool(entry.prefab, entry.initialSize, transform);
                _poolDict[entry.key] = pool;
            }
        }

        private void Start()
        {
            StartCoroutine(LoadAndSpawn());
        }

        private IEnumerator LoadAndSpawn()
        {
            // Читаем JSON асинхронно
            string path = Path.Combine(Application.streamingAssetsPath, _jsonFileName);
            using var req = UnityWebRequest.Get(path);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"SpawnManager: ошибка чтения {path}: {req.error}");
                yield break;
            }

            var data = JsonUtility.FromJson<SpawnData>(req.downloadHandler.text);
            if (data?.entries == null || data.entries.Length == 0)
            {
                Debug.LogWarning("SpawnManager: нет записей для спавна");
                yield break;
            }

            // Спавним
            foreach (var entry in data.entries)
            {
                Vector3 pos = entry.position.ToVector3();
                Quaternion rot = Quaternion.Euler(0f, 0f, entry.rotationZ);

                foreach (var key in entry.prefabKeys)
                {
                    if (!_poolDict.TryGetValue(key, out var pool))
                    {
                        Debug.LogWarning($"SpawnManager: пул для ключа '{key}' не найден");
                        continue;
                    }
                    pool.Spawn(pos, rot);
                }
            }
        }

#if UNITY_EDITOR
        // Метод для ручной синхронизации списка _pools с JSON-ключами
        public void SyncWithJson()
        {
            string path = Path.Combine(Application.streamingAssetsPath, _jsonFileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"SpawnManager: файл не найден {path}");
                return;
            }

            var txt = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SpawnData>(txt);
            if (data?.entries == null) return;

            var keys = new HashSet<string>();
            foreach (var e in data.entries)
                foreach (var k in e.prefabKeys)
                    keys.Add(k);

            // Добавить отсутствующие
            foreach (var k in keys)
                if (!_pools.Exists(x => x.key == k))
                    _pools.Add(new PrefabPoolEntry { key = k, initialSize = 5 });

            // Удалить лишние
            _pools.RemoveAll(x => !keys.Contains(x.key));
        }
#endif
    }

}