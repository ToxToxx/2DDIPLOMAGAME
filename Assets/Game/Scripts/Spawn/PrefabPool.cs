namespace SpawnLogic
{
    // Assets/Game/Scripts/Spawn/PrefabPool.cs
    using System.Collections.Generic;
    using UnityEngine;

    public class PrefabPool
    {
        readonly GameObject _prefab;
        readonly Transform _parent;
        readonly Queue<GameObject> _pool = new Queue<GameObject>();

        public PrefabPool(GameObject prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            for (int i = 0; i < initialSize; i++)
            {
                var go = GameObject.Instantiate(prefab, parent);
                go.SetActive(false);
                _pool.Enqueue(go);
            }
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject go;
            if (_pool.Count > 0)
            {
                go = _pool.Dequeue();
            }
            else
            {
                go = GameObject.Instantiate(_prefab, _parent);
            }
            go.transform.SetParent(_parent, worldPositionStays: true);
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.SetActive(true);
            return go;
        }

        public void Release(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(_parent, worldPositionStays: true);
            _pool.Enqueue(go);
        }
    }

}