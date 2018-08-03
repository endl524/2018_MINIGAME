using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle_Enemy_1_Detector : MonoBehaviour {

    bool m_is_Detected = false;
    GameObject m_Target;

    void OnTriggerEnter(Collider other)
    {
        if (!m_is_Detected && other.CompareTag("Player"))
        {
            m_is_Detected = true;
            m_Target = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (m_is_Detected && other.CompareTag("Player"))
        {
            m_is_Detected = false;
            m_Target = null;
        }
    }

    public bool Get_is_Detected()
    {
        return m_is_Detected;
    }

    public GameObject Get_Target()
    {
        return m_Target;
    }
}
