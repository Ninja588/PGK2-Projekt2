using UnityEngine;
using Alteruna;
using System.Collections;

public class ArenaSpawner : MonoBehaviour
{
    private Spawner _spawner;
    private bool arenaSpawned = false;
    [SerializeField] private Alteruna.Avatar avatar;

    void Start()
    {
        if(!avatar.IsMe) return;
        _spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        // _spawner.ForceSync = true;
        if(Multiplayer.Instance.GetUser().IsHost) {
            //Debug.LogWarning("Rozpoczynanie!");
            StartCoroutine(WaitForSpawn());
        }
    }

    private IEnumerator WaitForSpawn() {
        while (true) {
            if (Multiplayer.Instance.GetUsers().Count == 2 && Multiplayer.Instance.GetUser().IsHost) {
                //yield return new WaitForSeconds(3.5f);
                _spawner.Spawn(0,new Vector3(-16.59f, 4.51f, 0f), Quaternion.identity,new Vector3(0.8f, 5f, 9f)).transform.SetParent(null,true); // Paddle Blue
                //yield return new WaitForSeconds(3.5f);
                _spawner.Spawn(1,new Vector3(55.41f, 4.51f, 0f), Quaternion.Euler(0, 0, 0),new Vector3(0.8f, 5f, 9f)).transform.SetParent(null,true); // Paddle Red
                //yield return new WaitForSeconds(3.5f);
                _spawner.Spawn(2, new Vector3(20f, 3.14f, 0f),Quaternion.identity,new Vector3(2f, 2f, 2f)).transform.SetParent(null,true); // Ball(s)
                //_spawner.ForceSync = true;
                arenaSpawned = true;
            }

            if(arenaSpawned) {
                //Debug.LogWarning("Zatrzymano");
                StopCoroutine(WaitForSpawn());
                //Debug.LogError("XDDDDDD");
                //this.enabled = false;
                break;
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
