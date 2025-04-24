using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PlayerMovements playerMovement;   

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        playerMovement.enabled = false;    
        AudioListener.pause = true;       
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        playerMovement.enabled = true;
        AudioListener.pause = false;
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}