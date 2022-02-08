using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //인게임씬 클래스들이 접근 용이하도록 스태틱으로 사용
    [SerializeField] private Unit[] units;

    public static InGameManager Instance { get => instance; }
    public Unit[] Units { get => units; }

    //생각해보니 델리게이트로 하는게 더 나을듯?
    private IEnumerator Co_InitializeInGameData() //초기화가 필요한 모든 인게임 데이터 초기화. 완료되면 true 반환 후 로딩 완료.
    {
        yield return null;
    }
    private void Awake()
    {
        instance = this;
        StartCoroutine(Co_InitializeInGameData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
