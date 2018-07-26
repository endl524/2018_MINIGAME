using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

static class SCENE_NAMES
{
    public const int LOBBY_SCENE = 0;
    public const int MAIN_STAGE_SCENE = 1;
}

public class SceneSwap : MonoBehaviour {

    static SceneSwap m_Instance;

    void Start()
    {
        m_Instance = this;
    }

    public void Scene_Swap(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public static SceneSwap GetInstance()
    {
        return m_Instance;
    }
}
