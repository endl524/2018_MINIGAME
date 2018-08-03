using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_And_Item: MonoBehaviour {

    protected Animation m_Animations;

    IEnumerator m_Move_Coroutine;

    protected float m_Move_Speed;

    bool m_is_Destroied = false;
    

    void OnDestroy()
    {
        StopCoroutine(m_Move_Coroutine);
    }

    // ================================

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile_Catcher")) // 총알 캐쳐를 같이 사용하도록 한다.
        {
            Destroy(gameObject); // 화면 밖으로 나가면 소멸.
        }
    }


    IEnumerator Moving()
    {
        while (true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                Move();
            }
            yield return null;
        }
    }


    // =================================


    protected virtual void Move()
    {
        transform.Translate(new Vector3(-m_Move_Speed * Time.deltaTime, 0.0f, 0.0f));
        m_Animations.Play(m_Animations.GetClip("O_AND_I_Floating").name);
    }


    // =================================

    protected void Set_Up_Obstacle_System(float speed)
    {
        m_Animations = GetComponent<Animation>();
        m_Move_Speed = speed;
        m_Move_Coroutine = Moving();
        StartCoroutine(m_Move_Coroutine);
    }

    protected void Set_Speed(float speed)
    {
        m_Move_Speed = speed;
    }

    public bool Get_is_Destroied()
    {
        return m_is_Destroied;
    }

    public void Set_is_Destroied(bool b)
    {
        m_is_Destroied = b;
    }
}
