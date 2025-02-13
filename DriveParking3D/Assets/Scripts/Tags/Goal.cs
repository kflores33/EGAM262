using UnityEngine;
using UnityEngine.AI;

public class Goal : MonoBehaviour
{
    public CarStats stats;

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        meshRenderer.material = stats.goalMat;
        this.name = $"{stats.colorString}Goal";
    }
}
