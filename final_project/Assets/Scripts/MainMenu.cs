using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Text bestTimeText;
    public Button startButton;

    private void Start()
    {
        // 베스트 시간 로드
        float bestTime = PlayerPrefs.GetFloat("BestTime", -1f);
        // 이전 게임의 시간 기록 로드
        float gameTime = PlayerPrefs.GetFloat("GameTime", -1f);

        // 이전 게임의 시간이 베스트 시간보다 클 경우, 베스트 시간 업데이트
        if (gameTime > bestTime)
        {
            bestTime = gameTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        Debug.Log("BestTime : " + bestTime + "GameTime : " + gameTime);

        // 베스트 시간이 기록되었을 경우에만 표시
        int minutes = (int)(bestTime / 60);
        int seconds = (int)(bestTime % 60);
        string bestTimeString = string.Format("베스트 시간: {0:00}:{1:00}", minutes, seconds);
        bestTimeText.text = bestTimeString;

        // 버튼 클릭 이벤트에 메서드 연결
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        // 게임 씬으로 전환
        SceneManager.LoadScene("GameScene");
    }
}
