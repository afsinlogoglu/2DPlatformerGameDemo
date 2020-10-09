using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    GameObject level1;
    void Start()
    {
        level1 = GameObject.Find("Level1Button");
        level1.SetActive(false);
        
    }

    public void selectButton(int selectedButton)
    {

        if (selectedButton == 1)
        {
            SceneManager.LoadScene("Level 2");
        }
        else if (selectedButton == 2)
        {
            level1.SetActive(true);
        }
        else if (selectedButton == 3)
        {
            Application.Quit();
        }
    }
    
}
