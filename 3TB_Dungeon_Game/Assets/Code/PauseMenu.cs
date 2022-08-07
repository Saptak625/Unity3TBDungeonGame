using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject inGameHUD;
    public GameObject pauseMenu;
    public GameObject player;

    public void Pause()
    {
        inGameHUD.SetActive(false);
        pauseMenu.SetActive(true);
        player.GetComponent<PlayerController>().isPaused = true;
        Time.timeScale = 0f;
        gameObject.GetComponent<Button>().enabled = false;
    }

    public void Resume()
    {
        inGameHUD.SetActive(true);
        pauseMenu.SetActive(false);
        player.GetComponent<PlayerController>().isPaused = false;
        Time.timeScale = 1f;
        gameObject.GetComponent<Button>().enabled = true;
    }
    
    public void Quit()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
