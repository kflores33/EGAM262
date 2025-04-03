using UnityEngine;

// reference: https://www.youtube.com/watch?v=PkNRPOrtyls

[CreateAssetMenu(fileName = "PowerupEffect", menuName = "Scriptable Objects/PowerupEffect")]
public abstract class PowerupEffect : ScriptableObject
{
    public abstract void ApplyEffect();
}
