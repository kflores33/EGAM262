using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Powerups/Player")]
public class Player : PowerupEffect
{
    public override void ApplyEffect()
    {
        FindFirstObjectByType<GameManager>().AddVaus(1);
    }
}
