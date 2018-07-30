using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 큐를 통해 오브젝트 풀링을 한다.
// 총을 발사하면, 발사한 총알 개수만큼 큐에서 팝한다.

public class Projectile_Pooling_Manager : MonoBehaviour {

    static Projectile_Pooling_Manager m_Instance;
    public Sprite m_ShotGun_Bullet;
    public Sprite m_ShotGun_Bullet_Fire;
    public Sprite m_DSMG_Bullet;
    public Sprite m_DSMG_Bullet_Fire;
    public GameObject m_Player;

    Queue<GameObject> m_Projectiles_Pool;

    void Awake ()
    {
        // 큐를 만들어 총알들을 등록
        m_Projectiles_Pool = new Queue<GameObject>();
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            m_Projectiles_Pool.Enqueue(go);
        }
    }

    public static Projectile_Pooling_Manager GetInstance()
    {
        return m_Instance;
    }

    public void Get_In_The_Pool(GameObject projectile)
    {
        m_Projectiles_Pool.Enqueue(projectile);
    }

    public void Projectile_Fired(int type)
    {
        GameObject p = m_Projectiles_Pool.Dequeue();
        if (p != null)
        {
            p.GetComponent<Projectiles>().Set_Status(type);
            p.transform.position = m_Player.transform.position;
            Vector3 lpos = p.transform.localPosition;

            switch (type)
            {
                case GUN_TYPE.SHOTGUN:
                    lpos.x = BULLET_FIRST_POSITION.SHOTGUN_X;
                    lpos.y = BULLET_FIRST_POSITION.SHOTGUN_Y;
                    p.GetComponent<SpriteRenderer>().sprite = m_ShotGun_Bullet;
                    p.GetComponentInChildren<SpriteRenderer>().sprite = m_ShotGun_Bullet_Fire;
                    break;

                case GUN_TYPE.DSMG:
                    p.GetComponent<SpriteRenderer>().sprite = m_DSMG_Bullet;
                    p.GetComponentsInChildren<SpriteRenderer>()[0].sprite = m_DSMG_Bullet_Fire;
                    break;
            }

            p.transform.localPosition = lpos;
            p.GetComponent<Projectiles>().Fired();
        }
    }
}
