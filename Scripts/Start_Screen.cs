using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Screen : MonoBehaviour
{
    public static Start_Screen instance;
    
    [SerializeField] private GameObject startScreenUI;
    
    public static float DifficultyMultiplier { get; private set; } = 1f;
    
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
       
        DifficultyMultiplier = PlayerPrefs.GetFloat("DifficultyMultiplier", 1f);
    }
    
    private void Start()
    {
        ShowStartScreen();
    }
    
    public void ShowStartScreen()
    {
        if (startScreenUI != null)
        {
            startScreenUI.SetActive(true);
        }
        
       
        Time.timeScale = 0;
    }

    public void StartGameNormal()
    {
        Debug.Log("Starting Normal Mode");
        DifficultyMultiplier = 1f;
        PlayerPrefs.SetFloat("DifficultyMultiplier", 1f);
        PlayerPrefs.Save();
        StartGame();
    }

    public void StartGameHard()
    {
        Debug.Log("Starting Hard Mode");
        DifficultyMultiplier = 1.5f; 
        PlayerPrefs.SetFloat("DifficultyMultiplier", 1.5f);
        PlayerPrefs.Save();
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("StartGame called - hiding UI and resuming time");
    
        if (startScreenUI != null)
        {
            startScreenUI.SetActive(false);
            Debug.Log("UI hidden");
        }
    
       
        if (UI.instance != null)
        {
            UI.instance.ResetTimer();
        }
    
        
        Time.timeScale = 1;
        Debug.Log("Time.timeScale set to 1");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}