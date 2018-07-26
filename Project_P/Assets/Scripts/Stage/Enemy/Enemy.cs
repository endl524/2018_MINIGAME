using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    static Enemy m_Instance;

    // 애니메이션
    protected Animation m_Animations;
    protected Animation m_Hurt_Blinker;

    // 시스템
    IEnumerator m_Move_Coroutine;

    // 스탯
    protected float m_Health = 0.0f;
    protected float m_Move_Speed = 0.0f;
    protected float m_Hoping_Speed = 0.0f;
    protected float m_Hoping_Max = 0.0f;
    protected float m_Hoping_Min = 0.0f;

    bool m_is_Dead = false;
    
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



    // ==========================================

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !m_is_Dead)
        {
            StageManager.GetInstance().Game_Over();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Obstacle") && !m_is_Dead)
        {
            Destroy(collision.gameObject);
            Dead_Start();
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
        m_Health -= damage;

        if (m_Health <= 0.0f)
        {
            m_is_Dead = true;
            m_Animations.Play(m_Animations.GetClip("Enemy_Dead").name);
            StartCoroutine(Dead_Coroutine());
        }
        else transform.Translate(new Vector2(-knock_back, 0.0f)); // 넉백
        
        m_Hurt_Blinker.Play(m_Hurt_Blinker.GetClip("Enemy_Hurt2").name);
    }

    protected virtual void Move() { }

    void Dead_Start()
    {
        m_is_Dead = true;
        m_Animations.Play(m_Animations.GetClip("Enemy_Dead").name);
        StartCoroutine(Dead_Coroutine());
    }

    void Dead()
    {
        if (m_Animations["Enemy_Dead"].normalizedTime >= 0.99f)
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


    protected void Set_All_Status(float health, float move_speed, float hoping_speed, float hoping_max, float hoping_min)
    {
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
