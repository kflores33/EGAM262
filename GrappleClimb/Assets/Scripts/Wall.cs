using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum WallType
    {
        Default,
        Grappleable,
        Ice
    }
    public WallType type;
}
