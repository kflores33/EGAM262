using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField]GameObject _checkpointReached;
    [SerializeField] GameObject _winScreen;

    Coroutine _checkpointCoroutine;

    static UIManager _instance;

    public static UIManager Instance => _instance;

    private void Awake()
    {
        // Ensure only one instance of RespawnPlayer exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
    }

    public bool ToggleVisibility(GameObject objectToToggle)
    {
        if(objectToToggle.activeSelf)
        {
            objectToToggle.SetActive(false);
            return false;
        }
        else
        {
            objectToToggle.SetActive(true);
            return true;
        }
    }

    public void EndGame()
    {
        if (_winScreen != null)
        {
            _winScreen.SetActive(true);
        }

        Time.timeScale = 0f; // Pause the game
    }

    public void CheckpointReached()
    {
        if (_checkpointCoroutine != null)
        {
            StopCoroutine(_checkpointCoroutine);
        }
        _checkpointCoroutine = StartCoroutine(ShowCheckpointReached());
    }

    public IEnumerator ShowCheckpointReached()
    {
        _checkpointReached.SetActive(true);
        yield return new WaitForSeconds(2f);
        _checkpointReached.SetActive(false);

        _checkpointCoroutine = null;
    }
}
