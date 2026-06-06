using UnityEngine;

public class PlayerPersistent : MonoBehaviour
{
    private static PlayerPersistent instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SetPlayer(transform);
        }
    }
}