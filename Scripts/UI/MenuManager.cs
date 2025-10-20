using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        //PlayerPrefs.SetString("RestartSource", "Menu");
        //SceneManager.LoadScene(1);
        SceneFader.Instance.FadeToScene("Park");
    }

}

