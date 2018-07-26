using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance_Checker : MonoBehaviour {

    static Distance_Checker m_Instance;
    IEnumerator m_DistanceChecker_Coroutine;

    float m_Distance = 0.0f;
    float m_Speed;

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
        m_Speed = s * 0.1f;
    }

    IEnumerator Distance_Check() // 거리 체크 코루틴.
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                m_Distance += m_Speed * Time.deltaTime;
                m_Distance_Text.text = m_Distance.ToString("N2") + " m"; // 소수점 2자리 까지 출력
            }
            yield return null;
        }
    }

    public float Get_Distance() // 거리 반환.
    {
        return m_Distance;
    }
}
