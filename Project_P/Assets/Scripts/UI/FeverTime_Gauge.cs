using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverTime_Gauge : MonoBehaviour {
    Animation m_Animations;
    IEnumerator m_Fever_Checker;

    void Start ()
    {
        m_Animations = GetComponent<Animation>();
        StartCoroutine(Fever_Gauge_Check());
    }

    IEnumerator Fever_Gauge_Check()
    {
        while(true)
        {
            m_Animations.Play(m_Animations.GetClip("FeverTime_Gauge_Rainbow").name);

            if (StageManager.GetInstance().Get_is_FeverTime_On())
                GetComponent<Slider>().value = StageManager.GetInstance().Get_Elapsed_FeverTime() / StageManager.GetInstance().Get_FeverTime_Duration();
            else
                GetComponent<Slider>().value = StageManager.GetInstance().Get_Curr_FeverTime_Point() / StageManager.GetInstance().Get_FeverTime_Max_Point();
            
            yield return null;
        }
    }
}
