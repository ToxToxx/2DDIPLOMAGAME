namespace SpawnLogic
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    public class JsonSpawnManager : MonoBehaviour
    {
        [Header("JSON Settings")]
        [Tooltip("Имя файла внутри StreamingAssets")]
        [SerializeField] private string _jsonFileName = "spawnpoints.json";

        [Header("Prefab References")]
        [SerializeField] private List<PrefabReference> _prefabRefs;

        private Dictionary<string, GameObject> _lookup;

        private void Awake()
        {
            // Построить словарь ключ → префаб
            _lookup = new Dictionary<string, GameObject>();
            foreach (var pr in _prefabRefs)
            {
                if (!string.IsNullOrEmpty(pr.key) && pr.prefab != null)
                    _lookup[pr.key] = pr.prefab;
            }
        }

        private void Start()
        {
            LoadAndSpawn();
        }

        private void LoadAndSpawn()
        {
            string path = Path.Combine(Application.streamingAssetsPath, _jsonFileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"JsonSpawnManager: файл не найден: {path}");
                return;
            }

            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SpawnData>(json);
            if (data?.entries == null || data.entries.Length == 0)
            {
                Debug.LogWarning("JsonSpawnManager: нет точек спавна в JSON");
                return;
            }

            foreach (var entry in data.entries)
            {
                Vector3 pos = entry.position.ToVector3();
                Quaternion rot = Quaternion.Euler(0, 0, entry.rotationZ);

                // спавним по каждому ключу в списке
                foreach (var key in entry.prefabKeys)
                {
                    if (!_lookup.TryGetValue(key, out var prefab))
                    {
                        Debug.LogWarning($"JsonSpawnManager: ключ '{key}' не найден в PrefabReferences");
                        continue;
                    }
                    Instantiate(prefab, pos, rot);
                }
            }
        }
    }

}