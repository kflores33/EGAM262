using UnityEngine;

public class BrickScript : MonoBehaviour
{
    public BrickTypeData BrickTypeData;
    SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _spriteRenderer.sprite = BrickTypeData.Sprite;
    }
}
