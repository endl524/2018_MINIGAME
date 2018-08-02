using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 큐를 통해 오브젝트 풀링을 한다.
// 총을 발사하면, 발사한 총알 개수만큼 큐에서 팝한다.

public class Projectile_Pooling_Manager : MonoBehaviour {
    
    public Sprite m_ShotGun_Bullet;
    public Sprite m_ShotGun_Bullet_Fire;
    public Sprite m_DSMG_Bullet;
    public Sprite m_AR_Bullet;
    public Sprite m_AR_Bullet_Fire;
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
            Vector3 lpos = m_Player.transform.position;

            switch (type)
            {
                case GUN_TYPE.SHOTGUN:
                    lpos.x += BULLET_FIRST_POSITION.SHOTGUN_X;
                    lpos.y += BULLET_FIRST_POSITION.SHOTGUN_Y;
                    p.GetComponent<SpriteRenderer>().sprite = m_ShotGun_Bullet;
                    p.GetComponentsInChildren<SpriteRenderer>()[1].sprite = m_ShotGun_Bullet_Fire;
                    break;

                case GUN_TYPE.DSMG:
                    lpos.x += BULLET_FIRST_POSITION.DSMG_X;
                    lpos.y += BULLET_FIRST_POSITION.DSMG_Y;
                    p.GetComponent<SpriteRenderer>().sprite = m_DSMG_Bullet;
                    p.GetComponentsInChildren<SpriteRenderer>()[1].sprite = null;
                    break;

                case GUN_TYPE.AR:
                    lpos.x += BULLET_FIRST_POSITION.AR_X;
                    lpos.y += BULLET_FIRST_POSITION.AR_Y;
                    p.GetComponent<SpriteRenderer>().sprite = m_AR_Bullet;
                    p.GetComponentsInChildren<SpriteRenderer>()[1].sprite = m_AR_Bullet_Fire;
                    break;
            }

            p.transform.position = lpos;
            p.GetComponent<Projectiles>().Fired();
        }
    }
}
