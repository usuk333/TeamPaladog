using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //인게임씬에서 접근 용이하도록 스태틱으로 사용
    [SerializeField] private Unit[] units;

    public static InGameManager Instance { get => instance; }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
