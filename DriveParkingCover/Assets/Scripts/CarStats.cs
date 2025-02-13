using UnityEngine;

[CreateAssetMenu(fileName = "CarStats", menuName = "Scriptable Objects/CarStats")]
public class CarStats : ScriptableObject
{
    public Sprite goalMat;
    public Sprite carSprite;
    public Color color;
    public string colorString;
}
