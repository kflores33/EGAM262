using UnityEngine;

public class BrickScript : MonoBehaviour
{
    public BrickTypeData BrickTypeData;
    GameManager _gameManager;
    PowerUpSpawner _powerUpSpawner;
    SpriteRenderer _spriteRenderer;

    int _hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _powerUpSpawner = FindFirstObjectByType<PowerUpSpawner>();
        _gameManager = FindFirstObjectByType<GameManager>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _spriteRenderer.sprite = BrickTypeData.Sprite;

        _hp = BrickTypeData.HitsToBreak;
    }

    public void TakeDamage()
    {
        _hp--;
        if (_hp <= 0)
        {
            if (BrickTypeData.HitsToBreak == 1) { _powerUpSpawner.OnBrickDestroyed(this.transform.position); }

            _gameManager.AddScore(BrickTypeData.PointsWorth);
            Destroy(this.gameObject);
        }
    }
}
