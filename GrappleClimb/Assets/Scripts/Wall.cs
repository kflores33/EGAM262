using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType
    {
        Default,
        Grappleable
    }
    public WallType type;
}
