using UnityEngine;

[CreateAssetMenu(fileName = "SlowBall", menuName = "Powerups/Slow")]
public class SlowBall : PowerupEffect
{
    public override void ApplyEffect()
    {
        //BallScript[] balls = FindObjectsByType<BallScript>(FindObjectsSortMode.None);
        //foreach (BallScript ball in balls)
        //    ball.IsSlowed = true;

        FindFirstObjectByType<GameManager>().SlowBall(30);
    }
}
