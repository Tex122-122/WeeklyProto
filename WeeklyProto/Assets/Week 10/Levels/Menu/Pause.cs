using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseCamera;
    public GameObject fpc;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            pauseCamera.SetActive(true);
            fpc.SetActive(false);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

       
        
    }


    public void PauseTime()
    {


        Time.timeScale = 0;

    }

    public void Unpause()
    {
        
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseCamera.SetActive(false);
        fpc.SetActive(true);
    }
}
