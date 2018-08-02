using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class GUN_TYPE // 총기 리스트 정의
{
    public const int SHOTGUN = 0;
    public const int DSMG = 1;
    public const int AR = 2;
}

static class GUN_DAMAGE // 총기별 총알 데미지 정의
{
    public const float SHOTGUN = 3.0f;
    public const float DSMG = 2.0f;
    public const float AR = 10.0f;
}

static class GUN_BULLET_SPEED // 총기별 총알 속도 정의
{
    public const float SHOTGUN = 15.0f;
    public const float DSMG = 18.0f;
    public const float AR = 22.0f;
}

static class GUN_ACCURACY_ANGLE // 총기별 총알 발사 각도 정의
{
    public const float SHOTGUN_MAX = 3.0f;
    public const float SHOTGUN_MIN = -20.0f;
    public const float DSMG_MAX = 10.0f;
    public const float DSMG_MIN = -10.0f;
    public const float AR_MAX = 5.0f;
    public const float AR_MIN = -5.0f;
}

static class GUN_BULLET_NUM // 총기별 1회 발사 총알 개수 정의
{
    public const int SHOTGUN = 12;
    public const int DSMG = 2;
    public const int AR = 1;
}

static class GUN_FIRE_NUM_PER_RELOAD // 총기별 1회 장전당 발사 시도 횟수
{
    public const int SHOTGUN = 5;
    public const int DSMG = 40;
    public const int AR = 30;
}

static class GUN_RELOAD_TIME // 총기별 재장전 시간 정의
{
    public const float SHOTGUN = 3.0f;
    public const float DSMG = 3.0f;
    public const float AR = 2.5f;
}

static class GUN_AUTO_FIRE_TIME // 총기별 연사속도(초당 발사 수) 정의
{
    public const float SHOTGUN = 1.0f;
    public const float DSMG = 0.05f;
    public const float AR = 0.08f;
}

static class GUN_KNOCK_BACK_DISTANCE // 총기별 넉백 거리 정의
{
    public const float SHOTGUN = 1.0f;
    public const float DSMG = 0.5f;
    public const float AR = 2.0f;
}




public class Guns : MonoBehaviour {

    static Guns m_Instance;

    Animation m_Gun_Animations;

    public GameObject m_Pool_Manager;


    int m_Curr_Gun_Type; // 현재 켜진 총의 종류.

    float m_Curr_Gun_Damage = 0.0f; // 현재 켜진 총의 데미지.
    float m_Curr_Gun_Bullet_Speed = 0.0f; // 현재 켜진 총의 총알 속도.

    float m_Curr_Gun_Reload_Time = 0.0f; // 현재 켜진 총의 재장전 시간.
    float m_Waited_Reload_Time = 0.0f; // 재장전을 기다린 시간.

    float m_Curr_Gun_Knock_Back_Distance = 0.0f; // 현재 켜진 총의 넉백 거리.

    int m_Curr_Gun_Bullet_Num_Per_One_Shot = 0; // 현재 켜진 총의 1회 발사 총알 개수.

    int m_Curr_Gun_Fire_Per_Reload = 0; // 현재 켜진 총의 1회 장전당 발사 시도 횟수
    int m_Left_Rounds_In_Magazine = 0; // 남은 발사 횟수.

    float m_Curr_Gun_Auto_Fire_Time = 0.0f; // 현재 켜진 총의 연사 속도.
    float m_Waited_Auto_Fire_Time = 0.0f; // 연사를 기다린 시간.

    

    public Sprite m_ShotGun_Sprite;
    public Sprite m_ShotGun_Fire_Sprite;
    public Sprite m_DSMG_Sprite;
    public Sprite m_DSMG_Fire_Sprite;
    public Sprite m_AR_Sprite;
    public Sprite m_AR_Fire_Sprite;

    void Awake ()
    {
        m_Instance = this;

        m_Gun_Animations = GetComponent<Animation>();

        Gun_Change(GUN_TYPE.SHOTGUN);

        m_Waited_Reload_Time = 0.0f;
    }
    

    // ===================================
    public static Guns GetInstance()
    {
        return m_Instance;
    }

    public Animation Get_Animations()
    {
        return m_Gun_Animations;
    }

    public int Get_Gun_Type()
    {
        return m_Curr_Gun_Type;
    }
    public float Get_Gun_Damage()
    {
        return m_Curr_Gun_Damage;
    }
    public float Get_Gun_Bullet_Speed()
    {
        return m_Curr_Gun_Bullet_Speed;
    }
    public float Get_Gun_Knock_Back_Distance()
    {
        return m_Curr_Gun_Knock_Back_Distance;
    }

    public float Get_Gun_Reload_Time()
    {
        return m_Curr_Gun_Reload_Time;
    }
    public float Get_Waited_Reload_Time()
    {
        return m_Waited_Reload_Time;
    }
    public float Get_Gun_Bullet_Num_Per_One_Shot()
    {
        return m_Curr_Gun_Bullet_Num_Per_One_Shot;
    }
    public float Get_Gun_Fire_Per_Reload()
    {
        return m_Curr_Gun_Fire_Per_Reload;
    }
    public float Get_Gun_Left_Rounds_In_Magazine()
    {
        return m_Left_Rounds_In_Magazine;
    }
    
    // ===================================


