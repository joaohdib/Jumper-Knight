using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject deathCanvas;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // get the current scene and load it again
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentSceneIndex);
        
    }

    private void OnEnable()
    {
        // subscribe to the player's death event
        Player.OnPlayerDeath += ShowDeathUI;
    }

    private void OnDisable()
    {
        // unsubscribe to avoid memory leaks
        Player.OnPlayerDeath -= ShowDeathUI;
    }

    private void ShowDeathUI()
    {
        // activate the Canvas
        deathCanvas.SetActive(true);
        
        // pause the game
        Time.timeScale = 0f;
    }



}
