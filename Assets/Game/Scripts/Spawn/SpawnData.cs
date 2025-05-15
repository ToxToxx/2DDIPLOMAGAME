using UnityEngine;

namespace SpawnLogic
{
    [System.Serializable]
    public class SpawnData
    {
        public SpawnEntry[] entries;
    }

    [System.Serializable]
    public class SpawnEntry
    {
        public Vector3Data position;
        public float rotationZ;
        public string[] prefabKeys;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float x, y, z;
        public Vector3 ToVector3() => new Vector3(x, y, z);
    }

}