using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    Vector2 _velocity = Vector2.zero;

    private void Start()
    {
        _velocity = (Quaternion.AngleAxis(0, Vector3.forward) * Vector2.down) * 10;
    }

    private void Update()
    {
        if (transform.position.y < FindFirstObjectByType<FailPointRef>().transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 startPos = this.transform.position;
        Vector2 moveVectorThisFrame = (_velocity * Time.fixedDeltaTime);
        Vector2 endPos = startPos + moveVectorThisFrame;

        RaycastHit2D hit = Physics2D.BoxCast(startPos, new Vector2(4,1.8f), 0, moveVectorThisFrame, moveVectorThisFrame.magnitude);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<VausPaddle>() != null)
            {
                powerupEffect.ApplyEffect();
                FindFirstObjectByType<GameManager>().AddScore(100);
                Destroy(this.gameObject);
            }
        }

        transform.position = endPos;
    }
}
