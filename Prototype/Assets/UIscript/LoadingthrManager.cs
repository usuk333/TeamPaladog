using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingthrManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Go", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Go()
    {
        SceneManager.LoadScene("StartScene");
    }
}
