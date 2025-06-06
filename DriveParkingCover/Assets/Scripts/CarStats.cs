using UnityEngine;

[CreateAssetMenu(fileName = "CarStats", menuName = "Scriptable Objects/CarStats")]
public class CarStats : ScriptableObject
{
    public Sprite goalMat;
    public Sprite carSprite;
    public Color color;
    public string colorString;

    [Header("Car Specific Variables")]
    public float maxDistance = 0.1f;
    public float carSpeed = 15f;
}
