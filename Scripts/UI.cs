using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private TextMeshProUGUI Seconds;
    [SerializeField] private GameObject gameOverUI;
    
    private float gameStartTime; 

    private void Awake() 
    {
        instance = this;
    }
    
    private void Start()
    {
        gameStartTime = Time.time;
    }

    private void Update()
    {
      
        if (Time.timeScale > 0)
        {
            float elapsedTime = Time.time - gameStartTime;
            Seconds.text = elapsedTime.ToString("F2") + "s";
        }
    }
    
    public void ResetTimer()
    {
        gameStartTime = Time.time;
    }

    public void EnableGameOverUI()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1;
    
    
    if (gameOverUI != null)
    {
        gameOverUI.SetActive(false);
    }
    
   
    int sceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(sceneIndex);
    }
}