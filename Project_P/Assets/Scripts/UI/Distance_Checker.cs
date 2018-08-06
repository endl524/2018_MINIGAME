using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance_Checker : MonoBehaviour {

    static Distance_Checker m_Instance;
    IEnumerator m_DistanceChecker_Coroutine;

    float m_Distance = 0.0f;
    float m_Distance_1m_Check = 0.0f;
    float m_Distance_10m_Check = 0.0f;
    float m_Distance_100m_Check = 0.0f;
    float m_Distance_300m_Check = 0.0f;
    
    float m_Speed;
    float m_Dist_Var;

    public Text m_Distance_Text;
    

    void Awake()
    {
        m_Instance = this;
        
        Set_Speed(MapScroll.GetInstance().Get_Lane_Speed());

        m_DistanceChecker_Coroutine = Distance_Check();

        StartCoroutine(m_DistanceChecker_Coroutine);
    }

    void OnDestroy()
    {
        StopCoroutine(m_DistanceChecker_Coroutine);
    }

    public static Distance_Checker GetInstance()
    {
        return m_Instance;
    }

    public void Set_Speed(float s) // 속도값 변경.
    {
        m_Speed = s * 0.2f;
    }

    IEnumerator Distance_Check() // 거리 체크 코루틴.
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                m_Dist_Var = m_Speed * Time.deltaTime;
                m_Distance += m_Dist_Var;
                m_Distance_Text.text = m_Distance.ToString("N2") + " m"; // 소수점 2자리 까지 출력

                if (!StageManager.GetInstance().Get_is_FeverTime_On()) // 피버타임이 꺼졌을 때
                {
                    m_Distance_1m_Check += m_Dist_Var;
                    m_Distance_10m_Check += m_Dist_Var;
                    m_Distance_100m_Check += m_Dist_Var;
                    m_Distance_300m_Check += m_Dist_Var;

                    if (m_Distance_1m_Check >= 1.0f)
                    {
                        StageManager.GetInstance().Plus_FeverTime_Point(FEVER_TIME_POINT.DISTANCE_1M * System.Convert.ToInt32(m_Distance_1m_Check));
                        m_Distance_1m_Check -= System.Convert.ToInt32(m_Distance_1m_Check);
                    }

                    if (m_Distance_10m_Check >= 10.0f)
                    {
                        StageManager.GetInstance().Plus_FeverTime_Point(FEVER_TIME_POINT.DISTANCE_10M * System.Convert.ToInt32(m_Distance_10m_Check * 0.1f));
                        m_Distance_10m_Check -= System.Convert.ToInt32(m_Distance_10m_Check);
                    }

                    if (m_Distance_100m_Check >= 100.0f)
                    {
                        StageManager.GetInstance().Plus_FeverTime_Point(FEVER_TIME_POINT.DISTANCE_100M * System.Convert.ToInt32(m_Distance_100m_Check * 0.01f));
                        m_Distance_100m_Check -= System.Convert.ToInt32(m_Distance_100m_Check);
                    }

                    if (m_Distance_300m_Check >= 300.0f)
                    {
                        StageManager.GetInstance().Plus_FeverTime_Point(FEVER_TIME_POINT.DISTANCE_300M * System.Convert.ToInt32(m_Distance_300m_Check * 0.03f));
                        m_Distance_300m_Check -= System.Convert.ToInt32(m_Distance_300m_Check);
                    }
                }
            }
            yield return null;
        }
    }

    public float Get_Distance() // 거리 반환.
    {
        return m_Distance;
    }

    public void Reset_Distance_For_Fever_Point() // 거리 체크 변수 초기화.
    {
        m_Distance_1m_Check = 0.0f;
        m_Distance_10m_Check = 0.0f;
        m_Distance_100m_Check = 0.0f;
        m_Distance_300m_Check = 0.0f;
    }
}
