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

    PlayerMovement playerMovement;
    float playerHeight = 0;

    bool gameIsRunning;

    private void Awake()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        gameIsRunning = true;
    }

    private void Start()
    {
        UpdateScoreboard();

        submitScoreButton.onClick.AddListener(SubmitScore);
    }

    private void Update()
    {
        if (gameIsRunning)
        {
            if (playerMovement.gameObject.transform.position.y >= playerHeight)
            { playerHeight = playerMovement.gameObject.transform.position.y; }
        }
    }

    private void SubmitScore()
    {
        string playerName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) playerName = "?????";

        NameAndScore updatedScore = new NameAndScore
        {
            Score = Mathf.FloorToInt(playerHeight),
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
}
