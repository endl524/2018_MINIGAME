using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class GUN_TYPE // 총기 리스트 정의
{
    public const int SHOTGUN = 0;
}

static class GUN_DAMAGE // 총기별 총알 데미지 정의
{
    public const float SHOTGUN = 10.0f;
}

static class GUN_BULLET_SPEED // 총기별 총알 속도 정의
{
    public const float SHOTGUN = 10.0f;
}

static class GUN_RELOAD_TIME // 총기별 재장전 시간 정의
{
    public const float SHOTGUN = 1.6f;
}

static class GUN_KNOCK_BACK_DISTANCE // 총기별 넉백 거리 정의
{
    public const float SHOTGUN = 3.0f;
}

static class GUN_ANIMATION_NAME // 총기별 애니메이션 이름 정의
{
    public const string SHOTGUN = "";
}




public class Guns : MonoBehaviour {

    static Guns m_Instance;

    Animator m_Gun_Animator;

    string m_Curr_Gun_Ani_Name = ""; // 현재 켜놓은 애니메이션의 이름.
    float m_Curr_Gun_Damage = 0.0f; // 현재 켜진 총의 데미지.
    float m_Curr_Gun_Bullet_Speed = 0.0f; // 현재 켜진 총의 총알 속도.
    float m_Curr_Gun_Reload_Time = 0.0f; // 현재 켜진 총의 재장전 시간.
    float m_Curr_Gun_Knock_Back_Distance = 0.0f; // 현재 켜진 총의 넉백 거리.

    int m_Curr_Gun_Type;

    public Sprite m_ShotGun_Sprite;
    public Sprite m_ShotGun_Fire_Sprite;

    List<Collider> Damaged_Enemy_List;

    void Awake ()
    {
        m_Instance = this;

        m_Gun_Animator = GetComponent<Animator>();

        Gun_Change(GUN_TYPE.SHOTGUN);
    }
    

    // ===================================
    public static Guns GetInstance()
    {
        return m_Instance;
    }

    public Animator Get_Animator()
    {
        return m_Gun_Animator;
    }

    public int Get_Gun_Type()
    {
        return m_Curr_Gun_Type;
    }
    public string Get_Animation_Name()
    {
        return m_Curr_Gun_Ani_Name;
    }
    public float Get_Gun_Reload_Time()
    {
        return m_Curr_Gun_Reload_Time;
    }
    // ===================================

        
    void Gun_Change(int gun_type) // 총 변경 처리.
    {
        m_Curr_Gun_Type = gun_type;

        switch (gun_type)
        {
            case GUN_TYPE.SHOTGUN:
                m_Curr_Gun_Reload_Time = GUN_RELOAD_TIME.SHOTGUN; // 재장전 시간 변경.
                m_Curr_Gun_Ani_Name = GUN_ANIMATION_NAME.SHOTGUN; // 새 Ani 이름 설정.

                m_Curr_Gun_Damage = GUN_DAMAGE.SHOTGUN;
                m_Curr_Gun_Bullet_Speed = GUN_BULLET_SPEED.SHOTGUN;
                m_Curr_Gun_Knock_Back_Distance = GUN_KNOCK_BACK_DISTANCE.SHOTGUN;

                GetComponent<SpriteRenderer>().sprite = m_ShotGun_Sprite; // 새 총기 스프라이트로 변경.
                GameObject.Find("Gun_Fire").GetComponent<SpriteRenderer>().sprite = m_ShotGun_Fire_Sprite; // 새 총기 불꽃 스프라이트로 변경.

                // 위치 변경도 해야하나? 규격을 맞출것인가?
                break;

        }
    }


    public void GunFire() // Player의 Attack에서 수행.
    {
        switch(m_Curr_Gun_Type)
        {
            case GUN_TYPE.SHOTGUN:
                // 1. 총알 풀에서 원하는 개수 만큼 꺼낸다.
                // 2. 꺼낼 때 마다 총알의 스탯을 설정한다.
                // 3. 스탯 설정이 끝나면 코루틴을 진행시킨다. (이동)
                break;
        }
    }
}
