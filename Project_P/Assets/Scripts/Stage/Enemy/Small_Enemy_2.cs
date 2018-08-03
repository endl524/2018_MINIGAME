using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small_Enemy_2 : Enemy {
    
	void Start ()
    {
        Set_All_Status(ENEMY_TYPE.SMALL, 25.0f, 2.5f, 6.0f, 2.0f, -2.0f); // 스탯 설정
        Set_Up_Enemy_System(); // 시스템 가동

        GetComponentInChildren<SpriteRenderer>().transform.Rotate(0.0f, 0.0f, 70.0f);
    }

    protected override void Move()
    {
        if (transform.localPosition.y >= m_Hoping_Max)
        {
            m_Animations.Play(m_Animations.GetClip("Small_Enemy_2_Move_Down").name);

            if (!m_is_Edge)
            {
                m_Hoping_Speed *= -1.0f;
                m_is_Edge = true;
            }
        }

        if (transform.localPosition.y <= m_Hoping_Min)
        {
            m_Animations.Play(m_Animations.GetClip("Small_Enemy_2_Move").name);

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
}
