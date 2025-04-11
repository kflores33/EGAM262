using UnityEngine;

// mostly just stores information about the cursor
public class Cursor : MonoBehaviour
{
    Vector2 _mousePosLastFrame;
    public Vector2 CurrentVelocity { get; set; }

    private void Start()
    {
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosLastFrame = new Vector2(mousePosition3D.x, mousePosition3D.y);

        _mousePosLastFrame = Input.mousePosition; // mouse position in screen coordinates
    }

    private void Update()
    {
        TrackCursorVelocity();
    }

    public void TrackCursorVelocity()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = new Vector2(mousePosition3D.x, mousePosition3D.y);

        CurrentVelocity = (mousePosition - _mousePosLastFrame) / Time.deltaTime;

        _mousePosLastFrame = mousePosition;

        //Debug.Log("Mouse Speed is " + CurrentVelocity.magnitude + " units per second.");
    }
}
