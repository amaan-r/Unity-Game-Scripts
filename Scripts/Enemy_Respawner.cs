using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Respawner : MonoBehaviour
{
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float baseCooldown = 2f;
    [Space]
    [SerializeField] private float cooldownDecreaseRate = 0.05f;
    [SerializeField] private float baseCooldownCap = 0.7f;
    
    private float cooldown;
    private float cooldownCap;
    private float timer;
    private bool hasStarted = false;

    private Transform player;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("=== SCENE LOADED - FORCING RESET ===");
        ResetSpawner();
    }

    private void Awake()
    {
        Debug.Log($"=== AWAKE #{Time.frameCount} === GameObject: {gameObject.name}, InstanceID: {GetInstanceID()}");
        Debug.Log($"Cooldown BEFORE reset: {cooldown}, BaseCooldown: {baseCooldown}");
        
        ResetSpawner();
    }

    private void ResetSpawner()
    {
        player = FindFirstObjectByType<Player>()?.transform;
        
        
        hasStarted = false;
        cooldown = baseCooldown;
        cooldownCap = baseCooldownCap;
        timer = 0;
        
        if (Start_Screen.instance != null)
        {
            cooldown = baseCooldown / Start_Screen.DifficultyMultiplier;
            cooldownCap = baseCooldownCap / Start_Screen.DifficultyMultiplier;
        }
        
        Debug.Log($"RESET COMPLETE - Cooldown: {cooldown}, Timer: {timer}, hasStarted: {hasStarted}, Difficulty: {Start_Screen.DifficultyMultiplier}");
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            hasStarted = false;
            return;
        }
        
        if (!hasStarted)
        {
            timer = cooldown;
            hasStarted = true;
            Debug.Log($"Game started! Timer: {timer}, Cooldown: {cooldown}");
            return;
        }
        
        if (player == null)
        {
            player = FindFirstObjectByType<Player>()?.transform;
            return;
        }

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            timer = cooldown;
            CreateNewEnemy();
            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>()?.transform;
            if (player == null) return;
        }

        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        
        bool createdOnTheRight = newEnemy.transform.position.x > player.position.x;
        if(createdOnTheRight)
        {
             newEnemy.GetComponent<Enemy>().Flip();
        }
    }
}