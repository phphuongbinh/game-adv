using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Target Room Scene")]
    public string targetRoomScene;

    [Header("Spawn Point Name In Target Room")]
    public string spawnPointName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomManager.Instance.LoadRoom(targetRoomScene, spawnPointName);
        }
    }
}