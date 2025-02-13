using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [Header("References")]
    public CarStats carColor;
    public GameManager gameManager;

    LineRenderer lineRenderer;
    private Vector3 previousPos;

    [Header("Number Stuff")]
    [SerializeField] float minDistance;

    [Header("Stored Data")]
    public List<Vector3> waypoints;

    [Header("Misc")]
    public bool canDraw = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startColor = carColor.color;
        lineRenderer.endColor = carColor.color;

        // need to set the first position to something for it to actually work!
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position); // talking about this

        previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currerntPos = gameManager.cursorPos;
        currerntPos.y = 0.1f;

        if (canDraw)
        {
            if (Vector3.Distance(currerntPos, previousPos) >= minDistance)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currerntPos);

                waypoints.Add(currerntPos);

                previousPos = currerntPos;
            }
        }
    }
}
