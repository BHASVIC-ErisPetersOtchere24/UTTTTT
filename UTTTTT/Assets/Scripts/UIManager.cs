using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject hostScreen;
    [SerializeField] private GameObject joinScreen;
    [SerializeField] private TextMeshProUGUI codeInput;
    [SerializeField] private TextMeshProUGUI codeDisplay;

    public static UIManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public async void HostGame()
    {
        menuScreen.SetActive(false);

        //sets code text to join code
        string joinCode = await RelayManager.Instance.StartHostWithRelay();
        codeDisplay.text = joinCode;
        
        hostScreen.SetActive(true);
    }

    /*
    public async void HostGame()
    {
        menuScreen.SetActive(false);

        joinCode = await RelayManager.Instance.StartHostWithRelay(); //
        hostScreen.SetActive(true);
        codeDisplay.text = joinCode;//

        /*
        if (await LobbyManager.Instance.CreateLobby()) //waits until a lobby is created
        {
            hostScreen.SetActive(true);
        }
        
    }
    */
    public void JoinGame()
    {
        menuScreen.SetActive(false);
        joinScreen.SetActive(true);
    }

    public async void EnterCode()
    {
        string inputtedCode = codeInput.text;
        inputtedCode = inputtedCode.Substring(0, 6); //remove extra character

        if (await RelayManager.Instance.StartClientWithRelay(inputtedCode))
        {
            joinScreen.SetActive(false);
        }
    }
}
