using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    [SerializeField]ObstacleType _obstacleType;
    public ObstacleType ObstacleType
    {
        get { return _obstacleType; }
    }
}
