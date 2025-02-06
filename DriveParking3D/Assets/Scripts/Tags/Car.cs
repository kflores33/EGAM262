using UnityEngine;

public class Car : MonoBehaviour
{
    public CarStats stats;

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer.material = stats.colorMat;
        this.name = $"{stats.colorString}Car";
    }
}
