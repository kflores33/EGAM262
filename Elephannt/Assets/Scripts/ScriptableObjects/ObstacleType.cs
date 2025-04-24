using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleType", menuName = "Scriptable Objects/ObstacleType")]
public class ObstacleType : ScriptableObject
{
    public enum Type
    {
        Wall,
        Spike,
        SpeedUp,
        Destroyable,
        Checkpoint,
        Goal
    }
    public Type type;
}
