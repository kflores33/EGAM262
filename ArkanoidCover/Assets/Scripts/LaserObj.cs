using UnityEngine;

public class LaserObj : MonoBehaviour
{
    Vector2 _velocity = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _velocity = (Quaternion.AngleAxis(0, Vector3.forward) * Vector2.up) * 50;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LaserMovement();
    }
    void LaserMovement()
    {
        Vector2 startPos = this.transform.position;
        Vector2 moveVectorThisFrame = (_velocity * Time.fixedDeltaTime);
        Vector2 endPos = startPos + moveVectorThisFrame;

        RaycastHit2D hit = Physics2D.BoxCast(startPos, new Vector2(0.75f, 2.25f), 0, moveVectorThisFrame, moveVectorThisFrame.magnitude);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<BrickScript>() != null)
            {
                hit.collider.GetComponent<BrickScript>().TakeDamage();
                Destroy(this.gameObject);
            }

            if(hit.collider.GetComponentInParent<Border>() != null)
            {
                Destroy(this.gameObject);
            }
        }

        transform.position = endPos;

    }
}
