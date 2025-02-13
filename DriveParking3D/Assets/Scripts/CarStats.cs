using UnityEngine;

[CreateAssetMenu(fileName = "CarStats", menuName = "Scriptable Objects/CarStats")]
public class CarStats : ScriptableObject
{
    public Material colorMat;
    public Material goalMat;
    public Color color;
    public string colorString;
}
