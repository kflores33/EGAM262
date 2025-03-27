using UnityEngine;

[CreateAssetMenu(fileName = "BrickTypeData", menuName = "Scriptable Objects/BrickTypeData")]
public class BrickTypeData : ScriptableObject
{
    public Color Color;
    [Tooltip("Amount of points earned from breaking this brick")]public int PointsWorth;
    [Tooltip("Amount of hits it takes to break this brick")]public int HitsToBreak; 
}
