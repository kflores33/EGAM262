using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public CarStats carColor;
    LineRenderer lineRenderer;
    public GameManager gameManager;

    private Vector3 previousPos;
    [SerializeField] float minDistance;

    public List<Vector3> waypoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startColor = carColor.color;
        lineRenderer.endColor = carColor.color;

        previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameManager.cursorPos, previousPos) > minDistance)
        { 
            if (previousPos == transform.position)
            {
                lineRenderer.SetPosition(0, gameManager.cursorPos);
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, gameManager.cursorPos);
                previousPos = gameManager.cursorPos;
            }
        }
    }
}
