using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public GameObject ui;
    public GameObject music;
    public GameObject music2;
    public GameObject win;
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.SetActive(false);
            cam.SetActive(true);
            ui.SetActive(false);
            music.SetActive(false);
            music2.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            win.SetActive(true);
            
        }

    }
}
