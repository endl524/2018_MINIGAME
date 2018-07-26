using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small_Enemy_2 : Enemy {
    
	void Start ()
    {
        Set_All_Status(30.0f, 2.0f, 6.0f, 3.5f, -1.7f); // 스탯 설정
        Set_Up_Enemy_System(); // 시스템 가동
    }
    

    protected override void Move()
    {
        if (transform.localPosition.y >= m_Hoping_Max || transform.localPosition.y <= m_Hoping_Min)
            m_Hoping_Speed *= -1.0f;

        Vector2 pos = transform.localPosition;
        pos.x = pos.x + m_Move_Speed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y + m_Move_Speed * m_Hoping_Speed * Time.deltaTime, m_Hoping_Min, m_Hoping_Max);
        transform.localPosition = pos;

        m_Animations.Play(m_Animations.GetClip("Small_Enemy_2_Move").name);
    }
}
