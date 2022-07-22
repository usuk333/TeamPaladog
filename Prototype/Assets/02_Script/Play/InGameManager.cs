using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //인게임씬 클래스들이 접근 용이하도록 스태틱으로 사용

    private float timerSecond;
    private int timerMinute;
    [SerializeField] private Text timer;
    [SerializeField] private Unit[] units;
    private Player player;
    [SerializeField] private Boss boss;
    [SerializeField] private SkillData[] skillDataArray;
    public static InGameManager Instance { get => instance; }
    public Unit[] Units { get => units; }
    public Player Player { get => player; }
    public Boss Boss { get => boss; set => boss = value; }
    public SkillData[] SkillDataArray { get => skillDataArray; set => skillDataArray = value; }

    public void StopAllUnitCoroutines()
    {
        foreach (var item in units)
        {
            item.StopAllCoroutines();

        }
    }
    public void StartAllUnitCoroutines()
    {
        foreach (var item in units)
        {
            item.StartAllCoroutines();
        }
    }

    //생각해보니 델리게이트로 하는게 더 나을듯?
    private IEnumerator Co_InitializeInGameData() //초기화가 필요한 모든 인게임 데이터 초기화. 완료되면 true 반환 후 로딩 완료.
    {
        /*player.MaxHp = (float)DataManager.instance.gamePlayData.PlayerData["HP"];
        player.MaxMp = (float)DataManager.instance.gamePlayData.PlayerData["MP"];
        //플레이어 끝나면
        for (int i = 0; i < units.Length; i++)
        {
            units[i].CommonStatus.MaxHp = (float)DataManager.instance.gamePlayData.UnitData[i]["HP"];
        }*/
        yield return null;
    }
    private IEnumerator Co_Timer()
    {
        while (true)
        {
            if ((int)timerSecond > 59)
            {
                timerSecond = 0;
                timerMinute++;
            }
            timerSecond += Time.deltaTime;
            timer.text = string.Format("{0:00}:{1:00}", timerMinute, timerSecond);

            yield return null;
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
       // Instantiate(BossManager.BossSection);
        units = FindObjectsOfType<Unit>();
        Unit unit;
        for (int i = 0; i < units.Length; i++) //이 부분 추후에 함수로 빼기 (유닛리스트 탱커 ~ 원거리 순으로 정렬)
        {
            switch (units[i].UnitType)
            {
                case Unit.EUnitType.Tanker:
                    unit = units[0];
                    units[0] = units[i];
                    units[i] = unit;
                    break;
                case Unit.EUnitType.CloseDealer:
                    unit = units[1];
                    units[1] = units[i];
                    units[i] = unit;
                    break;
                case Unit.EUnitType.Wizard:
                    unit = units[2];
                    units[2] = units[i];
                    units[i] = unit;
                    break;
                case Unit.EUnitType.RemoteDealer:
                    unit = units[3];
                    units[3] = units[i];
                    units[i] = unit;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
        player = FindObjectOfType<Player>();
        StartCoroutine(Co_InitializeInGameData());
        boss = FindObjectOfType<Boss>();
    }
    private void Start()
    {
        StartCoroutine(Co_Timer());
    }
    private void InitSkillData()
    {
        for (int i = 0; i < 4; i++)
        {
            skillDataArray[i] = new SkillData(SkillData.SkillType.Active, 5, 5, 5);
        }
    }
    public void GameOver()
    {
        boss.StopAllCoroutines();
        Debug.Log("Game Over!");
    }
   /* private void ClearStage()
    {
        DataManager.instance.UpdatePlayerData("Gold", 500);
    }*/

    //StageManager <- 리워드 배열
    //보스 체력, 보스 공격력, 패턴 시간
    //BossManager 클래스에 hp, atk, int<List>patternTime 변수 추가
    //보스를 Instanciate로 만들어 준 후에 boss.CommonStatus.MaxHp = BossManager.hp; 이런 식으로 종류, 난이도에 따른 보스 능력치 초기화
    //BossManger 클래스에 gold 등등 리워드 변수도 추가
    //InGameManager에서 전달받은 후, 게임클리어 시 호출되는 함수에서 적용
}
