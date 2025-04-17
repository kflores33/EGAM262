using UnityEngine;

// mostly just stores information about the cursor
public class Cursor : MonoBehaviour
{
    Vector2 _mousePosLastFrame;
    public Vector2 CurrentVelocity { get; set; }

    LayerMask _thisLayer;
    public LayerMask ThisLayer
    {
        get { return _thisLayer; }
        set { _thisLayer = value; }
    }

    private void Start()
    {
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosLastFrame = new Vector2(mousePosition3D.x, mousePosition3D.y);

        _mousePosLastFrame = Input.mousePosition; // mouse position in screen coordinates

        _thisLayer = LayerMask.GetMask("Cursor");
    }

    LilGuy _lastClickedLilGuy;

    private void Update()
    {
        TrackCursorVelocity();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                // Check if the clicked object is the cursor itself
                if (hit.collider.GetComponent<LilGuy>())
                {
                    _lastClickedLilGuy = hit.collider.GetComponent<LilGuy>();
                    _lastClickedLilGuy.OnClickedByPlayer();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_lastClickedLilGuy != null)
            {
                _lastClickedLilGuy.OnReleasedByPlayer();
                _lastClickedLilGuy = null;
            }
        }
    }

    public void TrackCursorVelocity()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition = new Vector2(mousePosition3D.x, mousePosition3D.y);

        ObjToCursorPos(mousePosition);

        CurrentVelocity = (mousePosition - _mousePosLastFrame) / Time.deltaTime;

        _mousePosLastFrame = mousePosition;

        //Debug.Log("Mouse Speed is " + CurrentVelocity.magnitude + " units per second.");
    }
    void ObjToCursorPos(Vector2 mousePos)
    {
        transform.position = mousePos;
    }
}
