using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { PLAYING, PAUSED, CINEMATIC }
    GameState state = GameState.CINEMATIC;
    public int score, highScore;
    bool isPaused;

    #region Singleton
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        Object.DontDestroyOnLoad(this.gameObject);
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

    }
}
