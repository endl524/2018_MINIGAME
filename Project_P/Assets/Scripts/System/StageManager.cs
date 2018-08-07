using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class FEVER_TIME_POINT
{
    public const int FISH_ITEM = 100; // 물고기 아이템 획득 시 마다
    public const int DISTANCE_1M = 10; // 1m 마다
    public const int DISTANCE_10M = 50; // 10m 마다
    public const int DISTANCE_100M = 1000; // 100m 마다
    public const int DISTANCE_300M = 3000; // 300m 마다
}

static class FEVER_TIME_SPEED
{
    public const float NORMAL = 1.0f; // 기존상태 = 속도 1배
    public const float SLOWDOWN_X2 = 2.0f; // 감속 (2배)
    public const float SLOWDOWN_X3 = 3.0f; // 감속 (3배)
    public const float SLOWDOWN_X4 = 4.0f; // 감속 (4배)
    public const float FEVER = 5.0f; // 피버타임 = 속도 5배
}

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

    public GameObject m_Fade_Object; // 페이드 이미지 객체

    bool m_is_FeverTime_On = false; // 피버타임이 발동했는가?
    int m_FeverTime_Max_Point = 10000; // 피버타임 발동 포인트
    public int m_Curr_FeverTime_Point = 0; // 현재 모인 피버타임 포인트
    float m_FeverTime_Duration = 5.0f; // 피버타임 유지시간.
    float m_Elapsed_FeverTime; // 피버타임 경과시간.
    float m_SlowDown_Time; // 피버타임 종료 시 급정지가 아닌 천천히 정지하도록 유도하기위한 장치.


    IEnumerator m_FeverTime;
    IEnumerator m_SlowDown;


    public bool m_is_Debug_Mode; // 디버그 모드용이다. (CountDown의 카운트 숫자와 Player의 무적상태 관련.)
    public bool m_is_invincible_On; // 디버그 모드용이다. (Player의 무적상태 관련.)

    void Awake ()
    {
        m_Instance = this;
        m_isPause = true;
        m_isGameOver = false;

        m_FeverTime = FeverTime();
        m_SlowDown = SlowDown();

        m_Max_Enemy_Num_in_One_Pattern = 15; // 일단은 최대 15마리로 설정.
        m_Enemy_Pattern_List = new List<Object_Pattern_Structure>(); // 적 패턴 리스트 할당
        CSV_Manager.GetInstance().Get_Object_Pattern_List(ref m_Enemy_Pattern_List, m_Max_Enemy_Num_in_One_Pattern, OBJECT_TYPE.ENEMY); // "적 패턴 리스트"를 받아온다.

        m_Max_Obstacle_Num_in_One_Pattern = 15; // 일단은 최대 15개로 설정.
        m_Obstacle_Pattern_List = new List<Object_Pattern_Structure>(); // 장애물 패턴 리스트 할당
        CSV_Manager.GetInstance().Get_Object_Pattern_List(ref m_Obstacle_Pattern_List, m_Max_Obstacle_Num_in_One_Pattern, OBJECT_TYPE.OBSTACLE); // "장애물 패턴 리스트"를 받아온다.

        m_Max_Item_Num_in_One_Pattern = 22; // 일단은 최대 20개로 설정.
        m_Item_Pattern_List = new List<Object_Pattern_Structure>(); // 아이템 패턴 리스트 할당
        CSV_Manager.GetInstance().Get_Object_Pattern_List(ref m_Item_Pattern_List, m_Max_Item_Num_in_One_Pattern, OBJECT_TYPE.ITEM); // "아이템 패턴 리스트"를 받아온다.


        m_Elapsed_FeverTime = m_FeverTime_Duration; // 피버타임 시간 초기화.
        StartCoroutine(m_FeverTime); // 피버타임 검사 시작.
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
        m_isPause = true;
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
        Score_Board_UI.GetInstance().Set_Item_Num_Text(m_Got_Fish_Item_Num);

        if (!m_is_FeverTime_On)
            m_Curr_FeverTime_Point += FEVER_TIME_POINT.FISH_ITEM;
    }

    public void Plus_Obstacle_Destroy_Num()
    {
        ++m_Obstacle_Destroy_Num;
        Score_Board_UI.GetInstance().Set_Obstacle_Num_Text(m_Obstacle_Destroy_Num);
    }

    public void Plus_Enemy_Kill_Num(int type)
    {
        if (!m_is_FeverTime_On) // 피버타임이 끝나야 가능.
        {
            switch (type)
            {
                case ENEMY_TYPE.SMALL:
                    ++m_Small_Enemy_Kill_Num;
                    Score_Board_UI.GetInstance().Set_Small_Kill_Num_Text(m_Small_Enemy_Kill_Num);
                    break;

                case ENEMY_TYPE.MIDDLE:
                    ++m_Middle_Enemy_Kill_Num;
                    Score_Board_UI.GetInstance().Set_Middle_Kill_Num_Text(m_Middle_Enemy_Kill_Num);
                    break;

                case ENEMY_TYPE.BOSS:
                    ++m_Big_Enemy_Kill_Num;
                    Score_Board_UI.GetInstance().Set_Big_Kill_Num_Text(m_Big_Enemy_Kill_Num);
                    break;
            }
        }
    }



    public bool Get_is_FeverTime_On()
    {
        return m_is_FeverTime_On;
    }

    public float Get_FeverTime_Max_Point()
    {
        return m_FeverTime_Max_Point;
    }

    public float Get_Curr_FeverTime_Point()
    {
        return m_Curr_FeverTime_Point;
    }

    public float Get_FeverTime_Duration()
    {
        return m_FeverTime_Duration;
    }

    public float Get_Elapsed_FeverTime()
    {
        return m_Elapsed_FeverTime;
    }

    public void Plus_FeverTime_Point(int point) // 피버타임 포인트를 추가하는 메소드.
    {
        m_Curr_FeverTime_Point += point;
    }

    IEnumerator FeverTime()
    {
        while(true)
        {
            if (!m_isPause)
            {
                if (m_is_FeverTime_On) // 피버타임이 켜졌을 때
                {
                    if (m_Elapsed_FeverTime > 0.0f) // 피버타임 시간이 남았다면
                        m_Elapsed_FeverTime -= Time.deltaTime; // 시간 감소.

                    else // 피버타임 시간이 다 됐다면
                        SlowDown_Start(); // 감속 시작.
                }

                else // 피버타임이 꺼졌을 때
                {
                    if (m_Curr_FeverTime_Point >= m_FeverTime_Max_Point) // 포인트가 충분히 모였다면
                    {
                        m_is_FeverTime_On = true; // 피버타임 시작.
                        MapScroll.GetInstance().Change_Speed(FEVER_TIME_SPEED.FEVER); // 맵 스크롤 속도 5배 상승.
                        MapScroll.GetInstance().Map_Change_Fever(); // 맵을 피버타임 모드로 변경.
                        Enemy_All_Kill(); // 맵에 존재하는 적 객체를 전부 처치한다.
                        Player.GetInstance().Invincibility_Change(); // 무적 활성화.
                        Player.GetInstance().Attack_Off(); // 공격 중지.
                    }
                }
            }
            yield return null;
        }
    }

    void SlowDown_Start()
    {
        StopCoroutine(m_FeverTime);
        StartCoroutine(m_SlowDown);
    }

    IEnumerator SlowDown()
    {
        while (true)
        {
            if (m_SlowDown_Time >= 1.0f)
            {
                m_is_FeverTime_On = false; // 피버타임 종료.

                Player.GetInstance().Invincibility_Change(); // 무적 비활성화.
                MapScroll.GetInstance().Change_Speed(FEVER_TIME_SPEED.NORMAL); // 맵스크롤 속도 초기화.
                MapScroll.GetInstance().Map_Change_Normal(); // 맵을 노말 모드로 변경.
                m_Elapsed_FeverTime = m_FeverTime_Duration; // 시간 초기화.
                m_Curr_FeverTime_Point = 0; // 포인트 초기화.
                m_SlowDown_Time = 0.0f; // 감속 시간 초기화.
                Distance_Checker.GetInstance().Reset_Distance_For_Fever_Point(); // 피버타임용 거리 측정기 초기화.
                Player.GetInstance().Attack_On(); // 공격 재시작.

                StopCoroutine(m_SlowDown); // 감속 종료
                StartCoroutine(m_FeverTime); // 피버타임 검사 재시작.
            }

            else if (m_SlowDown_Time >= 0.75f)
            {
                MapScroll.GetInstance().Change_Speed(FEVER_TIME_SPEED.SLOWDOWN_X2); // 이동속도 초기화.
            }

            else if (m_SlowDown_Time >= 0.5f)
            {
                MapScroll.GetInstance().Change_Speed(FEVER_TIME_SPEED.SLOWDOWN_X3); // 이동속도 초기화.
            }

            else if (m_SlowDown_Time >= 0.25f)
            {
                MapScroll.GetInstance().Change_Speed(FEVER_TIME_SPEED.SLOWDOWN_X4); // 이동속도 초기화.
                m_Fade_Object.GetComponent<Fade_Manager>().Fade_In(); // 페이드 인 실행.
            }

            m_SlowDown_Time += Time.deltaTime;

            yield return null;
        }
    }

    void Enemy_All_Kill()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; ++i)
        {
            enemies[i].GetComponent<Enemy>().Dead_Start(); // 점수 획득 버전.
        }
    }

    public bool Get_is_Debug_Mode()
    {
        return m_is_Debug_Mode;
    }

    public bool Get_is_invincible_On()
    {
        return m_is_invincible_On;
    }
}