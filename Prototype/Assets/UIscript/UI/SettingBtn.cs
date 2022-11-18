using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    private GameObject settingpanel;

    private void Awake()
    {
        settingpanel = GameObject.Find("Canvas").transform.Find("SettingPanel").gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void settingbtn()
    {
        settingpanel.SetActive(true);


    }

    public void settingclosebtn()
    {
        settingpanel.SetActive(false);
    }
}
