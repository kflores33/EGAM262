using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    private static RespawnPlayer _instance;
    public static RespawnPlayer Instance => _instance;

    GameObject _latestCheckpoint;
    public GameObject LatestCheckpoint => _latestCheckpoint;

    public GameObject playerPrefab;

    void Awake()
    {
        // Ensure only one instance of RespawnPlayer exists
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        //DontDestroyOnLoad(gameObject); // Optional: Keep the instance across scenes (though it wouldn't be necessary in this case)
    }

    void Update()
    {
        var elephantsInScene = FindObjectsByType<LilGuy>(FindObjectsSortMode.None);
        if (elephantsInScene == null || elephantsInScene.Length == 0)
        {
            // If no elephants are found, respawn the player at the last checkpoint
            Respawn();
        }
    }

    public void SetNewCheckpoint(GameObject checkpoint)
    {
        _latestCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        if (FindObjectsByType<LilGuy>(FindObjectsSortMode.None) != null)
        {
            // Destroy the current player instance
            foreach (var elephant in FindObjectsByType<LilGuy>(FindObjectsSortMode.None))
            {
                Destroy(elephant.gameObject);
            }
        }

        if (_latestCheckpoint == null)
        {
            Instantiate(playerPrefab, new Vector2(0,0), Quaternion.identity);
        }
        else Instantiate(playerPrefab, _latestCheckpoint.transform.position, Quaternion.identity);
    }
}
