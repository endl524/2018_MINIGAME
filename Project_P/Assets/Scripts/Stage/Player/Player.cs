using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    static Player m_Instance;

    Animation m_Player_Animation;

    IEnumerator m_Move_Coroutine;
    IEnumerator m_Attack_Coroutine;
    IEnumerator m_Dead_Directer;

    float m_Move_Speed = 0.4f;
    float m_Max_Sens = 60.0f;
    float m_Min_Sens = -60.0f;


    float m_Move_Distance = 0.0f;
    float m_Curr_Mouse_Y;
    float m_Prev_Mouse_Y;

    bool m_is_Moving_ON = false;
    float m_Hoping_Speed = 4.0f;
    float m_Hoping_Max = 0.2f;
    float m_Hoping_Min = -0.2f;

    bool m_is_Invincible = false; // 무적 상태인가?
    bool m_is_Alive = true; // 살아 있는가?

    void Awake ()
    {
        m_Instance = this;
        m_Hoping_Max = 0.2f + transform.position.y;
        m_Hoping_Min = -0.2f + transform.position.y;

        m_Player_Animation = GetComponent<Animation>();

        m_Move_Coroutine = Move();
        m_Attack_Coroutine = Attack();
        m_Dead_Directer = Dead_Directing();

        StartCoroutine(m_Move_Coroutine);
        StartCoroutine(m_Attack_Coroutine);

        if (StageManager.GetInstance().Get_is_Debug_Mode() && StageManager.GetInstance().Get_is_invincible_On())
            m_is_Invincible = true;
    }

    void OnDestroy()
    {
        StopCoroutine(m_Move_Coroutine);
        StopCoroutine(m_Attack_Coroutine);
    }

    public static Player GetInstance()
    {
        return m_Instance;
    }


    void OnTriggerEnter(Collider collision)
    {
        if (m_is_Alive)
        {
            if (!m_is_Invincible) // 무적이 아닐때만..
            {
                // 장애물에 닿거나,
                // 살아있는 적에게 닿으면 사망.
                if (collision.gameObject.CompareTag("Obstacle") ||
                    (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<Enemy>().Get_is_Dead()))
                {
                    Dead_Start();
                }
            }

            if (collision.gameObject.CompareTag("Item_Fish"))
            {
                StageManager.GetInstance().Plus_Fish_Item_Num();
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Item_DSMG"))
            {
                Guns.GetInstance().Gun_Change(GUN_TYPE.DSMG);
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Item_ShotGun"))
            {
                Guns.GetInstance().Gun_Change(GUN_TYPE.SHOTGUN);
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Item_AR"))
            {
                Guns.GetInstance().Gun_Change(GUN_TYPE.AR);
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator Move()
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause() && m_is_Alive)
            {
                if (Input.GetMouseButtonDown(0)) // 화면에서 뗀 상태에서 클릭 시 한번만 수행.
                {
                    m_Curr_Mouse_Y = Input.mousePosition.y;
                    m_Prev_Mouse_Y = m_Curr_Mouse_Y;
                    m_is_Moving_ON = true;
                }
                if (Input.GetMouseButton(0)) // 클릭 중인 상태.
                {
                    m_Curr_Mouse_Y = Input.mousePosition.y;
                    float mouseMovingDistance = Mathf.Clamp(m_Curr_Mouse_Y - m_Prev_Mouse_Y, m_Min_Sens, m_Max_Sens); // 마우스 감도 범위 조절
                    m_Move_Distance = Mathf.Clamp(transform.position.y + m_Move_Speed * mouseMovingDistance * Time.deltaTime, Map_Movable_Range.Min, Map_Movable_Range.Max); // 움직일수있는 범위 조절
                    float z_Move = Mathf.Clamp(transform.position.z + m_Move_Speed * mouseMovingDistance * Time.deltaTime, Lane_Z_Range.Min, Lane_Z_Range.Max); // z 축도 이동.
                    transform.position = new Vector3(transform.position.x, m_Move_Distance, z_Move);
                    m_Prev_Mouse_Y = m_Curr_Mouse_Y;
                }
                if (Input.GetMouseButtonUp(0)) // 화면에서 뗀 순간.
                {
                    m_is_Moving_ON = false;
                    m_Hoping_Max = 0.2f + transform.position.y;
                    m_Hoping_Min = -0.2f + transform.position.y;
                }
                if (!m_is_Moving_ON)
                {
                    if (transform.position.y >= m_Hoping_Max || transform.position.y <= m_Hoping_Min)
                        m_Hoping_Speed *= -1.0f;

                    Vector3 pos = transform.position;
                    pos.y = Mathf.Clamp(pos.y + m_Move_Speed * m_Hoping_Speed * Time.deltaTime, m_Hoping_Min, m_Hoping_Max);
                    transform.position = pos;
                }
            }
            yield return null;
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause() && m_is_Alive)
            {
                Guns.GetInstance().GunFire();
            }
            yield return null;
        }
    }
    
    void Dead_Start()
    {
        m_Player_Animation.Play(m_Player_Animation.GetClip("Player_Dead").name);
        StartCoroutine(m_Dead_Directer);
        m_is_Alive = false;
    }

    IEnumerator Dead_Directing()
    {
        while (true)
        {
            if (m_Player_Animation["Player_Dead"].normalizedTime >= 0.95f)
            {
                StageManager.GetInstance().Game_Over();
                StopCoroutine(m_Dead_Directer);
            }
            yield return null;
        }
    }

    public void Invincibility_Change()
    {
        if (m_is_Invincible)
            m_is_Invincible = false;
        else m_is_Invincible = true;
    }
}
