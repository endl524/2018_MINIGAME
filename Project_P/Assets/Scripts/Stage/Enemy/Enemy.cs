using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ENEMY_TYPE
{
    public const int SMALL = 0;
    public const int MIDDLE = 1;
    public const int BIG = 2;
}

public class Enemy : MonoBehaviour {

    static Enemy m_Instance;

    // 애니메이션
    protected Animation m_Animations;
    protected Animation m_Hurt_Blinker;

    // 시스템
    IEnumerator m_Move_Coroutine;

    // 스탯
    protected int m_Enemy_Type;
    protected float m_Health = 0.0f;
    protected float m_Move_Speed = 0.0f;
    protected float m_Hoping_Speed = 0.0f;
    protected float m_Hoping_Max = 0.0f;
    protected float m_Hoping_Min = 0.0f;

    bool m_is_Dead = false;
    protected bool m_is_Edge = false;
    
    protected Enemy()
    {
        m_Instance = this;
    }

    void OnDestroy()
    {
        StopCoroutine(m_Move_Coroutine);
    }

    public static Enemy GetInstance()
    {
        return m_Instance;
    }

    public bool Get_is_Dead()
    {
        return m_is_Dead;
    }

    // ==========================================

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !m_is_Dead)
        {
            if (m_Enemy_Type == ENEMY_TYPE.SMALL) // 소형은 같이죽고
            {
                StageManager.GetInstance().Plus_Obstacle_Destroy_Num(); // 장애물 파괴 개수 증가.
                Destroy(collision.gameObject);
                Dead_Start();
            }
            else
            {
                StageManager.GetInstance().Plus_Obstacle_Destroy_Num(); // 장애물 파괴 개수 증가.
                Destroy(collision.gameObject); // 나머지는 장애물만 부순다.
            }
        }

        if (collision.gameObject.CompareTag("Enemy_Catcher")) // 적 캐쳐에 닿으면
        {
            Destroy(gameObject); // 그냥 소멸하도록.
        }
    }


    IEnumerator Moving()
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause() && !m_is_Dead)
            {
                Move();
            }
            yield return null;
        }
    }

    IEnumerator Dead_Coroutine()
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                Dead();
            }
            yield return null;
        }
    }




    // ===============================================

    public void Hurt(float damage, float knock_back)
    {
        if (!m_is_Dead)
        {
            m_Health -= damage;

            if (m_Health <= 0.0f) Dead_Start();
            else transform.Translate(new Vector2(-knock_back, 0.0f)); // 넉백

            m_Hurt_Blinker.Play(m_Hurt_Blinker.GetClip("Enemy_Hurt2").name);
        }
    }

    protected virtual void Move()
    {
        if (transform.localPosition.y >= m_Hoping_Max || transform.localPosition.y <= m_Hoping_Min)
        {
            if (!m_is_Edge)
            {
                m_Hoping_Speed *= -1.0f;
                m_is_Edge = true;
            }
        }

        transform.Translate(new Vector3(m_Move_Speed * Time.deltaTime, m_Move_Speed * m_Hoping_Speed * Time.deltaTime * 0.5f, 0.0f));

        if (transform.localPosition.y < m_Hoping_Max && transform.localPosition.y > m_Hoping_Min)
        {
            m_is_Edge = false;
        }
    }

    void Dead_Start()
    {
        StageManager.GetInstance().Plus_Enemy_Kill_Num(m_Enemy_Type);
        m_is_Dead = true;
        m_Animations.Play(m_Animations.GetClip("Enemy_Dead").name);
        StartCoroutine(Dead_Coroutine());
    }

    void Dead()
    {
        if (m_Animations["Enemy_Dead"].normalizedTime >= 0.95f)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector2 pos = transform.localPosition;
            pos.x = pos.x + -1.5f * Time.deltaTime;
            pos.y = pos.y + 10.0f * Time.deltaTime;
            transform.localPosition = pos;
        }
    }

    // =====================================================


    protected void Set_All_Status(int type, float health, float move_speed, float hoping_speed, float hoping_max, float hoping_min)
    {
        m_Enemy_Type = type;
        m_Health = health;
        m_Move_Speed = move_speed;
        m_Hoping_Speed = hoping_speed;
        m_Hoping_Max = hoping_max;
        m_Hoping_Min = hoping_min;
    }

    protected void Set_Up_Enemy_System()
    {
        m_Animations = GetComponent<Animation>(); // 애니메이션 등록.
        m_Hurt_Blinker = GetComponentsInChildren<Animation>()[1]; // 피격 깜빡이 애니메이션 등록

        m_Move_Coroutine = Moving(); // 이동 코루틴 등록
        StartCoroutine(m_Move_Coroutine); // 이동 시작
    }
}
