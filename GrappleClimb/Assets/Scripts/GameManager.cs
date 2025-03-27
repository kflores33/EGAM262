using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public UnityEngine.UI.Button submitScoreButton;
    public GameObject scoreboardContainer;

    public RectTransform scoreEntryParent;
    public GameObject scoreEntryPrefab;

    public GameObject endScreen;
    public TMP_Text endScreenText;
    bool _canShowEndScreen;

    public PlayerMovement _playerMovement;
    float _playerHeight = 0;
    float _score = 0;

    bool _gameIsRunning;
    public float _timeLimitMax = 60f;
    public float _maxHeight = 263;
    float _timeRemaining = 0;
    public TMP_Text timeText;

    private void Start()
    {
        //_playerMovement = FindFirstObjectByType<PlayerMovement>();

        Time.timeScale = 1;
        _gameIsRunning = true;
        _timeRemaining = _timeLimitMax;

        UpdateScoreboard();

        submitScoreButton.onClick.AddListener(SubmitScore);

        scoreboardContainer.SetActive(false);
        endScreen.SetActive(false);

        _canShowEndScreen = true;
    }

    private void Update()
    {
        if(_timeRemaining < 0)
        {
            _score = _playerHeight;
            endScreenText.text = "Time's up!";

            _gameIsRunning =false;
            Time.timeScale = 0;
        }
        else if (_playerHeight >= _maxHeight) {
            _score = _playerHeight + (Mathf.FloorToInt(_timeRemaining) * 10);
            endScreenText.text = "Nice job!";

            _gameIsRunning = false;
            Time.timeScale = 0;
        }

        if (_gameIsRunning)
        {
            _timeRemaining -= Time.deltaTime;
            DisplayTime(_timeRemaining);

            if (_playerMovement.gameObject.transform.position.y >= _playerHeight)
            { _playerHeight = _playerMovement.gameObject.transform.position.y; }
        }
        else
        {
            // show player name input, after submitting then show the highscore board
            // show restart button
            if (_canShowEndScreen) endScreen.SetActive(true);
        }
    }

    private void SubmitScore()
    {
        _canShowEndScreen = false;
        endScreen.SetActive(false);

        string playerName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) playerName = "?????";

        NameAndScore updatedScore = new NameAndScore
        {
            Score = Mathf.FloorToInt(_score),
            Name = playerName
        };

        ScoreManager.instance.AcceptNewScore(updatedScore);
        ScoreManager.instance.SaveScores();
        UpdateScoreboard();

        playerNameInput.gameObject.SetActive(false);
        submitScoreButton.gameObject.SetActive(false);
    }
    private void UpdateScoreboard()
    {
        foreach (Transform child in scoreEntryParent)
        {
            Destroy(child.gameObject);
        }

        int scoreCount = ScoreManager.instance.GetScoreCount();

        for (int i = 0; i < scoreCount; i++)
        {
            NameAndScore score = ScoreManager.instance.GetScoreAt(i);
            GameObject entry = Instantiate(scoreEntryPrefab, scoreEntryParent);
            entry.GetComponent<ScoreSlot>().Setup(score, i + 1);
        }

        scoreboardContainer.SetActive(true);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
