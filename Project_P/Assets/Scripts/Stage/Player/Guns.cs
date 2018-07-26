using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class GUN_LIST // 총기 리스트 정의
{
    public const int RIFLE = 0;
}

static class GUN_DAMAGE // 총기 데미지 정의
{
    public const float RIFLE = 10.0f;
}

static class GUN_RELOAD_TIME // 총기별 재장전 시간 정의
{
    public const float RIFLE = 1.6f;
}

static class GUN_KNOCK_BACK_DISTANCE // 총기별 넉백 거리 정의
{
    public const float RIFLE = 3.0f;
}

static class GUN_FIRE_RANGE // 총기 발사 범위 (충돌박스 크기) 정의
{
    public const float RIFLE_size_x = 12.0f;
    public const float RIFLE_size_y = 2.5f;
    public const float RIFLE_center_x = -7.0f;
    public const float RIFLE_center_y = 0.0f;
}

static class GUN_ANIMATION_NAME // 총기별 애니메이션 이름 정의
{
    public const string RIFLE = "is_Curr_Gun_Rifle";
}

static class GUN_ANI_N_TIMES_NAME // 총기별 애니메이션 노말타임 이름 정의
{
    public const string RIFLE = "Rifle_NormalizedTime";
}




public class Guns : MonoBehaviour {

    static Guns m_Instance;

    Animator m_Gun_Animator;
    BoxCollider m_Gun_Fire_Range_Collider;

    string m_Curr_Gun_Ani_Name = ""; // 현재 켜놓은 애니메이션의 이름.
    string m_Curr_Gun_Ani_N_Times_Name = ""; // 현재 켜진 애니메이션의 노말타임.
    float m_Curr_Gun_Damage = 0.0f; // 현재 켜진 총의 데미지.
    float m_Curr_Gun_Reload_Time = 0.0f; // 현재 켜진 총의 재장전 시간.
    float m_Curr_Gun_Knock_Back_Distance = 0.0f; // 현재 켜진 총의 넉백 거리.

    public Sprite m_Rifle_Sprite;
    public Sprite m_Rifle_Fire_Sprite;

    List<Collider> Damaged_Enemy_List;

    void Awake ()
    {
        m_Instance = this;

        m_Gun_Animator = GetComponent<Animator>();
        m_Gun_Fire_Range_Collider = GetComponent<BoxCollider>();

        Gun_Change(GUN_LIST.RIFLE);
        Damaged_Enemy_List = new List<Collider>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            bool is_damaged = false;

            foreach(Collider enemy in Damaged_Enemy_List)
            {
                if (enemy == other) // 충돌한 적이 이미 데미지를 입었다면
                {
                    is_damaged = true;
                    break; // 탈출
                }
            }

            if (!is_damaged) // 리스트에 없는 적은
            {
                other.gameObject.GetComponent<Enemy>().Hurt(m_Curr_Gun_Damage, m_Curr_Gun_Knock_Back_Distance); // 데미지를 주고
                Damaged_Enemy_List.Add(other); // 리스트에 추가한다.
            }
        }
    }


    public void Fire_End() // 사격 종료
    {
        m_Gun_Fire_Range_Collider.enabled = false;
        Damaged_Enemy_List.Clear();
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

    public string Get_Animation_Name()
    {
        return m_Curr_Gun_Ani_N_Times_Name;
    }
    public string Get_Ani_N_Times_Name()
    {
        return m_Curr_Gun_Ani_N_Times_Name;
    }
    public float Get_Gun_Reload_Time()
    {
        return m_Curr_Gun_Reload_Time;
    }
    // ===================================



    public void Gun_Fire_Range_Activate() // 총 발사 범위 충돌박스 활성화
    {
        m_Gun_Fire_Range_Collider.enabled = true;
    }

    void Gun_Change(int gun_num) // 총 변경 처리.
    {
        string ani_name = "";
        string n_time_name = "";

        switch (gun_num)
        {
            case GUN_LIST.RIFLE:
                m_Curr_Gun_Damage = GUN_DAMAGE.RIFLE; // 데미지 변경
                m_Curr_Gun_Reload_Time = GUN_RELOAD_TIME.RIFLE; // 재장전 시간 변경.
                m_Curr_Gun_Knock_Back_Distance = GUN_KNOCK_BACK_DISTANCE.RIFLE; // 넉백 거리 변경.

                Vector3 tmpV3;
                tmpV3.x = GUN_FIRE_RANGE.RIFLE_size_x;
                tmpV3.y = GUN_FIRE_RANGE.RIFLE_size_y;
                tmpV3.z = 0.5f;

                m_Gun_Fire_Range_Collider.size = tmpV3; // 범위 사이즈 변경.

                tmpV3.x = GUN_FIRE_RANGE.RIFLE_center_x;
                tmpV3.y = GUN_FIRE_RANGE.RIFLE_center_y;
                tmpV3.z = 0.0f;

                m_Gun_Fire_Range_Collider.center = tmpV3; // 범위 위치(센터) 변경.


                ani_name = GUN_ANIMATION_NAME.RIFLE; // 새 Ani 이름 설정.
                n_time_name = GUN_ANI_N_TIMES_NAME.RIFLE; // 새 Ani 노말타임 이름 설정.

                GetComponent<SpriteRenderer>().sprite = m_Rifle_Sprite; // 새 총기 스프라이트로 변경.
                GameObject.Find("Gun_Fire").GetComponent<SpriteRenderer>().sprite = m_Rifle_Fire_Sprite; // 새 총기 불꽃 스프라이트로 변경.

                // 위치 변경도 해야하나? 규격을 맞출것인가?
                break;

        }

        m_Gun_Animator.SetBool(m_Curr_Gun_Ani_Name, false); // 현재 애니메이션 Off.
        m_Gun_Animator.SetFloat(m_Curr_Gun_Ani_N_Times_Name, 0.0f); // 애니메이션 노말타임 초기화.

        m_Gun_Animator.SetBool(ani_name, true); // 새로운 애니메이션 On.

        m_Curr_Gun_Ani_Name = ani_name; // 현재 켜진 애니메이션 이름 기억.
        m_Curr_Gun_Ani_N_Times_Name = n_time_name; // 현재 켜진 노말타임 기억.
    }
}
