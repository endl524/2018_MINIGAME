using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map_Movable_Range
{
    public const float Min = -4.2f;
    public const float Max = -1.7f;
}

public static class Lane_Z_Range
{
    public const float Min = 5.5f;
    public const float Max = 8.5f;
}

public class MapScroll : MonoBehaviour {


    IEnumerator Map_Scroll_Coroutine;

    static MapScroll m_Instance;

    float m_Main_Speed = 1.0f;

    // Sky 관련 변수들
    public GameObject m_Sky_1;
    public GameObject m_Sky_2;
    float m_Sky_Speed = 2.0f;
    float m_Sky_Width;


    // Land 관련 변수들
    public GameObject m_Land_1;
    public GameObject m_Land_2;
    float m_Land_Speed = 8.0f;
    float m_Land_Width;


    // Lane 관련 변수들
    public GameObject m_Lane_1;
    public GameObject m_Lane_2;
    float m_Lane_Speed = 18.0f;
    float m_Lane_Width;



    void Awake ()
    {
        m_Instance = this;

        m_Sky_Width = m_Sky_2.transform.position.x;
        m_Land_Width = m_Land_2.transform.position.x;
        m_Lane_Width = m_Lane_2.transform.position.x;

        Map_Scroll_Coroutine = Map_Scroll();

        StartCoroutine(Map_Scroll_Coroutine);
    }

    void OnDestroy()
    {
        StopCoroutine(Map_Scroll_Coroutine);
    }


    public static MapScroll GetInstance()
    {
        return m_Instance;
    }






    // ==========================================================
    

    IEnumerator Map_Scroll() // Map 관련 스크롤 코루틴
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                Sky_Pos_Setting(ref m_Sky_1, m_Sky_2.transform.position.x);
                Sky_Pos_Setting(ref m_Sky_2, m_Sky_1.transform.position.x);

                Land_Pos_Setting(ref m_Land_1, m_Land_2.transform.position.x);
                Land_Pos_Setting(ref m_Land_2, m_Land_1.transform.position.x);

                Lane_Pos_Setting(ref m_Lane_1, m_Lane_2.transform.position.x);
                Lane_Pos_Setting(ref m_Lane_2, m_Lane_1.transform.position.x);
            }
            yield return null;
        }
    }


    // ==========================================================


    void Sky_Pos_Setting(ref GameObject sky, float pos_x) // Sky 오브젝트 위치 변경
    {
        if (sky.transform.position.x <= -m_Sky_Width)
        {
            Vector3 pos;
            pos.x = pos_x + m_Sky_Width;
            pos.y = sky.transform.position.y;
            pos.z = sky.transform.position.z;
            sky.transform.position = pos;
        }
        sky.transform.Translate(-m_Sky_Speed * m_Main_Speed * Time.deltaTime, 0.0f, 0.0f);
    }


    void Land_Pos_Setting(ref GameObject land, float pos_x) // Land 오브젝트 위치 변경
    {
        if (land.transform.position.x <= -m_Land_Width)
        {
            Vector3 pos;
            pos.x = pos_x + m_Land_Width;
            pos.y = land.transform.position.y;
            pos.z = land.transform.position.z;
            land.transform.position = pos;
        }
        land.transform.Translate(-m_Land_Speed * m_Main_Speed * Time.deltaTime, 0.0f, 0.0f);
    }


    void Lane_Pos_Setting(ref GameObject lane, float pos_x) // Lane 오브젝트 위치 변경
    {
        if (lane.transform.position.x <= -m_Lane_Width)
        {
            Vector3 pos;
            pos.x = pos_x + m_Lane_Width;
            pos.y = lane.transform.position.y;
            pos.z = lane.transform.position.z;
            lane.transform.position = pos;
        }
        
        lane.transform.Translate(-m_Lane_Speed * m_Main_Speed * Time.deltaTime, 0.0f, 0.0f);
    }


    // ==========================================================
    
    public float Get_Lane_Speed()
    {
        return m_Lane_Speed;
    }

    public float Get_Main_Speed()
    {
        return m_Main_Speed;
    }

    public void Change_Speed(float main_speed_var) // 몇 배속인가?
    {
        m_Main_Speed = main_speed_var;
        Distance_Checker.GetInstance().Set_Speed(m_Lane_Speed * m_Main_Speed);
    }
}
