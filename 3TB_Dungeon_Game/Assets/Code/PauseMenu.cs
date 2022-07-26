using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        player.GetComponent<PlayerController>().isPaused = true;
        Time.timeScale = 0f;
        gameObject.GetComponent<Button>().enabled = false;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        player.GetComponent<PlayerController>().isPaused = false;
        Time.timeScale = 1f;
        gameObject.GetComponent<Button>().enabled = true;
    }
    
    public void Quit()
    {
        pauseMenu.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
