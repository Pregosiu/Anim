using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{


    void Start()
    {

    }


    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.PLAYER))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
