using UnityEngine;
using Alteruna;
using System.Collections;

public class BallEnabler : MonoBehaviour
{
    [SerializeField] private BallScript ballScript;


    void Start()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check() {
        while(true) {
            if (Multiplayer.Instance.GetUsers().Count == 2) {
                yield return new WaitForSeconds(4);
                ballScript.enabled = true;
                this.enabled = false;
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
