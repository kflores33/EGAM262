using UnityEngine;

[CreateAssetMenu(fileName = "Disrupt", menuName = "Powerups/Disrupt")]
public class Disrupt : PowerupEffect
{
    public GameObject BallPrefab;
    public override void ApplyEffect()
    {
        BallScript ball = FindFirstObjectByType<BallScript>();

        GameObject ballToInstantiate = BallPrefab;

        if ((FindFirstObjectByType<GameManager>()._slowBallCoroutine != null))
        {
            ballToInstantiate.GetComponent<BallScript>().ChangeSpeed(ball.BallData.SlowSpeed);
        }
        else ballToInstantiate.GetComponent<BallScript>().ChangeSpeed(ball.BallData.BaseSpeed);

        float angle1 = ball.CurrentAngle() + 15f;
        float angle2 = ball.CurrentAngle() - 15f;

        Instantiate(ballToInstantiate, ball.transform.position, Quaternion.Euler(0, 0, angle1));
        Instantiate(ballToInstantiate, ball.transform.position, Quaternion.Euler(0, 0, angle2));
    }
}
