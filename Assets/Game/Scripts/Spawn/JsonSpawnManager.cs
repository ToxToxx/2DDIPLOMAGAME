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
        [SerializeField] private string _jsonFileName = "spawnpoints.json";

        [Header("Addressable Pools")]
        [Tooltip("Задайте ключи Addressables и сколько заранее закешировать")]
        [SerializeField] private List<AddressablePoolEntry> _poolsConfig;

        [System.Serializable]
        public class AddressablePoolEntry
        {
            public string key;
            public int initialSize = 5;
        }

        private Dictionary<string, AddressablePrefabPool> _poolDict;

        private void Awake()
        {
            _poolDict = new Dictionary<string, AddressablePrefabPool>();
            foreach (var e in _poolsConfig)
            {
                if (string.IsNullOrEmpty(e.key)) continue;
                _poolDict[e.key] = new AddressablePrefabPool(e.key, transform, e.initialSize);
            }
        }

        private void Start() => StartCoroutine(LoadAndSpawn());

        private IEnumerator LoadAndSpawn()
        {
            var path = Path.Combine(Application.streamingAssetsPath, _jsonFileName);
            using var req = UnityWebRequest.Get(path);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"SpawnManager: не удалось прочесть {path}: {req.error}");
                yield break;
            }

            var data = JsonUtility.FromJson<SpawnData>(req.downloadHandler.text);
            if (data?.entries == null) yield break;

            foreach (var entry in data.entries)
            {
                var pos = entry.position.ToVector3();
                var rot = Quaternion.Euler(0f, 0f, entry.rotationZ);

                foreach (var key in entry.prefabKeys)
                {
                    if (!_poolDict.TryGetValue(key, out var pool))
                    {
                        Debug.LogWarning($"SpawnManager: нет пула для ключа '{key}'");
                        continue;
                    }
                    pool.Spawn(pos, rot);
                }
            }
        }
#if UNITY_EDITOR
        public void SyncWithJson()
        {
            // Путь к JSON
            string path = Path.Combine(Application.streamingAssetsPath, _jsonFileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"SpawnManager: файл не найден {path}");
                return;
            }

            // Считываем и парсим JSON
            var txt = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SpawnData>(txt);
            if (data?.entries == null) return;

            // Собираем уникальные ключи из JSON
            var keys = new HashSet<string>();
            foreach (var e in data.entries)
            {
                foreach (var k in e.prefabKeys)
                    keys.Add(k);
            }

            // Добавляем в _poolsConfig те ключи, которых ещё нет
            foreach (var k in keys)
            {
                if (!_poolsConfig.Exists(x => x.key == k))
                {
                    _poolsConfig.Add(new AddressablePoolEntry
                    {
                        key = k,
                        initialSize = 5  // или любое значение по умолчанию
                    });
                }
            }

            // Удаляем из _poolsConfig все записи, которых нет в JSON
            _poolsConfig.RemoveAll(x => !keys.Contains(x.key));

            // Пометим сцену/объект как изменённый
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }

}