using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static int scoreValue = 0;
    public float killsTillNextRound = 1;
    public float win = 3;
    public GameObject winn;
    public GameObject ui;
    public GameObject music;
    public GameObject deathCam;
    public GameObject player;


    private void Update()
    {

        if (scoreValue >= killsTillNextRound)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            reset();
        }
        if (scoreValue >= win)
        {
            winn.SetActive(true);
            reset();
            player.SetActive(false);
            deathCam.SetActive(true);
            ui.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            music.SetActive(false);
        }
    }


    public void EnterRing()
    {
        scoreValue += 1;

    }


    public void reset()
    {
        killsTillNextRound = 0;
        scoreValue = 0;
    }
}
