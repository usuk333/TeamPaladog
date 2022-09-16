using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct BossStatus
{
    public float damage;
    public float hp;
    public float attackSpeed;

    public BossStatus(float damage, float hp, float attackSpeed)
    {
        this.damage = damage;
        this.hp = hp;
        this.attackSpeed = attackSpeed;
    }
}
public struct BeastStatus
{
    public float firstPatternDamage;
    public float firstPatternMinTime;
    public float firstPatternMaxTime;
    public float secondPatternDamage;
    public float secondPatternMinTime;
    public float secondPatternMaxTime;
    public float thirdPatternDamage;
    public float thirdPatternMinTime;
    public float thirdPatternMaxTime;
    public float forthPatternPercentage;
    public float forthPatternAtkValue;
    public float forthPatternAtksValue;

    public BeastStatus(float firstPatternDamage, float firstPatternMinTime, float firstPatternMaxTime, float secondPatternDamage, float secondPatternMinTime, float secondPatternMaxTime, float thirdPatternDamage, float thirdPatternMinTime, float thirdPatternMaxTime, float forthPatternPercentage, float forthPatternAtkValue, float forthPatternAtksValue)
    {
        this.firstPatternDamage = firstPatternDamage;
        this.firstPatternMinTime = firstPatternMinTime;
        this.firstPatternMaxTime = firstPatternMaxTime;
        this.secondPatternDamage = secondPatternDamage;
        this.secondPatternMinTime = secondPatternMinTime;
        this.secondPatternMaxTime = secondPatternMaxTime;
        this.thirdPatternDamage = thirdPatternDamage;
        this.thirdPatternMinTime = thirdPatternMinTime;
        this.thirdPatternMaxTime = thirdPatternMaxTime;
        this.forthPatternPercentage = forthPatternPercentage;
        this.forthPatternAtkValue = forthPatternAtkValue;
        this.forthPatternAtksValue = forthPatternAtksValue;
    }
}
public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //인게임씬 클래스들이 접근 용이하도록 스태틱으로 사용
    private int difficulty;
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
        instance = this;
        InitBossStatus();
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
    private void InitBossStatus()
    {
        Instantiate(BossManager.BossSection);
        difficulty = (int)BossManager.difficulty;
        boss = FindObjectOfType<Boss>();
        if (boss.GetComponent<Beast>())
        {
            InitBeast();
        }
        else if (boss.GetComponent<Gargoyle>())
        {

        }
        else if (boss.GetComponent<Mushroom>())
        {

        }
        else if (boss.GetComponent<Inside>())
        {

        }
    }
    private void InitBossUsualStatus(BossStatus bs)
    {
        boss.CommonStatus.AttackDamage = bs.damage;
        boss.CommonStatus.MaxHp = bs.hp;
        boss.CommonStatus.AttackSpeed = bs.attackSpeed;
    }
    private void InitBeast()
    {
        var beast = boss.GetComponent<Beast>();
        BossStatus bossStatus;
        BeastStatus beastStatus;
        switch (difficulty)
        {
            case 0:
                bossStatus = new BossStatus(100, 100000, 2);
                beastStatus = new BeastStatus(800, 25, 30, 500, 13, 23, 50, 5, 10, 10, 1.2f, 1.1f);
                InitBossUsualStatus(bossStatus);
                beast.InitStatus(beastStatus);
                break; 
            case 1:
                bossStatus = new BossStatus(210, 130000, 2);
                beastStatus = new BeastStatus(1200, 25, 30, 500, 12, 22, 70, 5, 10, 15, 20 , 1.1f);
                InitBossUsualStatus(bossStatus);
                beast.InitStatus(beastStatus);
                break;
            case 2:
                bossStatus = new BossStatus(300, 175000, 2);
                beastStatus = new BeastStatus(1600, 25, 30, 800, 11, 21, 90, 5, 10, 20, 30, 0.2f);
                InitBossUsualStatus(bossStatus);
                beast.InitStatus(beastStatus);
                break;
            default:
                break;
        }
    }
    private void InitGargoyle()
    {

    }
    private void InitMushroom()
    {

    }
    private void InitInside()
    {

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
        if(boss != null)
        {
            boss.StopAllCoroutines();
        }
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
