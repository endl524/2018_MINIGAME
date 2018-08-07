using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class FADE_ANI_NAME
{
    public const string FADE_1_IN = "Fade_1_In";
    public const string FADE_1_OUT = "Fade_1_Out";
}

public class Fade_Manager : MonoBehaviour {

    Animation m_Animations;

    void Start()
    {
        m_Animations = GetComponent<Animation>();
    }

    public void Fade_In()
    {
        m_Animations.Play(m_Animations.GetClip(FADE_ANI_NAME.FADE_1_IN).name);
    }

    public void Fade_Out()
    {
        m_Animations.Play(m_Animations.GetClip(FADE_ANI_NAME.FADE_1_OUT).name);
    }


}
