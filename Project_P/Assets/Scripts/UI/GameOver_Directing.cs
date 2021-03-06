﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Directing : MonoBehaviour {

    public GameObject m_Directing_Image; // 연출 이미지의 오브젝트 자체
    Animation m_Directing_Images_Animation;

    public GameObject m_Crack_Image; // 깨짐 이미지의 오브젝트 자체
    Animation m_Crack_Images_Animation;

    public GameObject m_UI_Panel; // 점수판 오브젝트
    Animation m_UI_Panels_Animation;

    public GameObject m_Replay_Button; // 재시작 버튼 오브젝트
    Animation m_Replay_Buttons_Animation;


    IEnumerator GameOver_Checker; // 게임오버를 체크
    IEnumerator GameOver_Director; // 게임오버 연출자

    public Texture m_GameOver_Directing_Image2; // 2번째 연출 이미지



    int m_Score_of_Distance = 0; // 이동거리 점수
    int m_Score_of_Small_Enemy_Kill = 0; // 소형 적 처치 점수
    int m_Score_of_Middle_Enemy_Kill = 0; // 중형 적 처치 점수
    int m_Score_of_Boss_Enemy_Kill = 0; // 대형(보스) 적 처치 점수
    int m_Score_of_Obstacle_Destroy = 0; // 장애물 파괴 점수
    int m_Score_of_Item_Getting = 0; // 아이템 획득 점수
    int m_Total_Score = 0; // 총 점수

    public Text m_Distance_Text;
    public Text m_Small_Kill_Text;
    public Text m_Middle_Kill_Text;
    public Text m_Boss_Kill_Text;
    public Text m_Obstacle_Text;
    public Text m_Item_Text;
    public Text m_Total_Score_Text;

    void Start ()
    {
        m_Directing_Images_Animation = m_Directing_Image.GetComponent<Animation>();
        m_Crack_Images_Animation = m_Crack_Image.GetComponent<Animation>();
        m_UI_Panels_Animation = m_UI_Panel.GetComponent<Animation>();
        m_Replay_Buttons_Animation = m_Replay_Button.GetComponent<Animation>();

        GameOver_Checker = GameOver_Check();
        GameOver_Director = GameOver_Direction_Process();

        StartCoroutine(GameOver_Checker);
    }

    IEnumerator GameOver_Check()
    {
        while(true)
        {
            if (StageManager.GetInstance().Get_isGameOver())
            {
                GameOver_Direction_Start();
            }
            yield return null;
        }
    }

    void GameOver_Direction_Start()
    {
        m_Directing_Image.SetActive(true); // 연출 이미지 활성화

        StopCoroutine(GameOver_Checker); // 게임오버 체크 종료

        Set_Up_Score_Data(); // 점수 설정.

        m_Directing_Images_Animation.Play(m_Directing_Images_Animation.GetClip("GameOver_Directing_1").name); // 1번 연출 시작.
        StartCoroutine(GameOver_Director); // 연출 과정 시작.
    }

    IEnumerator GameOver_Direction_Process()
    {
        while (true)
        {
            
            if (m_Directing_Images_Animation["GameOver_Directing_1"].normalizedTime >= 0.95f) // 1번 연출이 끝났다면
            {
                m_Directing_Image.GetComponent<RawImage>().texture = m_GameOver_Directing_Image2; // 이미지 변경.
                m_Crack_Image.SetActive(true); // 깨짐 이미지 활성화.

                m_Directing_Images_Animation.Play(m_Directing_Images_Animation.GetClip("GameOver_Directing_2").name); // 2번 연출 시작.
                m_Crack_Images_Animation.Play(m_Crack_Images_Animation.GetClip("GameOver_Directing_Crack").name); // 깨짐 연출 시작.
            }

            else if (m_Crack_Images_Animation["GameOver_Directing_Crack"].normalizedTime >= 0.95f) // 깨짐 연출이 끝났다면
            {
                m_UI_Panel.SetActive(true); // 점수판 활성화.
                m_UI_Panels_Animation.Play(m_UI_Panels_Animation.GetClip("GameOver_Panel_Up").name); // 점수판 연출 시작.
            }

            else if (m_UI_Panels_Animation["GameOver_Panel_Up"].normalizedTime >= 0.95f) // 점수판 연출이 끝났다면
            {
                m_Replay_Button.SetActive(true); // 재시작 버튼 활성화.
                m_Replay_Buttons_Animation.Play(m_Replay_Buttons_Animation.GetClip("Replay_Button_Fly").name); // 리플레이 버튼 표시 연출 시작.
                m_UI_Panels_Animation.Play(m_UI_Panels_Animation.GetClip("GameOver_Panel_Score").name); // 점수 표시 연출 시작.
            }
            
            else if (m_Replay_Buttons_Animation["Replay_Button_Fly"].normalizedTime >= 0.95f) // 리플레이 버튼 표시 연출이 끝났다면
            {
                Destroy(m_Directing_Image); // 연출 이미지를 지운다.
                Destroy(m_Crack_Image); // 깨짐 이미지를 지운다.

                StopCoroutine(GameOver_Director); // 게임오버 연출을 종료한다.
                StartCoroutine(Replay_Button_Swing()); // 리플레이 버튼 흔들기 시작.
            }

            yield return null;
        }
    }


    IEnumerator Replay_Button_Swing()
    {
        while (true)
        {
            m_Replay_Buttons_Animation.Play(m_Replay_Buttons_Animation.GetClip("Replay_Button_Idle").name); // 리플레이 버튼 흔들기 연출
            yield return null;
        }
    }



    // ===========================


    public void Replay_Button() // 리플레이 버튼을 누를 시 재시작.
    {
        SceneManager.LoadScene(SCENE_NAMES.MAIN_STAGE_SCENE);
    }


    void Set_Up_Score_Data() // 점수 설정 메소드.
    {
        m_Score_of_Distance = System.Convert.ToInt32(Distance_Checker.GetInstance().Get_Distance() * 100); // 1m당 100점
        m_Distance_Text.text = m_Score_of_Distance.ToString() + " 점";

        m_Score_of_Small_Enemy_Kill = StageManager.GetInstance().Get_Small_Enemy_Kill_Num() * 200; // 킬당 200점
        m_Small_Kill_Text.text = "소형 : " + m_Score_of_Small_Enemy_Kill.ToString() + " 점";

        m_Score_of_Middle_Enemy_Kill = StageManager.GetInstance().Get_Middle_Enemy_Kill_Num() * 1500; // 킬당 1500점
        m_Middle_Kill_Text.text = "중형 : " + m_Score_of_Middle_Enemy_Kill.ToString() + " 점";

        m_Score_of_Boss_Enemy_Kill = StageManager.GetInstance().Get_Big_Enemy_Kill_Num() * 10000; // 킬당 10000점
        m_Boss_Kill_Text.text = "보스 : " + m_Score_of_Boss_Enemy_Kill.ToString() + " 점";

        m_Score_of_Obstacle_Destroy = StageManager.GetInstance().Get_Obstacle_Destroy_Num() * 200; // 파괴당 200점
        m_Obstacle_Text.text = m_Score_of_Obstacle_Destroy.ToString() + " 점";

        m_Score_of_Item_Getting = StageManager.GetInstance().Get_Fish_Item_Num() * 300; // 개당 300점
        m_Item_Text.text = m_Score_of_Item_Getting.ToString() + " 점";

        // 총 점수
        m_Total_Score = m_Score_of_Distance + m_Score_of_Small_Enemy_Kill + m_Score_of_Middle_Enemy_Kill + m_Score_of_Boss_Enemy_Kill + m_Score_of_Obstacle_Destroy + m_Score_of_Item_Getting;

        m_Total_Score_Text.text = m_Total_Score.ToString() + " 점";
    }
}
