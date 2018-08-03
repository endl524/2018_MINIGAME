using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Board_UI : MonoBehaviour {
    
    static Score_Board_UI m_Instance;

    public Text m_Num_of_Obstacle_Text;
    public Text m_Num_of_Small_Kill_Text;
    public Text m_Num_of_Middle_Kill_Text;
    public Text m_Num_of_Big_Kill_Text;
    public Text m_Num_of_Item_Text;
    public Text m_Level_Text;

    void Awake()
    {
        m_Instance = this;
    }

    public static Score_Board_UI GetInstance()
    {
        return m_Instance;
    }

    public void Set_Obstacle_Num_Text(int num)
    {
        m_Num_of_Obstacle_Text.text = num.ToString() + " 개";
    }

    public void Set_Item_Num_Text(int num)
    {
        m_Num_of_Item_Text.text = num.ToString() + " 개";
    }

    public void Set_Small_Kill_Num_Text(int num)
    {
        m_Num_of_Small_Kill_Text.text = "소형 : " + num.ToString() + " 마리";
    }

    public void Set_Middle_Kill_Num_Text(int num)
    {
        m_Num_of_Middle_Kill_Text.text = "중형 : " + num.ToString() + " 마리";
    }

    public void Set_Big_Kill_Num_Text(int num)
    {
        m_Num_of_Big_Kill_Text.text = "대형 : " + num.ToString() + " 마리";
    }

    public void Set_Level_Text(int num)
    {
        m_Level_Text.text = num.ToString();
    }
}
