using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public Transform LayerTransform;
    public Vector2 ParallaxMultiplier = new Vector2(0.8f, 0.8f);
    public bool LockY = false;

    [HideInInspector] public Vector3 InitialPosition;
}
