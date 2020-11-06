using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static bool IsPlayerReady = false;
    public static bool IsGameOver = false;
    public static bool DisableInput = false;

    public float spawnDelay;
    private float _spawnTimerCounter;
    private float _score;

    [Header("UI")]
    public GameObject[] lostHearts;
    public Text pointsText;
    public GameObject endScreen;
    public Text finalScoreText;

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(ShowIntroMenu());
        yield return StartCoroutine(RunMainGame());
    }

    IEnumerator ShowIntroMenu()
    {
        while (!IsPlayerReady)
        {
            DisableInput = true;
            yield return null;
        }
    }

    IEnumerator RunMainGame()
    {
        MeshSpawner.Instance.SpawnMesh();

        while(!IsGameOver)
        {
            _spawnTimerCounter += Time.deltaTime;
            if(_spawnTimerCounter >= spawnDelay)
            {
                MeshSpawner.Instance.SpawnMesh();
                _spawnTimerCounter = 0;
            }

            yield return null;
        }

        DisableInput = true;
        ShowEndScreen();
    }    

    void ShowEndScreen()
    {
        endScreen.SetActive(true);
        finalScoreText.text = "Total Points\n\n" + _score.ToString();
    }

    public void StartGame()
    {
        IsPlayerReady = true;
        DisableInput = false;
    }

    public void UpdatePoints(int pointsValue)
    {
        pointsText.text = pointsValue.ToString();
        _score = pointsValue;
    }

    public void UpdateHealth(int health)
    {
        if (health < 0) return;

        lostHearts[health].SetActive(true);
        if (health == 0)
        {
            IsGameOver = true;
        }
    }
    
    public void RestartGame()
    {
        IsPlayerReady = false;
        IsGameOver = false;
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);        
    }

    public void ToSampleScene()
    {
        IsPlayerReady = false;
        IsGameOver = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
