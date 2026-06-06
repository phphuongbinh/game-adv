using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private Transform player;

    [Header("Main Scene Name (Never Unload)")]
    [SerializeField] private string mainSceneName = "MainScene";

    private string currentRoomScene = "";
    private bool isLoading = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadRoom("Forest_1A", "Spawn_Start");
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void LoadRoom(string roomSceneName, string spawnPointName)
    {
        StartCoroutine(LoadRoomCoroutine(roomSceneName, spawnPointName));
    }

    private IEnumerator LoadRoomCoroutine(string newRoom, string spawnPointName)
    {
        isLoading = true;

        // Fast Fade Out
        if (ScreenFader.Instance != null)
        {
            yield return ScreenFader.Instance.FadeOutFast();
        }
        //
        if (newRoom == currentRoomScene)
            yield break;

        // Load scene mới
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(newRoom, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        // Move player tới spawn
        GameObject spawnObj = GameObject.Find(spawnPointName);
        if (spawnObj != null && player != null)
        {
            player.position = spawnObj.transform.position;
        }

        // Unload room cũ (nhưng KHÔNG unload MainScene)
        if (!string.IsNullOrEmpty(currentRoomScene) && currentRoomScene != mainSceneName)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentRoomScene);
            while (!unloadOp.isDone)
                yield return null;
        }

        // Cập nhật currentRoomScene
        currentRoomScene = newRoom;

        if (ScreenFader.Instance != null)
        {
            yield return ScreenFader.Instance.FadeInFast();
        }

        isLoading = true;
    }
}