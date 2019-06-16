using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { PLAYING, PAUSED, CINEMATIC }
    GameState state = GameState.CINEMATIC;
    public int score, highScore;
    bool isPaused;

    #region Singleton
    public static GameManager instance = null;

    // Game over
    public GameObject uIGameOver;

    // Victory
    public GameObject uIVictory;
    public string SceneBad, SceneMedium, SceneGood;
    public AudioSource musicAudioSource;
    public AudioClip endGame;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        //Object.DontDestroyOnLoad(this.gameObject); // Si activais esto me rompeis las referencias (al recargar la escena y morir peta)
    }
    #endregion

    private void Update()
    {
        // Detect pause input
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }
    public void Pause()
    {
        Debug.Log("Game paused");
        state = GameState.PAUSED;
        Time.timeScale = 0;
    }

    // End pause
    public void Continue()
    {
        Debug.Log("Game despaused (?)");
        state = GameState.PLAYING;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        
        musicAudioSource.Stop();
        musicAudioSource.loop = false;
        musicAudioSource.PlayOneShot(endGame);


        uIGameOver.SetActive(true);
    }

    public void _ReloadScene()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("_Final");
    }

    public void Victory()
    {
        Time.timeScale = 1f;
        if(PlayerController.instance.GetInkRatio() > 0.3f)
        {
            SceneManager.LoadScene(SceneBad);
        }
        else if(PlayerController.instance.GetInkRatio() > 0.7f)
        {
            SceneManager.LoadScene(SceneMedium);
        }
        else
        {
            SceneManager.LoadScene(SceneGood);
        }   
    }
}
