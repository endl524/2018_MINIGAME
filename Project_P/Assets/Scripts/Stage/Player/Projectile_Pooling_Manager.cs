using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 큐를 통해 오브젝트 풀링을 한다.
// 총을 발사하면, 발사한 총알 개수만큼 큐에서 팝한다.

public class Projectile_Pooling_Manager : MonoBehaviour {

    static Projectile_Pooling_Manager m_Instance;

    void Awake ()
    {
		// 큐를 만들어 총알들을 등록
	}

    public static Projectile_Pooling_Manager GetInstance()
    {
        return m_Instance;
    }
}
