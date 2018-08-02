using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small_Enemy_1 : Enemy {
    
	void Start ()
    {
        Set_All_Status(ENEMY_TYPE.SMALL, 10.0f, 4.0f, 3.0f, 0.2f, -0.2f); // 스탯 설정
        Set_Up_Enemy_System(); // 시스템 가동
    }
}
