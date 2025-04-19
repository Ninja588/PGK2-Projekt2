using UnityEngine;
using Alteruna;

public class ArenaSpawner : MonoBehaviour
{
    private Spawner _spawner;
    private Transform _arena;
    private bool arenaSpawned = false;

    void Awake()
    {
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        _arena = GameObject.Find("Arena").transform;
    }

    void OnEnable()
    {
        Multiplayer.Instance.OnOtherUserJoined.AddListener(SpawnArena);
        //Debug.Log("jest");
    }

    void OnDisable()
    {
        if (Multiplayer.Instance != null) {
            Multiplayer.Instance.OnOtherUserJoined.RemoveListener(SpawnArena);
            //Debug.Log("nie ma");
        }
    }

    private void SpawnArena(Multiplayer mp, User me)
    {
        //Debug.Log("jestem tu");
        //Debug.Log("ArenaSpawned: " + arenaSpawned + " playerCount: " + mp.GetUsers().Count + " Host: " + me.IsHost);
        if(!arenaSpawned && mp.GetUsers().Count == 2 && me.IsHost) {
            //Debug.Log("Respie!");
            _spawner.Spawn(0,new Vector3(-16.59253f, 4.500999f, 0f), Quaternion.identity,new Vector3(0.8f, 5f, 9f)).transform.SetParent(_arena); // Paddle Blue
            _spawner.Spawn(1,new Vector3(55.40747f, 4.500999f, 0f), Quaternion.Euler(0, 180f, 0),new Vector3(0.8f, 5f, 9f)).transform.SetParent(_arena); // Paddle Red
            _spawner.Spawn(2, new Vector3(20f, 3.14f, 0f),Quaternion.identity,new Vector3(2f, 2f, 2f)); // Ball(s)

            arenaSpawned = true;
        }
    }
    
}
