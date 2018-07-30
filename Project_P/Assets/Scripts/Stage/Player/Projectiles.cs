using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 여러종류의 총알들을 하나의 스크립트로 관리하기 때문에
// => 발사에 사용되는 총알은 매번 스탯을 설정해주어야 한다.

// 2. 발사 후 소멸할 때 큐에 다시 넣는다.

static class BULLET_SIZE
{
    public const float SHOTGUN_X = 1.0f;
    public const float SHOTGUN_Y = 1.0f;
    public const float DSMG_X = 0.7f;
    public const float DSMG_Y = 0.3f;
}

static class BULLET_FIRST_POSITION
{
    public const float SHOTGUN_X = -1.1f;
    public const float SHOTGUN_Y = -3.1f;
    //public const float DSMG_X = 0.7f;
    //public const float DSMG_Y = 0.3f;
}

public class Projectiles : MonoBehaviour {
    
    float m_Move_Speed;
    float m_Damage;
    float m_Knock_Back_Distance;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile_Catcher")) Dead(); // 화면 밖으로 나가면 소멸

        if (other.CompareTag("Enemy")) // 적과 충돌 시
        {
            other.gameObject.GetComponent<Enemy>().Hurt(m_Damage, m_Knock_Back_Distance); // 데미지와 넉백을 주고
            Dead(); // 소멸
        }

        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // 장애물 파괴 (Destroy 보다는 풀링으로 구현할 것)
            Dead(); // 소멸
        }
    }




    public void Set_Status(int gun_type) // 총의 종류에 맞게 총알의 스탯 설정.
    {
        m_Damage = Guns.GetInstance().Get_Gun_Damage();
        m_Move_Speed = Guns.GetInstance().Get_Gun_Bullet_Speed();
        m_Knock_Back_Distance = Guns.GetInstance().Get_Gun_Knock_Back_Distance();

        Vector3 size = GetComponent<BoxCollider>().size;
        transform.localPosition = Vector3.zero;
        switch (gun_type)
        {
            case GUN_TYPE.SHOTGUN:
                size.x = BULLET_SIZE.SHOTGUN_X;
                size.y = BULLET_SIZE.SHOTGUN_Y;
                GetComponent<BoxCollider>().size = size;

                Set_Random_Direction(gun_type);
                break;

            case GUN_TYPE.DSMG:
                size.x = BULLET_SIZE.DSMG_X;
                size.y = BULLET_SIZE.DSMG_Y;
                GetComponent<BoxCollider>().size = size;

                Set_Random_Direction(gun_type);
                break;
        }
        
        
        
    }
    

    void Set_Random_Direction(int gun_type) // 랜덤방향 지정 메소드. (난사형 총기에 사용)
    {
        Vector3 rot = transform.eulerAngles;

        switch (gun_type)
        {
            case GUN_TYPE.SHOTGUN:
                rot.z = Random.Range(-7.0f, 12.0f);
                break;
            case GUN_TYPE.DSMG:
                rot.z = Random.Range(-7.0f, 7.0f);
                break;
        }

        transform.Rotate(rot);
    }





    IEnumerator Move() // 총알 이동
    {
        while(true)
        {
            if (!StageManager.GetInstance().Get_isPause())
            {
                transform.Translate(new Vector3(-m_Move_Speed * Time.deltaTime, 0.0f, 0.0f)); // 직진!
            }
            yield return null;
        }
    }

    public void Fired()
    {
        StartCoroutine(Move());
    }


    void Dead() // 총알 소멸 (큐로 복귀)
    {
        transform.localPosition = Vector3.zero;
        Quaternion q;
        q.x = q.y = q.z = q.w = 0.0f;

        transform.rotation = q;

        StopAllCoroutines();

        transform.parent.GetComponent<Projectile_Pooling_Manager>().Get_In_The_Pool(gameObject);
    }
}
