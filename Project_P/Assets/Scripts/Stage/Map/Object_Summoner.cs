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
}

public class Object_Summoner : MonoBehaviour {

    public GameObject[] m_Object_Lanes; // 적 출현 레인들.
    public GameObject[] m_Object_Prefabs; // 적 프리팹들.

    Object_Pattern_Structure m_Object_Pattern; // 임시로 패턴을 담을 구조체

    GameObject m_Temp_Object; // 임시 객체

    public int m_Object_Type;
    public float m_Summon_Time = 10.0f; // 소환 대기 시간. (Default 10초로 설정.)
    float m_Waited_Summon_Time; // 실제 대기한 시간.
    
	void Start ()
    {
        m_Waited_Summon_Time = m_Summon_Time;
        StartCoroutine(Summon_Object()); // 소환 시작
	}

    IEnumerator Summon_Object()
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (m_Waited_Summon_Time >= m_Summon_Time) // 소환 시간이 되면
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

    public float Get_Summon_Time()
    {
        return m_Summon_Time;
    }

    public void Set_Summon_Time(float t)
    {
        m_Summon_Time = t;
    }

}
