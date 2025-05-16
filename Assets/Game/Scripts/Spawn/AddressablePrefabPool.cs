namespace SpawnLogic
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AddressablePrefabPool
    {
        private readonly string _addressKey;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _pool = new Queue<GameObject>();
        private GameObject _prefab;
        private bool _isReady;
        private readonly List<Action> _deferredSpawns = new List<Action>();

        /// <summary>
        /// Создаёт пул, сразу запуская асинхронную загрузку префаба.
        /// </summary>
        public AddressablePrefabPool(string addressKey, Transform parent, int initialSize = 0)
        {
            _addressKey = addressKey;
            _parent = parent;
            LoadAndPrewarm(initialSize);
        }

        /// <summary>
        /// Выдаёт объект из пула (или создаёт новый), устанавливает позицию/ротацию и активирует.
        /// Если префаб ещё не загружен — вызов будет отложен до готовности.
        /// </summary>
        public void Spawn(Vector3 pos, Quaternion rot)
        {
            void DoSpawn()
            {
                GameObject go;
                if (_pool.Count > 0)
                    go = _pool.Dequeue();
                else
                    go = GameObject.Instantiate(_prefab, _parent);

                go.transform.SetParent(_parent, worldPositionStays: true);
                go.transform.SetPositionAndRotation(pos, rot);
                go.SetActive(true);
            }

            if (_isReady)
                DoSpawn();
            else
                _deferredSpawns.Add(DoSpawn);
        }

        /// <summary>
        /// Возвращает объект в пул (деактивирует и ставит в очередь).
        /// </summary>
        public void Release(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(_parent, worldPositionStays: true);
            _pool.Enqueue(go);
        }

        private void LoadAndPrewarm(int initialSize)
        {
            Addressables.LoadAssetAsync<GameObject>(_addressKey)
                .Completed += (AsyncOperationHandle<GameObject> handle) =>
                {
                    if (handle.Status != AsyncOperationStatus.Succeeded)
                    {
                        Debug.LogError($"[Pool] Не удалось загрузить Addressable «{_addressKey}»: {handle.OperationException}");
                        return;
                    }

                    _prefab = handle.Result;
                    _isReady = true;

                    // Предпрогрев пула
                    for (int i = 0; i < initialSize; i++)
                    {
                        var go = GameObject.Instantiate(_prefab, _parent);
                        go.SetActive(false);
                        _pool.Enqueue(go);
                    }

                    // Выполняем все отложенные спавны, если они были вызваны до готовности
                    foreach (var act in _deferredSpawns) act();
                    _deferredSpawns.Clear();
                };
        }
    }
}