    public void Gun_Change(int gun_type) // 총 변경 처리.
    {
        m_Curr_Gun_Type = gun_type; // 현재 총기 타입 변경.

        switch (gun_type)
        {
            case GUN_TYPE.SHOTGUN:
                m_Curr_Gun_Reload_Time = GUN_RELOAD_TIME.SHOTGUN; // 재장전 시간 변경.

                m_Curr_Gun_Damage = GUN_DAMAGE.SHOTGUN; // 데미지 변경.
                m_Curr_Gun_Bullet_Speed = GUN_BULLET_SPEED.SHOTGUN; // 총알 속도 변경.
                m_Curr_Gun_Knock_Back_Distance = GUN_KNOCK_BACK_DISTANCE.SHOTGUN; // 넉백 거리 변경.

                m_Curr_Gun_Bullet_Num_Per_One_Shot = GUN_BULLET_NUM.SHOTGUN; // 1회 발사 총알 개수 변경.
                m_Curr_Gun_Fire_Per_Reload = GUN_FIRE_NUM_PER_RELOAD.SHOTGUN; // 1회 장전당 발사 횟수 변경.

                m_Curr_Gun_Auto_Fire_Time = GUN_AUTO_FIRE_TIME.SHOTGUN; // 연사수 (시간) 변경.

                GetComponent<SpriteRenderer>().sprite = m_ShotGun_Sprite; // 새 총기 스프라이트로 변경.
                GameObject.Find("Gun_Fire").GetComponent<SpriteRenderer>().sprite = m_ShotGun_Fire_Sprite; // 새 총기 불꽃 스프라이트로 변경.
                
                break;

            case GUN_TYPE.DSMG:
                m_Curr_Gun_Reload_Time = GUN_RELOAD_TIME.DSMG;

                m_Curr_Gun_Damage = GUN_DAMAGE.DSMG;
                m_Curr_Gun_Bullet_Speed = GUN_BULLET_SPEED.DSMG;
                m_Curr_Gun_Knock_Back_Distance = GUN_KNOCK_BACK_DISTANCE.DSMG;

                m_Curr_Gun_Bullet_Num_Per_One_Shot = GUN_BULLET_NUM.DSMG;
                m_Curr_Gun_Fire_Per_Reload = GUN_FIRE_NUM_PER_RELOAD.DSMG;

                m_Curr_Gun_Auto_Fire_Time = GUN_AUTO_FIRE_TIME.DSMG;

                GetComponent<SpriteRenderer>().sprite = m_DSMG_Sprite;
                GameObject.Find("Gun_Fire").GetComponent<SpriteRenderer>().sprite = m_DSMG_Fire_Sprite;

                break;

            case GUN_TYPE.AR:
                m_Curr_Gun_Reload_Time = GUN_RELOAD_TIME.AR;

                m_Curr_Gun_Damage = GUN_DAMAGE.AR;
                m_Curr_Gun_Bullet_Speed = GUN_BULLET_SPEED.AR;
                m_Curr_Gun_Knock_Back_Distance = GUN_KNOCK_BACK_DISTANCE.AR;

                m_Curr_Gun_Bullet_Num_Per_One_Shot = GUN_BULLET_NUM.AR;
                m_Curr_Gun_Fire_Per_Reload = GUN_FIRE_NUM_PER_RELOAD.AR;

                m_Curr_Gun_Auto_Fire_Time = GUN_AUTO_FIRE_TIME.AR;

                GetComponent<SpriteRenderer>().sprite = m_AR_Sprite;
                GameObject.Find("Gun_Fire").GetComponent<SpriteRenderer>().sprite = m_AR_Fire_Sprite;

                break;
        }
        m_Waited_Reload_Time = 0.0f; // 재장전 대기 시간 초기화.
        m_Waited_Auto_Fire_Time = m_Curr_Gun_Auto_Fire_Time; // 연사 시간 Full.
        m_Left_Rounds_In_Magazine = m_Curr_Gun_Fire_Per_Reload; // 남은 발사 횟수 Full.
    }


    public void GunFire() // 발사
    {
        if (m_Left_Rounds_In_Magazine > 0) // 탄창 안에 총알이 있으면
        {
            if (Auto_Fire_Ready()) // 그리고 연사 준비가 됐으면
            {
                for (int i = 0; i < m_Curr_Gun_Bullet_Num_Per_One_Shot; ++i) // 1회 발사당 총알 개수 만큼 수행.
                    m_Pool_Manager.GetComponent<Projectile_Pooling_Manager>().Projectile_Fired(m_Curr_Gun_Type);

                --m_Left_Rounds_In_Magazine; // 발사 횟수 감소.
                m_Waited_Auto_Fire_Time = 0.0f; // 발사 후 연사 시간 초기화.

                m_Gun_Animations.Play(m_Gun_Animations.GetClip("GunFire").name); // 발사 애니메이션 수행.
            }
        }

        else Reload(); // 총알이 없으면 재장전 수행.
    }

    void Reload() // 재장전
    {
        if (m_Waited_Reload_Time >= m_Curr_Gun_Reload_Time) // 장전 시간이 다 됐으면
        {
            m_Left_Rounds_In_Magazine = m_Curr_Gun_Fire_Per_Reload; // 탄창을 채운다.
            m_Waited_Auto_Fire_Time = m_Curr_Gun_Auto_Fire_Time; // 연사도 준비 한다.
            m_Waited_Reload_Time = 0.0f;
        }

        else // 아니면
        {
            m_Waited_Reload_Time += Time.deltaTime; // 장전 대기
        }
    }

    bool Auto_Fire_Ready() // 연사
    {
        if (m_Waited_Auto_Fire_Time >= m_Curr_Gun_Auto_Fire_Time) // 연사 준비가 됐으면
            return true;// 완료 보고.

        else // 아니면
        {
            m_Waited_Auto_Fire_Time += Time.deltaTime; // 연사시간을 기다리고
            return false; // 미완료 보고.
        }
    }
}
