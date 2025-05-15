namespace SpawnLogic
{
    using UnityEngine;

    [System.Serializable]
    public class PrefabReference
    {
        [Tooltip("Ключ из JSON → префаб")]
        public string key;
        [Tooltip("Сам префаб")]
        public GameObject prefab;
    }

}
