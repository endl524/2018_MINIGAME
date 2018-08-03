using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Enemy_1 : Enemy
{
    Middle_Enemy_1_Detector m_Detector;
    public GameObject m_Target;

    int m_Move_Dir = 1;

    float m_Finding_Speed = 3.0f;
    int m_Finding_Dir = 1;
    float m_Sudden_Attack_CoolTime = 5.0f; // 급습 쿨타임
    float m_Waited_SA_cooltime = 0.0f; // 급습 쿨타임 기다린 시간
    float m_Following_Time = 3.0f; // 따라가기 지속 시간
    float m_Followed_Time = 0.0f; // 따라간 시간

    IEnumerator m_Pos_X_Checker;
    IEnumerator m_Move_Controller;
    IEnumerator m_Ready_To_Attack;
    IEnumerator m_Follow;

    void Start()
    {
        Set_All_Status(ENEMY_TYPE.MIDDLE, 300.0f, 4.0f, 1.0f, 0.1f, -0.1f); // 스탯 설정
        Set_Up_Enemy_System(); // 시스템 가동

        m_Detector = GetComponentInChildren<Middle_Enemy_1_Detector>(); // 감지기 등록

        m_Pos_X_Checker = Pos_X_Check(); // x좌표 체커 등록.
        m_Move_Controller = Move_Controller(); // 이동 컨트롤러 등록.
        m_Ready_To_Attack = Ready_To_Attack(); // 공격 대기 코루틴 등록.
        m_Follow = Following(); // 따라가기 코루틴 등록.

        m_Waited_SA_cooltime = m_Sudden_Attack_CoolTime; // 쿨타임 Full

        StartCoroutine(m_Pos_X_Checker); // x좌표 체크 실행.
        StartCoroutine(m_Ready_To_Attack); // 공격 대기 실행.
    }

    IEnumerator Pos_X_Check() // x좌표를 체크 후 이동제한 코루틴을 실행함.
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (transform.localPosition.x >= 8.75f)
                {
                    StartCoroutine(m_Move_Controller); // 이동 컨트롤 실행.
                    StopCoroutine(m_Pos_X_Checker); // x 좌표 체크 종료.
                }
            }
            yield return null;
        }
    }

    IEnumerator Move_Controller() // 이동에 제한을 걸어주는 코루틴
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (transform.localPosition.x >= 9.5f || transform.localPosition.x <= 8.5f)
                {
                    m_Move_Dir *= -1;
                }
            }
            yield return null;
        }
    }



    protected override void Move() // 이동 오버로딩
    {
        // 현재 Hoping에 문제 발생.
        /*
        if (transform.localPosition.y >= m_Hoping_Max || transform.localPosition.y <= m_Hoping_Min)
        {
            if (!m_is_Edge)
            {
                m_Hoping_Speed *= -1.0f;
                m_is_Edge = true;
            }
        }

        transform.Translate(new Vector3(m_Move_Speed * m_Move_Dir * Time.deltaTime, m_Move_Speed * m_Hoping_Speed * Time.deltaTime * 0.5f, 0.0f));

        if (transform.localPosition.y < m_Hoping_Max && transform.localPosition.y > m_Hoping_Min)
        {
            m_is_Edge = false;
        }
        */
        // 당분간 앞뒤로만 움직이도록.
        transform.Translate(new Vector3(m_Move_Speed * m_Move_Dir * Time.deltaTime, 0.0f, 0.0f));

    }



    IEnumerator Ready_To_Attack() // 공격 준비. (감지 및 쿨타임 대기)
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (m_Waited_SA_cooltime >= m_Sudden_Attack_CoolTime) // 쿨타임이 다 찼고,
                {
                    if (m_Detector.Get_is_Detected()) // 탐지 성공 시
                    {
                        Dive(); // 잠수 시작.
                    }
                }

                else m_Waited_SA_cooltime += Time.deltaTime; // 쿨타임이 될 때 까지 경과시간을 더해준다.
            }
            yield return null;
        }
    }


    void Dive() // 잠수 시작
    {
        m_Animations.Play(m_Animations.GetClip("Middle_Enemy_1_Dive").name); // 잠수 애니메이션 실행.
        StopCoroutine(m_Ready_To_Attack); // 공격 대기를 멈춘다.
        m_Waited_SA_cooltime = 0.0f; // 대기한 쿨타임 초기화.
        m_Target = m_Detector.Get_Target(); // 타겟을 받아온다.

        m_Followed_Time = 0.0f; // 따라가기 시간 초기화.
        StartCoroutine(m_Follow); // 따라가기 시작.
    }


    IEnumerator Following() // 따라가기 수행
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (m_Followed_Time <= m_Following_Time) // 따라가기 지속시간이 될 때 까지
                {
                    m_Followed_Time += Time.deltaTime;
                    Vector3 pos = m_Target.transform.position;
                    pos.x = transform.position.x;
                    transform.position = pos;
                }

                else
                {
                    Sudden_Attack();
                }
            }
            yield return null;
        }
    }

    void Sudden_Attack() // 급습!
    {
        m_Animations.Play(m_Animations.GetClip("Middle_Enemy_1_Sudden_Attack").name); // 급습 애니메이션 실행.
        
        if (m_Animations["Middle_Enemy_1_Sudden_Attack"].normalizedTime >= 0.95f)
        {
            StopCoroutine(m_Follow); // 따라가기를 멈춘다.
            m_Followed_Time = 0.0f; // 따라가기 시간 초기화.

            m_Target = null; // 타겟 초기화.
            StartCoroutine(m_Ready_To_Attack); // 공격 대기 시작.
        }
    }
}
