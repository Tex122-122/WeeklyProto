using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public int Respawn;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            SceneManager.LoadScene(Respawn);
        }
    }
}

