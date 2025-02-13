using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Vector2 cursorPos;
    [HideInInspector] public Vector2 cameraPos;
    [HideInInspector] public bool lineMustDie;

    [Header("References")]
    public GameObject LineObjPrefab;
    public Button startButton;

    public LayerMask layersCar;
    public LayerMask layersGoal;

    [Header("Lists")]
    public List<LineDrawer> spawnedLines;
    public LineDrawer lineDrawer;

    public Car[] carList;
    public Goal[] goalList;
    int carGoalCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.enabled = false;

        // check number of cars & goals in scene (if number is incongruent, return an error)
        carList = FindObjectsByType<Car>(FindObjectsSortMode.None);
        goalList = FindObjectsByType<Goal>(FindObjectsSortMode.None);

        if(carList.Count() != goalList.Count())
        {
            Debug.Assert(false, "Amount of Cars and Goals do not match!");
        }
        else
        {
            carGoalCount = carList.Count();
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

        if (spawnedLines.Count() == carGoalCount) 
        {
            startButton.enabled = true;
        }
    }

    void GetMousePos()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 

        cameraPos = Camera.main.WorldToViewportPoint(cursorPos);
    }

    void SpawnLine()
    {
        // origin of ray
        Vector2 rayPoint = cursorPos;

        // position lineDrawer object should spawn
        Vector2 spawnPoint = rayPoint;

        // create ray
        RaycastHit2D hit = Physics2D.Raycast(rayPoint, Vector2.zero);

        if (hit)
        {
            Car car = hit.collider.GetComponentInParent<Car>();
            if (car != null)
            {
                Debug.Log("car hit");

                // spawn line obj
                if (!ColorAlreadyUsed(spawnedLines, car)) // if color is not already used, spawn a line
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
        // origin of ray
        Vector2 rayPoint = cursorPos;

        // position lineDrawer object should spawn
        Vector2 spawnPoint = rayPoint;

        // create ray
        RaycastHit2D hit = Physics2D.Raycast(rayPoint, Vector2.zero);

        if (hit) 
        { 
            Goal goal = hit.collider.GetComponentInParent<Goal>();

            if (goal != null)
            {
                if (lineMustDie) // if the line has gone out of the bounds of the screen, it won't go through
                {
                    DestroyImmediate(lineDrawer.gameObject);
                    Debug.Log("fuck you the line doesn't work");
                }
                else // line is added to a list and the lineDrawer variable is set to null
                {
                    Debug.Log("reached goal YYAAAYY!!");
                    lineDrawer.canDraw = false;
                    spawnedLines.Add(lineDrawer);
                    lineDrawer = null;
                }
            }
            else // destroy line if it doesnt reach the target
            {
                DestroyImmediate(lineDrawer.gameObject);
                Debug.Log("fuck you the line doesn't work");
            }
        }
    }

    bool ColorAlreadyUsed(List<LineDrawer> lines, Car car)
    {
        string carColor = car.stats.colorString;
        List<string> lineColors = new List<string>();

        // add color string of each line into a new string list
        foreach (LineDrawer line in lines) // creates a list storing the color value assigned to the lines
        {
            lineColors.Add(line.carColor.colorString);
        }

        if (lineColors.Any(line => line.Contains(carColor))) // checks if the list of lines are the same color as the car
        {
            //Debug.Log("color already used!");
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CursorOutOfBounds(Vector2 cursor)
    {
        if (cameraPos.x < 0.0) return true;
        if (cameraPos.x > 1.0) return true;
        if (cameraPos.y < 0.0) return true;
        if (cameraPos.y > 1.0) return true;

        else return false;
    }

    public void StartButton() // make cars move
    {
        foreach (Car car in carList)
        {
            car.currentState = Car.CarStates.Driving;
        }
    }
}
