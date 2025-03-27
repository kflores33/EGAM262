using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "Scriptable Objects/BallData")]
public class BallData : ScriptableObject
{
    [Header("Speed")]
    public float BaseSpeed;
    public float SlowSpeed;
}
