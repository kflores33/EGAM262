using UnityEngine;

[CreateAssetMenu(fileName = "Catch", menuName = "Powerups/Catch")]
public class Catch : PowerupEffect
{
    public override void ApplyEffect()
    {
        VausPaddle player = FindFirstObjectByType<VausPaddle>();
        if(player.CurrentState != VausPaddle.VausState.Default)
        {
            player.ResetVausState();
        }

        player.SetVausState(VausPaddle.VausState.Catch);
    }
}
