using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public struct Object_Pattern_Structure
{
    public int Pattern_Number;
    public int[] Line_Number;
    public int[] Object_Number;
    public int[] X_Offset_Distance; // 로컬 오프셋이다!

    public int Count_of_Objects;
}

public class CSV_Manager : MonoBehaviour {


    private static CSV_Manager m_Instance = null;

    public static CSV_Manager GetInstance()
    {
        return m_Instance;
    }



    public TextAsset m_Enemy_Pattern_CSV;
    public TextAsset m_Obstacle_Pattern_CSV;
    //public TextAsset m_Item_Pattern_CSV;

    protected string[] m_data;
    protected string[] m_stringList;



    void Awake()
    {
        m_Instance = this;
    }
    
    int Counting_EOF(TextAsset csv) // CSV 파일의 행 개수를 세어주는 함수
    {
        StringReader m_stringReader = new StringReader(csv.text);
        string m_string_Line = m_stringReader.ReadLine();
        int count = 0;

        while (m_string_Line != null)
        {
            m_string_Line = m_stringReader.ReadLine();
            ++count;
        }
        return count;
    }


    // =================================================================


    // "각종 오브젝트 패턴 리스트"를 넘겨주는 메소드.
    public void Get_Object_Pattern_List(ref List<Object_Pattern_Structure> list, int Max_object_num_in_one_pattern, int object_type)
    {
        Object_Pattern_Structure pattern_Structure = new Object_Pattern_Structure(); // 구조체를 할당한다.
        int file_Line_Count = 0;

        switch (object_type)  // 여기서 csv 파일 내부의 행 수를 세어두고, 개행 단위로 잘라놓는다.
        {
            case OBJECT_TYPE.ENEMY:
                file_Line_Count = Counting_EOF(m_Enemy_Pattern_CSV);
                m_stringList = m_Enemy_Pattern_CSV.text.Split('\n');
                break;

            case OBJECT_TYPE.OBSTACLE:
                file_Line_Count = Counting_EOF(m_Obstacle_Pattern_CSV);
                m_stringList = m_Obstacle_Pattern_CSV.text.Split('\n');
                break;

            //case OBJECT_TYPE.ITEM:
                //file_Line_Count = Counting_EOF(m_Item_Pattern_CSV);
                //m_stringList = m_Item_Pattern_CSV.text.Split('\n');
                //break;
        }

        list.Clear(); // 리스트를 비워준다.

        int pattern_num = 1;
        int list_Index = 0;
        
        m_data = m_stringList[2].Split(','); // 한줄을 읽는다.

        for (int i = 2; i < file_Line_Count; ) // 2 부터 시작
        {
            pattern_Structure.Line_Number = new int[Max_object_num_in_one_pattern]; // 임시 구조체 내부 배열 할당_1
            pattern_Structure.Object_Number = new int[Max_object_num_in_one_pattern]; // 임시 구조체 내부 배열 할당_2
            pattern_Structure.X_Offset_Distance = new int[Max_object_num_in_one_pattern]; // 임시 구조체 내부 배열 할당_3

            pattern_Structure.Pattern_Number = System.Convert.ToInt32(m_data[0]); // (1) 임시 구조체에 "패턴 번호" 저장.

            while (pattern_Structure.Pattern_Number == System.Convert.ToInt32(m_data[0])) // 동일한 패턴 번호인지 확인한다.
            {
                pattern_Structure.Line_Number[list_Index] = System.Convert.ToInt32(m_data[1]); // (2) 임시 구조체에 "라인 번호" 저장
                pattern_Structure.Object_Number[list_Index] = System.Convert.ToInt32(m_data[2]); // (3) 임시 구조체에 "객체 번호" 저장
                pattern_Structure.X_Offset_Distance[list_Index] = System.Convert.ToInt32(m_data[3]); // (4) 임시 구조체에 "x축 오프셋" 저장
                
                ++list_Index; // 인덱스를 1 증가시킨다.
                ++i; // 1줄 증가시킨다.

                if (i < file_Line_Count) // 마지막 줄인지를 검사한다.
                    m_data = m_stringList[i].Split(','); // 마지막줄이 아니라면 그 1줄을 읽어놓는다.
                else break; // 마지막 줄이라면 탈출.
            }

            pattern_Structure.Count_of_Objects = list_Index - 1; // 패턴 내의 객체 수를 기록.

            list_Index = 0; // 인덱스 초기화
            
            ++pattern_num; // 다음 패턴 번호로 넘어간다.
            
            list.Add(pattern_Structure); // 리스트에 기록한다.
        }
    }
}
