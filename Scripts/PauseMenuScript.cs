using UnityEngine;
using UnityEngine.SceneManagement;
using Alteruna;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject overlay;
    private GameObject loading;
    private GameObject lobbyBrowser;
    private Alteruna.Avatar avatar;
    private GameObject disconnectButton;
    private PlayerMovement playerMovement;
    //private GameObject panel;

    private bool d = true;

    private void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();

        pauseMenu = GameObject.Find("PauseMenuCanvas");
        overlay = GameObject.Find("OverlayCanvas");
        loading = GameObject.Find("LoadingCanvas");
        disconnectButton = GameObject.Find("DisconnectButton");
        lobbyBrowser = GameObject.Find("LobbyBrowserCanvas");
        //if (GameObject.Find("RoomMenuPanel") != null) panel = GameObject.Find("RoomMenuPanel");
        playerMovement = GetComponent<PlayerMovement>();
        
        overlay.GetComponent<Canvas>().enabled = true;
        pauseMenu.GetComponent<Canvas>().enabled = false;
        loading.GetComponent<Canvas>().enabled = false;
        lobbyBrowser.GetComponent<Canvas>().enabled = false;

        //if(panel != null) panel.SetActive(false);

        disconnectButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Multiplayer.Instance.CurrentRoom.Leave();
            loading.GetComponent<Canvas>().enabled = true;
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void PauseMenu()
    {
        overlay.GetComponent<Canvas>().enabled = !overlay.GetComponent<Canvas>().enabled;
        pauseMenu.GetComponent<Canvas>().enabled = !pauseMenu.GetComponent<Canvas>().enabled;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.enabled = false;

        d = !d;
    }

    void Update()
    {
        if (!avatar.IsMe) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();

            if (d)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerMovement.enabled = true;
            }
        }
    }
}
