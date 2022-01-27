using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSkillData : MonoBehaviour
{
    public static StageSkillData Instance;
    private int firstSkillIndex;
    private int secondSkillIndex;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
