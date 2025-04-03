using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> PowerUpTypes;

    Vector2 _lastBrickPos;

    [SerializeField] private int amtOfBricksSinceLastPowerUp = 0;
    [SerializeField] private float _probabilityOfSpawn;

    public int MaxAmtBetweenPowerUps = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBrickDestroyed(Vector2 pos)
    {
        ++amtOfBricksSinceLastPowerUp;

        _lastBrickPos = pos;

        TrySpawnPowerUp(_lastBrickPos);
    }

    public void ResetPowerUpCounter()
    {
        amtOfBricksSinceLastPowerUp = 0;
        _probabilityOfSpawn = 0;
    }

    public bool CanSpawnPowerup()
    {
        _probabilityOfSpawn = Mathf.InverseLerp(0, MaxAmtBetweenPowerUps, amtOfBricksSinceLastPowerUp);
        if (ProbabilityOfSpawn(_probabilityOfSpawn))
        {
            ResetPowerUpCounter();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ProbabilityOfSpawn(float probability)
    {
        float rnd = Random.Range(0f, 1f);
        if(rnd <= probability) return true;
        else return false;
    }

    public void TrySpawnPowerUp(Vector2 position)
    {
        if (CanSpawnPowerup())
        {
            int randomPowerUp = Random.Range(0, PowerUpTypes.Count); 
            GameObject powerUp = Instantiate(PowerUpTypes[randomPowerUp], position, Quaternion.identity);
        }
    }
}
