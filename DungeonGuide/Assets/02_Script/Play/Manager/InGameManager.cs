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

public class Reward
{
    public int exp;
    public int gold;
    public int warriorPoint;
    public int assassinPoint;
    public int magicianPoint;
    public int archorPoint;

    public Reward(int exp, int gold, byte warriorPoint, byte assassinPoint, byte magicianPoint, byte archorPoint)
    {
        this.exp = exp;
        this.gold = gold;
        this.warriorPoint = warriorPoint;
        this.assassinPoint = assassinPoint;
        this.magicianPoint = magicianPoint;
        this.archorPoint = archorPoint;
    }
    public int[] GetReward()
    {
        int[] rewardArray = { gold, warriorPoint, assassinPoint, magicianPoint, archorPoint };
        return rewardArray;
    }
}
public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //인게임씬 클래스들이 접근 용이하도록 스태틱으로 사용
    private int difficulty;
    private string title;
    private float timerSecond;
    private int timerMinute;
    [SerializeField] private Text timer;
    [SerializeField] private List<Unit> units = new List<Unit>();
    private Player player;
    [SerializeField] private Boss boss;

    private Reward stageReward;

    [SerializeField] private GameObject[] bossPrefabArray;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClipArray;
    public static InGameManager Instance { get => instance; }
    public List<Unit> Units { get => units; set => units = value; }
    public Player Player { get => player; }
    public Boss Boss { get => boss; set => boss = value; }

    public bool GameOver { get; set; }
    
    public bool GameClear { get; set; }
    public string Title { get => title; }
    public Reward StageReward { get => stageReward; }

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
        string[] path = { "Warrior", "Assassin", "Magician", "Archor" };
        for (int i = 0; i < units.Count; i++)
        {
            units[i].CommonStatus.AttackDamage = System.Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{path[i]}ATK"]);
            units[i].CommonStatus.MaxHp = System.Convert.ToInt32(GameManager.Instance.FirebaseData.UnitDictionary[$"{path[i]}HP"]);
            units[i].skillCondition = GameManager.Instance.FirebaseData.GetUnitSkillConditionByLevel(path[i]);
        }
    }
    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<Player>();
        InitBossStatus();
        InitUnitStatus();
        StartCoroutine(Co_InitializeInGameData());
    }
    private void InitBossStatus()
    {
        Instantiate(bossPrefabArray[GameManager.Instance.StageInfo.BossIndex]);
        difficulty = (int)GameManager.Instance.StageInfo.Difficulty;
        boss = FindObjectOfType<Boss>();
        if (boss.GetComponent<Beast>())
        {
            InitBeast();
            title = "잊혀진 사자들의 왕";
        }
        else if (boss.GetComponent<Gargoyle>())
        {
            InitGargoyle();
            title = "깨어난 고대의 감시자";
        }
        else if (boss.GetComponent<Mushroom>())
        {
            InitMushroom();
            title = "끈적끈적 보랏빛 재앙";
        }
        else if (boss.GetComponent<Inside>())
        {
            InitInside();
            title = "깨달음을 저버린 자";
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
                stageReward = new Reward(200, Random.Range(240, 260), 0, 1, 0, 1);
                InitBossUsualStatus(bossStatus);
                beast.beastStatus = beastStatus;
                break; 
            case 1:
                bossStatus = new BossStatus(210, 130000, 2);
                beastStatus = new BeastStatus(1200, 25, 30, 500, 12, 22, 70, 5, 10, 15, 20 , 1.1f);
                stageReward = new Reward(400, Random.Range(440, 520), 0, 2, 0, 2);
                InitBossUsualStatus(bossStatus);
                beast.beastStatus = beastStatus;
                break;
            case 2:
                bossStatus = new BossStatus(300, 175000, 2);
                beastStatus = new BeastStatus(1600, 25, 30, 800, 11, 21, 90, 5, 10, 20, 30, 0.2f);
                stageReward = new Reward(800, Random.Range(740, 840), 1, 3, 1, 3);
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
                stageReward = new Reward(350, Random.Range(260, 280), 1, 0, 0, 1);
                InitBossUsualStatus(bossStatus);
                gargoyle.gargoyleStatus = gargoyleStatus;
                break;
            case 1:
                bossStatus = new BossStatus(180, 140000, 2);
                gargoyleStatus = new GargoyleStatus(70, 85, 30, 40, 600, 11, 14, 1000000, 18, 25, 10000, new float[] { 70, 50, 20 }, 10);
                stageReward = new Reward(700, Random.Range(520, 700), 2, 0, 0, 2);
                InitBossUsualStatus(bossStatus);
                gargoyle.gargoyleStatus = gargoyleStatus;
                break;
            case 2:
                bossStatus = new BossStatus(240, 200000, 2);
                gargoyleStatus = new GargoyleStatus(120, 90, 25, 40, 1000, 8, 13, 1000000, 15, 20, 200000 * 10 / 100, new float[] { 75, 40, 5 }, 10);
                stageReward = new Reward(1200, Random.Range(900, 1100), 3, 1, 1, 3);
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
                bossStatus = new BossStatus(110, 120000, 2);
                mushroomStatus = new MushroomStatus(100, 10, 20, 20, 110, 2, 500, 50, 5, 8, 12, 15, 15, 5, 1);
                stageReward = new Reward(250, Random.Range(245, 265), 1, 0, 1, 0);
                InitBossUsualStatus(bossStatus);
                mushroom.mushroomStatus = mushroomStatus;
                break;
            case 1:
                bossStatus = new BossStatus(150, 155000, 2);
                mushroomStatus = new MushroomStatus(145, 10, 20, 17, 150, 2, 800, 70, 7, 13, 15, 15, 15, 5, 1);
                stageReward = new Reward(500, Random.Range(465, 595), 2, 0, 2, 0);
                InitBossUsualStatus(bossStatus);
                mushroom.mushroomStatus = mushroomStatus;
                break;
            case 2:
                bossStatus = new BossStatus(200, 200000, 2);
                mushroomStatus = new MushroomStatus(210, 10, 20, 13, 200, 2, 1200, 150, 15, 10, 13, 15, 15, 5, 1);
                stageReward = new Reward(900, Random.Range(780, 900), 3, 1, 3, 1);
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
                stageReward = new Reward(300, Random.Range(250, 270), 0, 1, 1, 0);
                InitBossUsualStatus(bossStatus);
                inside.insideStatus = insideStatus;
                break;
            case 1:
                bossStatus = new BossStatus(160, 130000, 2);
                insideStatus = new InsideStatus(70, 40, 800, 10, 20, new float[] { 75, 35 }, 1500, 5, 15);
                stageReward = new Reward(600, Random.Range(485, 620), 0, 2, 2, 0);
                InitBossUsualStatus(bossStatus);
                inside.insideStatus = insideStatus;
                break;
            case 2:
                bossStatus = new BossStatus(220, 170000, 2);
                insideStatus = new InsideStatus(100, 50, 1500, 10, 20, new float[] { 80, 50, 20 }, 2000, 5, 10);
                stageReward = new Reward(1000, Random.Range(800, 950), 1, 3, 3, 1);
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
        SoundManager.Instance.SetBGM(GameManager.Instance.StageInfo.GetBossBGM());
        StartCoroutine(Co_Timer());
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
        PlayAudio(0);
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
        GameManager.Instance.FirebaseData.SetReward(stageReward);
        InGameUIManager.instance.ShowClearPopUp();
        PlayAudio(1);
        UpdateStageClearInfo();
    }
    private void UpdateStageClearInfo()
    {
        int stageNumber = GameManager.Instance.StageInfo.BossIndex + 1;
        int stageDifficulty = (int)GameManager.Instance.StageInfo.Difficulty;
        string[] pathArray = { "E", "N", "H" };
        GameManager.Instance.FirebaseData.SaveData("Stage", $"S{stageNumber}{pathArray[stageDifficulty]}Clear", true);
    }
    /// <summary>
    /// 0 = 게임 오버, 1 = 게임 클리어
    /// </summary>
    /// <param name="index"></param>
    private void PlayAudio(int index)
    {
        audioSource.volume = SoundManager.Instance.SfxAudio.volume;
        SoundManager.Instance.BgmAudio.Stop();
        audioSource.Stop();
        audioSource.clip = audioClipArray[index];
        audioSource.Play();
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
