using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    IEnumerator m_Move_Coroutine;

    protected float m_Move_Speed;

    void OnDestroy()
    {
        StopCoroutine(m_Move_Coroutine);
    }


    // ================================


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
    }


    // =================================

    protected void Set_Up_Obstacle_System(float speed)
    {
        m_Move_Speed = speed;
        m_Move_Coroutine = Moving();
        StartCoroutine(m_Move_Coroutine);
    }

    protected void Set_Speed(float speed)
    {
        m_Move_Speed = speed;
    }
}
