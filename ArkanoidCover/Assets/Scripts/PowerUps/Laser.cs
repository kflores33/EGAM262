using UnityEngine;

[CreateAssetMenu(fileName = "Laser", menuName = "Powerups/Laser")]
public class Laser : PowerupEffect
{
    public override void ApplyEffect()
    {
        VausPaddle player = FindFirstObjectByType<VausPaddle>();
        if (player.CurrentState != VausPaddle.VausState.Default)
        {
            player.ResetVausState();
        }

        player.SetVausState(VausPaddle.VausState.Laser);
    }
}
