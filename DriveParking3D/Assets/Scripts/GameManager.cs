using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Vector3 cursorPos;

    public GameObject LineObjPrefab;

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
            Ray ray = new Ray(cursorPos, Vector3.up * 2);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Car car = hit.collider.GetComponent<Car>();
                Debug.Log("car hit");
                if (car != null)
                {
                    // spawn line obj
                    GameObject objToSpawn = LineObjPrefab;
                    objToSpawn.GetComponent<LineDrawer>().gameManager = this.gameObject.GetComponent<GameManager>();
                    objToSpawn.GetComponent<LineDrawer>().carColor = car.stats;

                    Instantiate(objToSpawn, cursorPos, Quaternion.identity);
                }
            }
        }
    }

    // if player clicks and holds m1 at a car, create line renderer object and give it the color of the car

    private void GetMousePos()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        cursorPos.y = 0;
        Debug.DrawRay(cursorPos, Vector3.up * 20, Color.magenta, 2f);

        Vector3 cameraPos = Camera.main.WorldToViewportPoint(cursorPos);
        if (cameraPos.x < 0.0) Debug.Log("Cursor is left of the camera's view");
        if (cameraPos.x > 1.0) Debug.Log("Cursor is right of the camera's view");
        if (cameraPos.y < 0.0) Debug.Log("Cursor is below the camera's view");
        if (cameraPos.y > 1.0) Debug.Log("Cursor is above the camera's view");
        // if any of these are true when drawing a line, delete the line on release of m1
    }
}
