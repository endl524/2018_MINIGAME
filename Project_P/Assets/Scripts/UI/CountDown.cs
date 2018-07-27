using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
    
    public Texture m_CountDown_2;
    public Texture m_CountDown_1;
    public Texture m_CountDown_Start;

    int m_Current_Count = 0; // 3으로 할것
    

    RawImage m_CurrentImage;
    Animator m_CountDown_Animator;

    IEnumerator Count_Coroutine;

    void Start ()
    {
        m_CurrentImage = GetComponent<RawImage>();
        m_CountDown_Animator = GetComponent<Animator>();

        m_CurrentImage.enabled = true;
        Count_Coroutine = Counting();
        StartCoroutine(Count_Coroutine); // 카운팅 시작
    }

    void OnDestroy()
    {
        StopCoroutine(Count_Coroutine);
    }

    IEnumerator Counting()
    {
        while (true)
        {
            if (m_CountDown_Animator.GetFloat("Count_NormalizedTime") >= 0.99f) // 애니메이션이 끝나면
            {
                if (m_Current_Count == 3)
                {
                    m_CurrentImage.texture = m_CountDown_2;
                    m_Current_Count = 2;
                }
                else if (m_Current_Count == 2)
                {
                    m_CurrentImage.texture = m_CountDown_1;
                    m_Current_Count = 1;
                }
                else if (m_Current_Count == 1)
                {
                    m_CurrentImage.texture = m_CountDown_Start;
                    m_Current_Count = 0;
                }
                else if (m_Current_Count == 0)
                {
                    StageManager.GetInstance().Game_Start();
                    Destroy(gameObject);
                }
                m_CountDown_Animator.SetFloat("Count_NormalizedTime", 0.0f);
            }

            else m_CountDown_Animator.SetFloat("Count_NormalizedTime", m_CountDown_Animator.GetFloat("Count_NormalizedTime") + Time.deltaTime);
            
            yield return null;
        }
    }
}
