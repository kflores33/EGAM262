using UnityEngine;

public class VausPaddle : MonoBehaviour
{
    float _playerSpeed = 38f;
    private Vector2 _velocity = Vector2.zero;

    public LayerMask PlayerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerLayer = LayerMask.GetMask("Vaus");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 startPos = this.transform.position;
        Vector2 moveVectorThisFrame = (_velocity  * Time.fixedDeltaTime);
        Vector2 endPos = startPos + moveVectorThisFrame;

        RaycastHit2D hit = Physics2D.BoxCast(startPos, new Vector2(8.5f, 2), 0, moveVectorThisFrame, moveVectorThisFrame.magnitude, ~PlayerLayer);

        if (hit.collider != null)
        {
            // we only car if we're going "into" this collision
            float velocityComparedToNormal = Vector2.Dot(moveVectorThisFrame.normalized, hit.normal); // if this is negative, we're going into the collision

            if (velocityComparedToNormal < 0.0f) // we're going into the collision
            {
                // try bouncing!
                endPos = hit.centroid;

                _velocity = Vector2.zero;
            }
        }

        if (Input.GetButton("Horizontal"))
        {
            float move = Input.GetAxis("Horizontal");
            _velocity = new Vector2(move, 0) * _playerSpeed;
        }
        else
        {
            _velocity = Vector2.zero;
        }

        transform.position = endPos;
    }
}
