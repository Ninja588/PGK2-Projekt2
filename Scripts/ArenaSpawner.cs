using UnityEngine;
using Alteruna;

public class ArenaSpawner : MonoBehaviour
{
    private Spawner _spawner;
    [SerializeField] private Transform _arena;
    private bool arenaSpawned = false;

    void Awake()
    {
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        _spawner.ForceSync = true;
    }

    void Update() {
        if (Multiplayer.Instance.GetUsers().Count == 1 && Multiplayer.Instance.GetUser().IsHost) {
            _spawner.Spawn(0,new Vector3(-16.59253f, 4.500999f, 0f), Quaternion.identity,new Vector3(0.8f, 5f, 9f)).transform.SetParent(_arena); // Paddle Blue
            _spawner.Spawn(1,new Vector3(55.40747f, 4.500999f, 0f), Quaternion.Euler(0, 180f, 0),new Vector3(0.8f, 5f, 9f)).transform.SetParent(_arena); // Paddle Red
            _spawner.Spawn(2, new Vector3(20f, 3.14f, 0f),Quaternion.identity,new Vector3(2f, 2f, 2f)); // Ball(s)

            arenaSpawned = true;
        }

        if(arenaSpawned) gameObject.SetActive(false);
    }
}
