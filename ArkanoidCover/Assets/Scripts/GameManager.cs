using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int CurrentScore;
    public int CurrentHealth;
    public int BaseHealth = 2;

    public TMP_Text ScoreText;
    public RectTransform VausUI;
    public LayoutGroup HPLayout;

    public GameObject BallPrefab;

    public Coroutine _slowBallCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        CurrentHealth = BaseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindFirstObjectByType<BallScript>() == null)
        {
            RemoveVaus(1);

            FindFirstObjectByType<VausPaddle>().ResetVausState();
            FindFirstObjectByType<VausPaddle>().SetVausState(VausPaddle.VausState.Start);
            FindFirstObjectByType<VausPaddle>().transform.position = new Vector2(0, FindFirstObjectByType<VausPaddle>().transform.position.y);

            foreach(PowerUpPickup powerUp in FindObjectsByType<PowerUpPickup>(FindObjectsSortMode.None))
            {
                Destroy(powerUp.gameObject);
            }

            FindFirstObjectByType<PowerUpSpawner>().ResetPowerUpCounter();

            // instantiate a new ball
            GameObject ball = BallPrefab;
            //ball.GetComponent<BallScript>().ChangeSpeed(0);
            Instantiate(ball, new Vector2(0, -26.9f), Quaternion.identity, FindFirstObjectByType<VausPaddle>().GetComponent<Transform>());
        }
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        ScoreText.text = CurrentScore.ToString();
    }

    public void AddVaus(int vaus)
    {
        CurrentHealth += vaus;

        Instantiate(VausUI, HPLayout.transform);
    }
    public void RemoveVaus(int vaus)
    {
        CurrentHealth -= vaus;
        if (CurrentHealth <= 0)
        {
            // Game Over
            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }

        if (HPLayout.transform.childCount > 0)
        {
            Destroy(HPLayout.transform.GetChild(HPLayout.transform.childCount - 1).gameObject);
        }
    }

    public void SlowBall(float duration)
    {
        if (_slowBallCoroutine != null)
        {
            StopCoroutine(_slowBallCoroutine);
            _slowBallCoroutine = null;
        }
        _slowBallCoroutine = StartCoroutine(SlowBallCoroutine(duration));
    }

    public IEnumerator SlowBallCoroutine(float duration)
    {
        foreach (BallScript ball in FindObjectsByType<BallScript>(FindObjectsSortMode.None))
        {
            ball.ChangeSpeed(ball.BallData.SlowSpeed);
        }
        yield return new WaitForSeconds(duration);
        foreach (BallScript ball in FindObjectsByType<BallScript>(FindObjectsSortMode.None))
        {
            ball.ChangeSpeed(ball.BallData.BaseSpeed);
        }

        StopCoroutine(_slowBallCoroutine);
        _slowBallCoroutine = null;
    }
}
