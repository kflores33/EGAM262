using Unity.Cinemachine;
using UnityEngine;

public class CameraFindElephant : MonoBehaviour
{
    GameObject _lastSpawnPoint;
    CinemachineCamera cam;

    private void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        if (cam == null)
        {
            Debug.LogError("CinemachineCamera component not found on this GameObject.");
            return;
        }
        var elephantsInScene = FindObjectsByType<LilGuy>(FindObjectsSortMode.None);
        if (elephantsInScene != null && elephantsInScene.Length > 0)
        {
            cam.Follow = elephantsInScene[0].transform;
        }
        else
        {
            _lastSpawnPoint = RespawnPlayer.Instance.LatestCheckpoint;
            cam.Follow = _lastSpawnPoint.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        var lilGuys = FindObjectsByType<LilGuy>(FindObjectsSortMode.None);
        if (lilGuys == null || lilGuys.Length == 0)
        {
            _lastSpawnPoint = RespawnPlayer.Instance.LatestCheckpoint;

            cam.Follow = _lastSpawnPoint.transform;
        }
        else
        {
            cam.Follow = lilGuys[0].transform;
        }
        // if i feel like it, allow for camera to follow multiple elephants (in the event of multiples)
        // or target the elephant closest to the cursor
    }
}
