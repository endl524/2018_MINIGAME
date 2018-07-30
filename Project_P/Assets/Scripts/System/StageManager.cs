using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class StageManager : MonoBehaviour {

    static StageManager m_Instance;
    public Button m_PauseButton;
    bool m_isPause;
    bool m_isGameOver;
    int m_Got_Fish_Item_Num = 0;
    int m_Obstacle_Destroy_Num = 0;
    int m_Small_Enemy_Kill_Num = 0;
    int m_Middle_Enemy_Kill_Num = 0;
    int m_Big_Enemy_Kill_Num = 0;

    void Awake ()
    {
        m_Instance = this;
        m_isPause = true;
        m_isGameOver = false;
    }


    public void Pause_Change() // 호출 시 일시정지 스위칭
    {
        if (m_isPause) m_isPause = false;
        else m_isPause = true;
    }

    public bool Get_isPause() // 현재 상태가 일시정지인지 확인
    {
        return m_isPause;
    }

    public bool Get_isGameOver() // 현재 상태가 일시정지인지 확인
    {
        return m_isGameOver;
    }

    public static StageManager GetInstance() // StageManager 객체 반환
    {
        return m_Instance;
    }

    public void Game_Start()
    {
        Pause_Change();
        m_PauseButton.enabled = true;
        GameObject.Find("Wave").GetComponent<Animator>().SetBool("is_Run", true);
        GameObject.Find("Background_Lane1").GetComponent<Animator>().SetBool("is_Run", true);
        GameObject.Find("Background_Lane2").GetComponent<Animator>().SetBool("is_Run", true);
    }

    public void Game_Over()
    {
        Pause_Change();
        m_PauseButton.enabled = false;
        m_isGameOver = true;
    }



    // ======================================

    public int Get_Fish_Item_Num()
    {
        return m_Got_Fish_Item_Num;
    }

    public int Get_Obstacle_Destroy_Num()
    {
        return m_Obstacle_Destroy_Num;
    }

    public int Get_Small_Enemy_Kill_Num()
    {
        return m_Small_Enemy_Kill_Num;
    }

    public int Get_Middle_Enemy_Kill_Num()
    {
        return m_Middle_Enemy_Kill_Num;
    }

    public int Get_Big_Enemy_Kill_Num()
    {
        return m_Big_Enemy_Kill_Num;
    }

    // ======================================

    public void Plus_Fish_Item_Num()
    {
        ++m_Got_Fish_Item_Num;
    }

    public void Plus_Obstacle_Destroy_Num()
    {
        ++m_Obstacle_Destroy_Num;
    }

    public void Plus_Enemy_Kill_Num(int type)
    {
        switch (type)
        {
            case ENEMY_TYPE.SMALL:
                ++m_Small_Enemy_Kill_Num;
                break;

            case ENEMY_TYPE.MIDDLE:
                ++m_Middle_Enemy_Kill_Num;
                break;

            case ENEMY_TYPE.BIG:
                ++m_Big_Enemy_Kill_Num;
                break;
        }
    }
}