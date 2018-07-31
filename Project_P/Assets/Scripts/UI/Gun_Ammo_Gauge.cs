using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun_Ammo_Gauge : MonoBehaviour
{
    IEnumerator m_Ammo_Checker;
    bool is_Reloading = false;

	void Start ()
    {
        StartCoroutine(Ammo_Check());
	}

    IEnumerator Ammo_Check()
    {
        while(true)
        {
            if (is_Reloading)
            {
                GetComponent<Slider>().value = Guns.GetInstance().Get_Waited_Reload_Time() / Guns.GetInstance().Get_Gun_Reload_Time();
                GetComponentInChildren<Text>().text = "Reloading!";
                if (Guns.GetInstance().Get_Gun_Left_Rounds_In_Magazine() >= Guns.GetInstance().Get_Gun_Fire_Per_Reload()) is_Reloading = false;
            }
            else
            {
                GetComponent<Slider>().value = Guns.GetInstance().Get_Gun_Left_Rounds_In_Magazine() / Guns.GetInstance().Get_Gun_Fire_Per_Reload();
                GetComponentInChildren<Text>().text = "AMMO : " + Guns.GetInstance().Get_Gun_Left_Rounds_In_Magazine().ToString() + " / " + Guns.GetInstance().Get_Gun_Fire_Per_Reload().ToString();
                if (Guns.GetInstance().Get_Gun_Left_Rounds_In_Magazine() <= 0) is_Reloading = true;
            }
            yield return null;
        }
    }
}
