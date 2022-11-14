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
public struct MushroomStatus
{
    public float firstPatternDamage;
    public float firstPatternMinTime;
    public float firstPatternMaxTime;
    public float secondPatternCount;
    public float thirdPatternDamage;
    public float thirdPatternCount;
    public float forthPatternDamage;
    public float forthPatternDotDamage;
    public float forthPatternDotDuration;
    public float forthPatternMinCount;
    public float forthPatternMaxCount;
    public float forthPatternMinTime;
    public float forthPatternMaxTime;
    public float fifthPatternDuration;
    public float fifthPatternCastingTime;

    public MushroomStatus(float firstPatternDamage, float firstPatternMinTime, float firstPatternMaxTime, float secondPatternCount, float thirdPatternDamage, float thirdPatternCount, float forthPatternDamage, float forthPatternDotDamage, float forthPatternDotDuration, float forthPatternMinCount, float forthPatternMaxCount, float forthPatternMinTime, float forthPatternMaxTime, float fifthPatternDuration, float fifthPatternCastingTime)
    {
        this.firstPatternDamage = firstPatternDamage;
        this.firstPatternMinTime = firstPatternMinTime;
        this.firstPatternMaxTime = firstPatternMaxTime;
        this.secondPatternCount = secondPatternCount;
        this.thirdPatternDamage = thirdPatternDamage;
        this.thirdPatternCount = thirdPatternCount;
        this.forthPatternDamage = forthPatternDamage;
        this.forthPatternDotDamage = forthPatternDotDamage;
        this.forthPatternDotDuration = forthPatternDotDuration;
        this.forthPatternMinCount = forthPatternMinCount;
        this.forthPatternMaxCount = forthPatternMaxCount;
        this.forthPatternMinTime = forthPatternMinTime;
        this.forthPatternMaxTime = forthPatternMaxTime;
        this.fifthPatternDuration = fifthPatternDuration;
        this.fifthPatternCastingTime = fifthPatternCastingTime;
    }
}
public struct InsideStatus
{
    public float firstPatternDamage;
    public float firstPatternMana;
    public float firstPatternTrueDamage;
    public float firstPatternMinTime;
    public float firstPatternMaxTime;
    public float[] secondPatternPercentage;
    public float secondPatternDummyHp;
    public float secondPatternManaRegen;
    public float secondPatternPortalDuration;

    public InsideStatus(float firstPatternDamage, float firstPatternMana, float firstPatternTrueDamage, float firstPatternMinTime, float firstPatternMaxTime, float[] secondPatternPercentage, float secondPatternDummyHp, float secondPatternManaRegen, float secondPatternPortalDuration)
    {
        this.firstPatternDamage = firstPatternDamage;
        this.firstPatternMana = firstPatternMana;
        this.firstPatternTrueDamage = firstPatternTrueDamage;
        this.firstPatternMinTime = firstPatternMinTime;
        this.firstPatternMaxTime = firstPatternMaxTime;
        this.secondPatternPercentage = secondPatternPercentage;
        this.secondPatternDummyHp = secondPatternDummyHp;
        this.secondPatternManaRegen = secondPatternManaRegen;
        this.secondPatternPortalDuration = secondPatternPortalDuration;
    }
}
public struct GargoyleStatus
{
    public float firstPatternDamage;
    public float firstPatternSlowDown;
    public float firstPatternMinTime;
    public float firstPatternMaxTime;
    public float secondPatternDamage;
    public float secondPatternMinTime;
    public float secondPatternMaxTime;
    public float thirdPatternDamage;
    public float thirdPatternMinTime;
    public float thirdPatternMaxTime;
    public float forthPatternShield;
    public float[] forthPatternPercentage;
    public float forthPatternDuration;

