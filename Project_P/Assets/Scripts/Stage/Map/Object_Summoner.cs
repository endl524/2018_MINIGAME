using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class OBJECT_TYPE
{
    public const int ENEMY = 0;
    public const int OBSTACLE = 1;
    public const int ITEM = 2;
}

static class MONSTER_NUMBER
{
    public const int SMALL_1 = 0;
    public const int SMALL_2 = 1;
}

static class OBSTACLE_NUMBER
{
    public const int WOODEN_BOX = 0;
    public const int ICE_1 = 1;
}

static class ITEM_NUMBER
{
    public const int FISH = 0;
    public const int DSMG = 1;
    public const int SHOTGUN = 2;
    public const int AR = 3;
}

public class Object_Summoner : MonoBehaviour {

    public GameObject[] m_Object_Lanes; // 적 출현 레인들.
    public GameObject[] m_Object_Prefabs; // 적 프리팹들.

    Object_Pattern_Structure m_Object_Pattern; // 임시로 패턴을 담을 구조체

    GameObject m_Temp_Object; // 임시 객체

    public int m_Object_Type;
    public float m_Summon_Time = 10.0f; // 소환 대기 시간. (Default 10초로 설정.)
    float m_Waited_Summon_Time; // 실제 대기한 시간.

    IEnumerator m_Summon_Coroutine;
    IEnumerator m_FeverTime_Checker;

    float m_Normal_Summon_Time; // 피버타임 대비용. (소환 대기 시간 저장)
    bool m_is_Fever_Mode_Setting_Over = false;

	void Start ()
    {
        m_Waited_Summon_Time = m_Summon_Time;
        
        m_Summon_Coroutine = Summon_Object(); // 소환 코루틴 등록.
        m_FeverTime_Checker = FeverTime_Check(); // 피버타임 체크 코루틴 등록.
        StartCoroutine(m_Summon_Coroutine); // 소환 시작.
        StartCoroutine(m_FeverTime_Checker); // 피버타임 체크 시작.
    }

    IEnumerator Summon_Object()
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (m_Object_Type == OBJECT_TYPE.ENEMY && StageManager.GetInstance().Get_is_FeverTime_On())
                    yield return null;

                else if (m_Waited_Summon_Time >= m_Summon_Time) // 소환 시간이 되면
                {
                    switch(m_Object_Type) // 무작위 패턴을 불러온다.
                    {
                        case OBJECT_TYPE.ENEMY:
                            m_Object_Pattern = StageManager.GetInstance().Random_Pick_One_Monster_Pattern();
                            break;

                        case OBJECT_TYPE.OBSTACLE:
                            m_Object_Pattern = StageManager.GetInstance().Random_Pick_One_Obstacle_Pattern();
                            break;

                        case OBJECT_TYPE.ITEM:
                            m_Object_Pattern = StageManager.GetInstance().Random_Pick_One_Item_Pattern();
                            break;
                    }

                    for (int i = 0; i < m_Object_Pattern.Count_of_Objects; ++i)
                    {
                        m_Temp_Object = Instantiate(m_Object_Prefabs[m_Object_Pattern.Object_Number[i] - 1]); // 객체를 인스턴싱.
                        m_Temp_Object.transform.SetParent(m_Object_Lanes[m_Object_Pattern.Line_Number[i] - 1].transform); // 그 다음 해당 레인의 자식으로 설정.
                        Vector3 lpos = Vector3.zero;
                        lpos.x = m_Object_Pattern.X_Offset_Distance[i];
                        m_Temp_Object.transform.localPosition = lpos; // 포지션 설정.
                    }
                    m_Waited_Summon_Time = 0.0f;
                }

                else m_Waited_Summon_Time += Time.deltaTime;
            }

            yield return null;
        }
    }

    

    IEnumerator FeverTime_Check()
    {
        while(true)
        {
            if (StageManager.GetInstance().Get_is_FeverTime_On() && !m_is_Fever_Mode_Setting_Over) // 피버타임이 활성화되면
            {
                m_Normal_Summon_Time = m_Summon_Time; // 원래 소환 시간을 기억해두고
                m_Summon_Time = m_Summon_Time * 0.2f; // 소환시간을 5배 빠르게 한다.
                m_is_Fever_Mode_Setting_Over = true; // 세팅 완료 알림.
            }

            if (!StageManager.GetInstance().Get_is_FeverTime_On() && m_is_Fever_Mode_Setting_Over) // 피버타임 세팅이 끝났고, 피버타임이 끝났다면
            {
                m_Summon_Time = m_Normal_Summon_Time; // 원래 소환 시간으로 복귀.
                m_is_Fever_Mode_Setting_Over = false; // 세팅 완료 알림 종료.
            }
            yield return null;
        }
    }


    


    public void Start_Spawn()
    {
        StartCoroutine(m_Summon_Coroutine); // 소환 코루틴 시작
    }

    public void Stop_Spawn()
    {
        StopCoroutine(m_Summon_Coroutine); // 소환 코루틴 종료
    }


    public float Get_Summon_Time()
    {
        return m_Summon_Time;
    }

    public void Set_Summon_Time(float t)
    {
        m_Summon_Time = t;
    }

}
