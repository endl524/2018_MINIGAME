using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 여러종류의 총알들을 하나의 스크립트로 관리하기 때문에
// => 발사에 사용되는 총알은 매번 스탯을 설정해주어야 한다.

// 2. 발사 후 소멸할 때 큐에 다시 넣는다.


public class Projectiles : MonoBehaviour {
    
    float m_Move_Speed;
    float m_Damage;
    float m_Knock_Back_Distance;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile_Catcher")) Dead(); // 화면 밖으로 나가면 소멸

        if (other.CompareTag("Enemey")) // 적과 충돌 시
        {
            other.gameObject.GetComponent<Enemy>().Hurt(m_Damage, m_Knock_Back_Distance); // 데미지와 넉백을 주고
            Dead(); // 소멸
        }

        if (other.CompareTag("Obstacle"))
        {
            // 1. 파괴 개수 카운팅 하도록 구현할것.
            Destroy(other.gameObject); // 장애물 파괴
            Dead(); // 소멸
        }
    }




    public void Set_Status(int gun_type, float damage, float speed, float knock_back) // 총의 종류에 맞게 총알의 스탯 설정.
    {
        m_Damage = damage;
        m_Move_Speed = speed;
        m_Knock_Back_Distance = knock_back;

        switch(gun_type)
        {
            case GUN_TYPE.SHOTGUN:
                Set_Random_Direction(gun_type);
                break;
        }
    }


    

    void Set_Random_Direction(int gun_type) // 랜덤방향 지정 메소드. (난사형 총기에 사용)
    {
        Quaternion rot = transform.rotation;

        switch (gun_type)
        {
            case GUN_TYPE.SHOTGUN:
                rot.z = Random.Range(-10.0f, 10.0f);
                break;
        }

        transform.rotation = rot;
    }





    IEnumerator Move() // 총알 이동
    {
        while(true)
        {
            // 1. 그저 주어진 방향과 속도를 가지고 직진하면 된다.
            if (StageManager.GetInstance().Get_isPause())
            {
                //transform.Translate(new Vector3(-m_Move_Speed * Time.deltaTime, ))
            }
            yield return null;
        }
    }





    void Dead() // 총알 소멸 (큐로 복귀)
    {
        // 1. 위치, 회전 값 초기화 시킬것.
        // 2. 진행중인 코루틴 전부 종료할것.
    }
}