    public GargoyleStatus(float firstPatternDamage, float firstPatternSlowDown, float firstPatternMinTime, float firstPatternMaxTime, float secondPatternDamage, float secondPatternMinTime, float secondPatternMaxTime, float thirdPatternDamage, float thirdPatternMinTime, float thirdPatternMaxTime, float forthPatternShield, float[] forthPatternPercentage, float forthPatternDuration)
    {
        this.firstPatternDamage = firstPatternDamage;
        this.firstPatternSlowDown = firstPatternSlowDown;
        this.firstPatternMinTime = firstPatternMinTime;
        this.firstPatternMaxTime = firstPatternMaxTime;
        this.secondPatternDamage = secondPatternDamage;
        this.secondPatternMinTime = secondPatternMinTime;
        this.secondPatternMaxTime = secondPatternMaxTime;
        this.thirdPatternDamage = thirdPatternDamage;
        this.thirdPatternMinTime = thirdPatternMinTime;
        this.thirdPatternMaxTime = thirdPatternMaxTime;
        this.forthPatternShield = forthPatternShield;
        this.forthPatternPercentage = forthPatternPercentage;
        this.forthPatternDuration = forthPatternDuration;
    }
}
public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //인게임씬 클래스들이 접근 용이하도록 스태틱으로 사용
    private int difficulty;
    private float timerSecond;
    private int timerMinute;
    [SerializeField] private Text timer;
    [SerializeField] private List<Unit> units = new List<Unit>();
    private Player player;
    [SerializeField] private Boss boss;
    [SerializeField] private SkillData[] skillDataArray;
    public static InGameManager Instance { get => instance; }
    public List<Unit> Units { get => units; set => units = value; }
    public Player Player { get => player; }
    public Boss Boss { get => boss; set => boss = value; }
    public SkillData[] SkillDataArray { get => skillDataArray; set => skillDataArray = value; }

    public bool GameOver { get; set; }
    
    public bool GameClear { get; set; }

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
    private void InitUnitStatus()
    {
        Unit[] unitArray = FindObjectsOfType<Unit>();
        for (int i = 0; i < unitArray.Length; i++)
        {
            units.Add(unitArray[i]);
        }
        Unit unit;
        for (int i = 0; i < units.Count; i++) //이 부분 추후에 함수로 빼기 (유닛리스트 탱커 ~ 원거리 순으로 정렬)
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
        for (int i = 0; i < units.Count; i++)
        {
            switch (i)
            {
                case 0:
                    units[i].CommonStatus.AttackDamage = BossManager.tanker.currentDamage;
                    units[i].CommonStatus.MaxHp = BossManager.tanker.currentHp;
                    units[i].CommonStatus.AttackSpeed = BossManager.tanker.attackSpeed;
                    units[i].skillCondition = BossManager.tanker.skillCondition;
                    break;
                case 1:
                    units[i].CommonStatus.AttackDamage = BossManager.warrior.currentDamage;
                    units[i].CommonStatus.MaxHp = BossManager.warrior.currentHp;
                    units[i].CommonStatus.AttackSpeed = BossManager.warrior.attackSpeed;
                    units[i].skillCondition = BossManager.warrior.skillCondition;
                    break;
                case 2:
                    units[i].CommonStatus.AttackDamage = BossManager.mage.currentDamage;
                    units[i].CommonStatus.MaxHp = BossManager.mage.currentHp;
                    units[i].CommonStatus.AttackSpeed = BossManager.mage.attackSpeed;
                    units[i].skillCondition = BossManager.mage.skillCondition;
                    break;
                case 3:
                    units[i].CommonStatus.AttackDamage = BossManager.archer.currentDamage;
                    units[i].CommonStatus.MaxHp = BossManager.archer.currentHp;
                    units[i].CommonStatus.AttackSpeed = BossManager.archer.attackSpeed;
                    units[i].skillCondition = BossManager.archer.skillCondition;
                    break;
                default:
                    break;
            }
        }
    }
    private void InitPlayerStatus()
    {
        player = FindObjectOfType<Player>();
        player.MaxHp = BossManager.hp.max;
        player.MaxMp = BossManager.mp.max;
        player.HpRegenerative = BossManager.hp.regen;
        player.MpRegenerative = BossManager.mp.regen;
        float[] skillValueArray = new float[6];
        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    skillValueArray[0] = BossManager.barrior.size;
                    break;
                case 1:
                    skillValueArray[1] = BossManager.heal.size;
                    break;
                case 2:
                    skillValueArray[2] = BossManager.powerUp.size;
                    break;
                case 3:
                    skillValueArray[3] = BossManager.attack.attackDamage[0];
                    break;
                case 4:
                    skillValueArray[4] = BossManager.attack.attackDamage[1];
                    break;
                case 5:
                    skillValueArray[5] = BossManager.attack.attackDamage[2];
                    break;
                default:
                    break;
            }
        }
        player.SkillValueArray = skillValueArray;
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    player.SkillCoolTimeArray[0] = BossManager.barrior.coolTime;
                    player.SkillManaArray[0] = BossManager.barrior.cost;
                    break;
                case 1:
                    player.SkillCoolTimeArray[1] = BossManager.heal.coolTime;
                    player.SkillManaArray[1] = BossManager.heal.cost;
                    break;
                case 2:
                    player.SkillCoolTimeArray[2] = BossManager.powerUp.coolTime;
                    player.SkillManaArray[2] = BossManager.powerUp.cost;
                    break;
                case 3:
                    player.SkillCoolTimeArray[3] = BossManager.attack.coolTime;
                    player.SkillManaArray[3] = BossManager.attack.cost;
                    break;
                default:
                    break;
            }
        }
        player.ShieldDuration = BossManager.barrior.time;
    }
    private void Awake()
    {
        instance = this;
        InitBossStatus();
        InitUnitStatus();
        InitPlayerStatus();
        StartCoroutine(Co_InitializeInGameData());
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
            InitGargoyle();
        }
        else if (boss.GetComponent<Mushroom>())
        {
            InitMushroom();
        }
        else if (boss.GetComponent<Inside>())
        {
            InitInside();
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
                beast.beastStatus = beastStatus;
                break; 
            case 1:
                bossStatus = new BossStatus(210, 130000, 2);
                beastStatus = new BeastStatus(1200, 25, 30, 500, 12, 22, 70, 5, 10, 15, 20 , 1.1f);
                InitBossUsualStatus(bossStatus);
                beast.beastStatus = beastStatus;
                break;
            case 2:
                bossStatus = new BossStatus(300, 175000, 2);
                beastStatus = new BeastStatus(1600, 25, 30, 800, 11, 21, 90, 5, 10, 20, 30, 0.2f);
                InitBossUsualStatus(bossStatus);
                beast.beastStatus = beastStatus;
                break;
            default:
                break;
        }
    }
    private void InitGargoyle()
    {
        var gargoyle = boss.GetComponent<Gargoyle>();
        BossStatus bossStatus;
        GargoyleStatus gargoyleStatus;
        switch (difficulty)
        {
            case 0:
                bossStatus = new BossStatus(140, 120000, 2);
                gargoyleStatus = new GargoyleStatus(40, 80, 30, 40, 300, 12, 16, 800, 20, 25, 6000, new float[] { 80, 40 }, 10);
                InitBossUsualStatus(bossStatus);
                gargoyle.gargoyleStatus = gargoyleStatus;
                break;
            case 1:
                bossStatus = new BossStatus(180, 140000, 2);
                gargoyleStatus = new GargoyleStatus(70, 85, 30, 40, 600, 11, 14, 1000000, 18, 25, 10000, new float[] { 70, 50, 20 }, 10);
                InitBossUsualStatus(bossStatus);
                gargoyle.gargoyleStatus = gargoyleStatus;
                break;
            case 2:
                bossStatus = new BossStatus(240, 200000, 2);
                gargoyleStatus = new GargoyleStatus(120, 90, 25, 40, 1000, 8, 13, 1000000, 15, 20, 200000 * 10 / 100, new float[] { 75, 40, 5 }, 10);
                InitBossUsualStatus(bossStatus);
                gargoyle.gargoyleStatus = gargoyleStatus;
                break;
            default:
                break;
        }
    }
    private void InitMushroom()
    {
        var mushroom = boss.GetComponent<Mushroom>();
        BossStatus bossStatus;
        MushroomStatus mushroomStatus;
        switch (difficulty)
        {
            case 0:
                bossStatus = new BossStatus(110, 100000, 2);
                mushroomStatus = new MushroomStatus(55, 10, 20, 20, 110, 2, 500, 50, 5, 8, 12, 15, 15, 5, 1);
                InitBossUsualStatus(bossStatus);
                mushroom.mushroomStatus = mushroomStatus;
                break;
            case 1:
                bossStatus = new BossStatus(150, 125000, 2);
                mushroomStatus = new MushroomStatus(75, 10, 20, 17, 150, 2, 800, 70, 7, 13, 15, 15, 15, 5, 1);
                InitBossUsualStatus(bossStatus);
                mushroom.mushroomStatus = mushroomStatus;
                break;
            case 2:
                bossStatus = new BossStatus(200, 160000, 2);
                mushroomStatus = new MushroomStatus(100, 10, 20, 13, 200, 2, 1200, 150, 15, 10, 13, 15, 15, 5, 1);
                InitBossUsualStatus(bossStatus);
                mushroom.mushroomStatus = mushroomStatus;
                break;
            default:
                break;
        }
    }
    private void InitInside()
    {
        var inside = boss.GetComponent<Inside>();
        BossStatus bossStatus;
        InsideStatus insideStatus;
        switch (difficulty)
        {
            case 0:
                bossStatus = new BossStatus(120, 105000, 2);
                insideStatus = new InsideStatus(50, 30, 500, 10, 20, new float[] { 80, 40 }, 1000, 5, 15);
                InitBossUsualStatus(bossStatus);
                inside.insideStatus = insideStatus;
                break;
            case 1:
                bossStatus = new BossStatus(160, 130000, 2);
                insideStatus = new InsideStatus(70, 40, 800, 10, 20, new float[] { 75, 35 }, 1500, 5, 15);
                InitBossUsualStatus(bossStatus);
                inside.insideStatus = insideStatus;
                break;
            case 2:
                bossStatus = new BossStatus(220, 170000, 2);
                insideStatus = new InsideStatus(100, 50, 1500, 10, 20, new float[] { 80, 50, 20 }, 2000, 5, 10);
                InitBossUsualStatus(bossStatus);
                inside.insideStatus = insideStatus;
                inside.difficulty = true;
                break;
            default:
                break;
        }
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
    public void SetGameOver()
    {
        GameOver = true;
        var a = FindObjectsOfType<MonoBehaviour>();
        foreach (var item in a)
        {
            item.StopAllCoroutines();
        }
        InGameUIManager.instance.ShowGameOverPopUp();
        Debug.Log("Game Over!");
    }
    public void SetGameClear()
    {
        GameClear = true;
        var a = FindObjectsOfType<MonoBehaviour>();
        foreach (var item in a)
        {
            item.StopAllCoroutines();
        }
        FindObjectOfType<BossHpBar>().SetZeroBossHpBar();
        Debug.Log("Game Clear!");
        InGameUIManager.instance.ShowClearPopUp();
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
