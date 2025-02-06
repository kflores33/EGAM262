using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Vector3 cursorPos;

    public GameObject LineObjPrefab;

    public LayerMask layersCar;
    public LayerMask layersGoal;

    public List<LineDrawer> spawnedLines;
    public LineDrawer lineDrawer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GetMousePos();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 rayPoint = cursorPos;
            rayPoint.y = 3;

            Vector3 spawnPoint = rayPoint;
            spawnPoint.y = 0;

            Ray ray = new Ray(rayPoint, Vector3.down * 20);
            Debug.DrawRay(rayPoint, Vector3.down *20, Color.red, 1.5f);
            if (Physics.Raycast(ray, out RaycastHit hit, layersCar))
            {
                //Debug.Log("HIT HIT HIT");

                Car car = hit.collider.GetComponentInParent<Car>();
                if (car != null)
                {
                    Debug.Log("car hit");
                    // spawn line obj
                    GameObject newLine = Instantiate(LineObjPrefab, spawnPoint, Quaternion.identity);
                    lineDrawer = newLine.GetComponent<LineDrawer>();
                    if (lineDrawer != null)
                    {
                        lineDrawer.gameManager = this.gameObject.GetComponent<GameManager>();
                        lineDrawer.carColor = car.stats;
                    }

                    lineDrawer.name = $"{lineDrawer.carColor.colorString}Line";
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 rayPoint = cursorPos;
            rayPoint.y = 3;

            Vector3 spawnPoint = rayPoint;
            spawnPoint.y = 0;

            Ray ray = new Ray(rayPoint, Vector3.down * 20);
            Debug.DrawRay(rayPoint, Vector3.down * 20, Color.red, 1.5f);
            if (Physics.Raycast(ray, out RaycastHit hit, layersGoal))
            {
                Goal goal = hit.collider.GetComponentInParent<Goal>();
                if (goal != null)
                {
                    Debug.Log("reached goal YYAAAYY!!");
                    spawnedLines.Add(lineDrawer);
                }

                //////////
            }
            else
            {
                Debug.Log("fuck you the line doesn't work");
                lineDrawer.DestroyThis();
            }

        }
    }

    // if player clicks and holds m1 at a car, create line renderer object and give it the color of the car

    private void GetMousePos()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        cursorPos.y = 0;
        //Debug.DrawRay(cursorPos, Vector3.up * 20, Color.magenta, 2f);

        Vector3 cameraPos = Camera.main.WorldToViewportPoint(cursorPos);
        if (cameraPos.x < 0.0) Debug.Log("Cursor is left of the camera's view");
        if (cameraPos.x > 1.0) Debug.Log("Cursor is right of the camera's view");
        if (cameraPos.y < 0.0) Debug.Log("Cursor is below the camera's view");
        if (cameraPos.y > 1.0) Debug.Log("Cursor is above the camera's view");
        // if any of these are true when drawing a line, delete the line on release of m1
    }
}
