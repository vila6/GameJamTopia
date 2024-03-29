﻿using System.Collections;
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
    public GameObject uiPause;

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
            if (Time.timeScale == 0)
                _Continue();
            else Pause();
        }
    }
    public void Pause()
    {
        state = GameState.PAUSED;

        uiPause.SetActive(true);

        Time.timeScale = 0;
    }

    // End pause
    public void _Continue()
    {
        state = GameState.PLAYING;

        uiPause.SetActive(false);

        Time.timeScale = 1;
    }

    public void _ExitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        
        musicAudioSource.Stop();
        musicAudioSource.loop = false;
        musicAudioSource.PlayOneShot(endGame);

        SceneManager.LoadScene(SceneBad);
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
        if(PlayerController.instance.GetInkRatio() < 0.3f)
        {
            SceneManager.LoadScene(SceneBad);
        }
        else if(PlayerController.instance.GetInkRatio() < 0.6f)
        {
            SceneManager.LoadScene(SceneMedium);
        }
        else
        {
            SceneManager.LoadScene(SceneGood);
        }   
    }
}
