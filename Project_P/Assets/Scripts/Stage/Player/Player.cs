using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    static Player m_Instance;

    IEnumerator m_Move_Coroutine;
    IEnumerator m_Attack_Coroutine;
    float m_Gun_Reload_Time;
    bool m_isGunFired = false;

    float m_Move_Speed = 0.4f;
    float m_Max_Sens = 50.0f;
    float m_Min_Sens = -50.0f;


    float m_Move_Distance = 0.0f;
    float m_Curr_Mouse_Y;
    float m_Prev_Mouse_Y;

    bool m_is_Moving_ON = false;
    float m_Hoping_Speed = 4.0f;
    float m_Hoping_Max = 0.2f;
    float m_Hoping_Min = -0.2f;

    void Awake ()
    {
        m_Instance = this;
        m_Hoping_Max = 0.2f + transform.position.y;
        m_Hoping_Min = -0.2f + transform.position.y;

        m_Move_Coroutine = Move();
        m_Attack_Coroutine = Attack();
        StartCoroutine(m_Move_Coroutine);
        StartCoroutine(m_Attack_Coroutine);
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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StageManager.GetInstance().Game_Over();
            Destroy(gameObject);
        }
    }

    IEnumerator Move()
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause())
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
            if (!StageManager.GetInstance().Get_isPause())
            {
                if (m_Gun_Reload_Time >= Guns.GetInstance().Get_Gun_Reload_Time()) // 재장전이 완료되면.
                {
                    // 애니메이션 진행.
                    Guns.GetInstance().Get_Animator().SetFloat(Guns.GetInstance().Get_Ani_N_Times_Name(), Guns.GetInstance().Get_Animator().GetFloat(Guns.GetInstance().Get_Ani_N_Times_Name()) + Time.deltaTime * 3.0f);

                    if (!m_isGunFired) // 총알이 발사되지 않았다면
                    {
                        // 발사처리
                        Guns.GetInstance().Gun_Fire_Range_Activate(); // 충돌박스 On
                        m_isGunFired = true;
                    }
                    if (Guns.GetInstance().Get_Animator().GetFloat(Guns.GetInstance().Get_Ani_N_Times_Name()) >= 0.99f) // 애니메이션이 끝나면
                    {
                        Guns.GetInstance().Get_Animator().SetFloat(Guns.GetInstance().Get_Ani_N_Times_Name(), 0.0f); // 애니메이션 초기화
                        m_Gun_Reload_Time = 0.0f; // 재장전 시간 초기화.
                        m_isGunFired = false;
                        Guns.GetInstance().Fire_End(); // 데미지 리스트 초기화 및 충돌박스 Off
                    }
                }
                else m_Gun_Reload_Time += Time.deltaTime; // 재장전 대기.
            }
            yield return null;
        }
    }
}
