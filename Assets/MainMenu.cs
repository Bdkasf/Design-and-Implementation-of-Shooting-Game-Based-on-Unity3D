using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    string newGameScene = "SampleScene";
    string newGameScene1 = "Demo_Scene_1";
    string newGameScene2 = "Demo_Scene_2";
    // Start is called before the first frame update
    void Start()
    {
    //显示鼠标指针
    Cursor.visible = true;
    //解除鼠标锁定
    Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void StartNewGame1()
    {
        SceneManager.LoadScene(newGameScene1);
    }

        public void StartNewGame2()
    {
        SceneManager.LoadScene(newGameScene2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
