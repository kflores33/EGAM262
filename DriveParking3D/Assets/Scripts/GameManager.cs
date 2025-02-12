using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Vector3 cursorPos;
    public Vector3 cameraPos;

    public bool lineMustDie;

    public GameObject LineObjPrefab;

    public LayerMask layersCar;
    public LayerMask layersGoal;

    public List<LineDrawer> spawnedLines;
    public LineDrawer lineDrawer;

    public List<CarAndGoalInScene> winConditionList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // check number of cars & goals in scene (if number is incongruent, return an error)
        List<Car> carsInScene = new List<Car>();
        List<Goal> goalsInScene = new List<Goal>();

        if(carsInScene.Count != goalsInScene.Count)
        {
            Debug.Assert(false, "Amount of Cars and Goals do not match!");
        }
        else
        {
            //foreach (Car car in carsInScene) {
            //    CarAndGoalInScene tempCarAndGoal = 
            //        ;
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePos();

        // if the cursor goes out of bounds, record it
        if (CursorOutOfBounds(cameraPos))
        {
            lineMustDie = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lineMustDie = false;    
            SpawnLine();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (lineDrawer != null)
            {
                HandleLineOnRelease();
            }
        }
    }

    private void GetMousePos()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        cursorPos.y = 0;

        cameraPos = Camera.main.WorldToViewportPoint(cursorPos);
    }

    void SpawnLine()
    {  
        Vector3 rayPoint = cursorPos;
        rayPoint.y = 3;

        Vector3 spawnPoint = rayPoint;
        spawnPoint.y = 0;

        Ray ray = new Ray(rayPoint, Vector3.down * 20);
        Debug.DrawRay(rayPoint, Vector3.down *20, Color.red, 1.5f);
        if (Physics.Raycast(ray, out RaycastHit hit, layersCar))
        {
            Car car = hit.collider.GetComponentInParent<Car>();
            if (car != null)
            {
                Debug.Log("car hit");

                // spawn line obj
                if (!ColorAlreadyUsed(spawnedLines, car))
                {
                    GameObject newLine = Instantiate(LineObjPrefab, spawnPoint, Quaternion.identity);
                    lineDrawer = newLine.GetComponent<LineDrawer>();
                    if (lineDrawer != null)
                    {
                        lineDrawer.gameManager = this.gameObject.GetComponent<GameManager>();
                        lineDrawer.carColor = car.stats;
                    }

                    lineDrawer.name = $"{lineDrawer.carColor.colorString}Line";
                }
                else
                {
                    lineDrawer = null;
                    Debug.Log("color already present---don't spawn line");
                }
            }
            else
            {
                Debug.Log("nothing here");
            }
        }
    }

    void HandleLineOnRelease()
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
                if (lineMustDie)
                {
                    DestroyImmediate(lineDrawer.gameObject);
                    Debug.Log("fuck you the line doesn't work");
                }
                else
                {
                    Debug.Log("reached goal YYAAAYY!!");
                    lineDrawer.canDraw = false;
                    spawnedLines.Add(lineDrawer);
                    lineDrawer = null;
                }
            }
            else
            {
                DestroyImmediate(lineDrawer.gameObject);
                Debug.Log("fuck you the line doesn't work");
            }
        }
    }

    public bool ColorAlreadyUsed(List<LineDrawer> lines, Car car)
    {
        string carColor = car.stats.colorString;
        List<string> lineColors = new List<string>();

        // add color string of each line into a new string list
        foreach (LineDrawer line in lines) 
        {
            lineColors.Add(line.carColor.colorString);
        }

        if (lineColors.Any(line => line.Contains(carColor)))
        {
            //Debug.Log("color already used!");
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CursorOutOfBounds(Vector3 cursor)
    {
        if (cameraPos.x < 0.0) return true;
        if (cameraPos.x > 1.0) return true;
        if (cameraPos.y < 0.0) return true;
        if (cameraPos.y > 1.0) return true;

        else return false;
    }
}

[System.Serializable]
public struct CarAndGoalInScene
{
    public Car car;
    public string carColor;

    public Goal goal;
    public string goalColor;

    public bool hasReachedGoal;
}
