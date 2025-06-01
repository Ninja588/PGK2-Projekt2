using UnityEngine;
using UnityEngine.UI;
using Alteruna;

public class InfoTextHandler : MonoBehaviour
{
    [SerializeField] private Text infoText;

    void Update()
    {
        if (Alteruna.Multiplayer.Instance.GetUsers().Count < 2) infoText.text = "Waiting for players...";
        else infoText.text = "";
    }
}
