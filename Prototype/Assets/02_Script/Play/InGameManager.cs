using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance; //�ΰ��Ӿ� Ŭ�������� ���� �����ϵ��� ����ƽ���� ���

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

    //�����غ��� ��������Ʈ�� �ϴ°� �� ������?
    private IEnumerator Co_InitializeInGameData() //�ʱ�ȭ�� �ʿ��� ��� �ΰ��� ������ �ʱ�ȭ. �Ϸ�Ǹ� true ��ȯ �� �ε� �Ϸ�.
    {
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
        StartCoroutine(Co_InitializeInGameData());
        Instantiate(BossManager.BossSection);
        units = FindObjectsOfType<Unit>();
        Unit unit;
        for (int i = 0; i < units.Length; i++) //�� �κ� ���Ŀ� �Լ��� ���� (���ָ���Ʈ ��Ŀ ~ ���Ÿ� ������ ����)
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
}
