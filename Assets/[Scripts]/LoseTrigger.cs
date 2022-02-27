using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseTrigger : MonoBehaviour
{
    //Lose Condition
    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.tag == "BackWall") || (other.gameObject.tag == "FrontWall"))
        {
            Debug.Log("Contact" + other.gameObject.tag);
            //On Player Lose
            SceneManager.LoadScene("LoseScene");
        }
    }

}
