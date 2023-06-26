using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    private bool isGameOver;

    private GameObject[] rabbits;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        // Rabbit 태그를 가진 모든 오브젝트를 찾습니다.
       rabbits = GameObject.FindGameObjectsWithTag("Rabbit");

        // 현재 프레임에서 감지한 개수를 저장합니다.
        int currentCount = rabbits.Length;

        if(currentCount <= 0)
        {
            isGameOver = true;
        }

        if (!isGameOver)
        {
            float elapsedTime = Time.time - startTime;

            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = timerString;
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        // 게임 오버 시간 기록
        float gameOverTime = Time.time - startTime;
        PlayerPrefs.SetFloat("GameTime", gameOverTime);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainScene");
    }
}
