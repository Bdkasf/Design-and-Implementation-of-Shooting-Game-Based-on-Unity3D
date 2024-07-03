using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    string newGameScene = "MainMenu";
    private KeyCode BackToMain = KeyCode.P;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BackMainMenu();
    }

    public void BackMainMenu()
    {
        if (Input.GetKeyDown(BackToMain))
        {
            SceneManager.LoadScene(newGameScene);
            Debug.Log("按下了P键");
        }
    }
}
