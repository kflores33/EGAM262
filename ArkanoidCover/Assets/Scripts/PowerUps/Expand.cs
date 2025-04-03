using UnityEngine;

[CreateAssetMenu(fileName = "Expand", menuName = "Powerups/Expand")]
public class Expand : PowerupEffect
{
    public override void ApplyEffect()
    {
        VausPaddle player = FindFirstObjectByType<VausPaddle>();
        if (player.CurrentState != VausPaddle.VausState.Default)
        {
            player.ResetVausState();
        }

        player.SetVausState(VausPaddle.VausState.Enlarge);
    }
}
