using UnityEngine;
using UnityEngine.AI;

public class Goal : MonoBehaviour
{
    public CarStats stats;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = stats.goalMat;
        this.name = $"{stats.colorString}Goal";
    }
}
