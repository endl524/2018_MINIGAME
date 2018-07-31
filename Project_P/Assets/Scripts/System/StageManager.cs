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
    List<Object_Pattern_Structure> m_Enemy_Pattern_List; // 적 패턴 구조 리스트.
    List<Object_Pattern_Structure> m_Obstacle_Pattern_List; // 장애물 패턴 구조 리스트.
    List<Object_Pattern_Structure> m_Item_Pattern_List; // 아이템 패턴 구조 리스트.
    int m_Max_Enemy_Num_in_One_Pattern; // 패턴 하나에 최대 몇마리의 적을 둘것인가? (CSV 파일과 동기화 되어야 한다.)
    int m_Max_Obstacle_Num_in_One_Pattern; // 패턴 하나에 최대 몇개의 장애물을 둘것인가? (CSV 파일과 동기화 되어야 한다.)
    int m_Max_Item_Num_in_One_Pattern; // 패턴 하나에 최대 몇개의 아이템을 둘것인가? (CSV 파일과 동기화 되어야 한다.)

    void Awake ()
    {
        m_Instance = this;
        m_isPause = true;
        m_isGameOver = false;

        m_Max_Enemy_Num_in_One_Pattern = 15; // 일단은 최대 15마리로 설정.
        m_Enemy_Pattern_List = new List<Object_Pattern_Structure>(); // 적 패턴 리스트 할당
        CSV_Manager.GetInstance().Get_Object_Pattern_List(ref m_Enemy_Pattern_List, m_Max_Enemy_Num_in_One_Pattern, OBJECT_TYPE.ENEMY); // "적 패턴 리스트"를 받아온다.

        m_Max_Obstacle_Num_in_One_Pattern = 15; // 일단은 최대 15개로 설정.
        m_Obstacle_Pattern_List = new List<Object_Pattern_Structure>(); // 장애물 패턴 리스트 할당
        CSV_Manager.GetInstance().Get_Object_Pattern_List(ref m_Obstacle_Pattern_List, m_Max_Obstacle_Num_in_One_Pattern, OBJECT_TYPE.OBSTACLE); // "장애물 패턴 리스트"를 받아온다.

        m_Max_Item_Num_in_One_Pattern = 15; // 일단은 최대 15개로 설정.
        m_Item_Pattern_List = new List<Object_Pattern_Structure>(); // 아이템 패턴 리스트 할당
        CSV_Manager.GetInstance().Get_Object_Pattern_List(ref m_Item_Pattern_List, m_Max_Item_Num_in_One_Pattern, OBJECT_TYPE.ITEM); // "아이템 패턴 리스트"를 받아온다.
    }

    public static StageManager GetInstance() // StageManager 객체 반환
    {
        return m_Instance;
    }


    // ======================================


    public List<Object_Pattern_Structure> Get_Enemy_Pattern_List() // 적 출현 패턴 리스트 자체를 반환.
    {
        return m_Enemy_Pattern_List;
    }

    public List<Object_Pattern_Structure> Get_Obstacle_Pattern_List() // 장애물 출현 패턴 리스트 자체를 반환.
    {
        return m_Obstacle_Pattern_List;
    }

    public List<Object_Pattern_Structure> Get_Item_Pattern_List() // 아이템 출현 패턴 리스트 자체를 반환.
    {
        return m_Item_Pattern_List;
    }

    public Object_Pattern_Structure Random_Pick_One_Monster_Pattern() // 적 출현 패턴 중 하나를 뽑아 준다.
    {
        return m_Enemy_Pattern_List[Random.Range(0, m_Enemy_Pattern_List.Count)]; // 랜덤한 인덱스를 선택하여 반환.
    }

    public Object_Pattern_Structure Random_Pick_One_Obstacle_Pattern() // 장애물 출현 패턴 중 하나를 뽑아 준다.
    {
        return m_Obstacle_Pattern_List[Random.Range(0, m_Obstacle_Pattern_List.Count)]; // 랜덤한 인덱스를 선택하여 반환.
    }

    public Object_Pattern_Structure Random_Pick_One_Item_Pattern() // 아이템 출현 패턴 중 하나를 뽑아 준다.
    {
        return m_Item_Pattern_List[Random.Range(0, m_Item_Pattern_List.Count)]; // 랜덤한 인덱스를 선택하여 반환.
    }


    // ======================================


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


    // ======================================


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