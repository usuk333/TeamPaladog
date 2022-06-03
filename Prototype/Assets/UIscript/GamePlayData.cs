using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayData : MonoBehaviour
{
    // �����忡 ������ �̱��� ����
    private static bool shuttingDown = false;
    private static object Lock = new object();
    private static GamePlayData instance;

    public static GamePlayData Instance
    {
        get
        {
            if (shuttingDown)
            {
                Debug.LogWarning("User Instance already destroyed. return null");
                return null;
            }

            lock (Lock)
            {
                if (instance == null)
                {
                    instance = (GamePlayData)FindObjectOfType(typeof(GamePlayData));

                    if (instance == null)
                    {
                        var userObject = new GameObject();
                        instance = userObject.AddComponent<GamePlayData>();
                        userObject.name = "ServerManager";

                        DontDestroyOnLoad(userObject);
                    }
                }

                return instance;
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // ���ӿ��� ������ ���� ������ �����صδ� ��
    public userLoginData mainUser;
    // ������ �κ��丮
    public userGoodsData mainInventory;

    // ���� ���� Ŭ����
    [System.Serializable]
    public class userLoginData
    {
        public enum LoginType
        {
            None = 0,
            anony = 1,
            email = 2,
            google = 3
        }
        public string nickname;
        public LoginType loginType;
        public string uid;
        public string email;
        public string pw;
        public string deviceModel;
        public string deviceName;
        public DeviceType deviceType;
        public string deviceOS;
        public ulong createDate;
    }

    // ���� ��ȭ ���� Ŭ����
    [System.Serializable]
    public class userGoodsData
    {
        public string uid;
        public int coin;
    }

    // ���� ������ ����
    [System.Serializable]
    public class userReceipt
    {
        //플레이어 1레벨 경험치
        public int userEXP = 800;
        //5레벨 마다 최대량 증가
        public int userEXPplus = 100;
        //1레벨 유닛 성장 능력치 1당 골드 소모량
        public int UnitUpgradeGold = 50;
        public string product_id;
        //5레벨 마다 최대량 증가
        public int UnitUpgradeGoldplus = 50;
        //1레벨 유닛 경험치
        public int UnitEXP = 800;
        //5레벨 마다 최대량 증가
        public int UnitEXPPlus = 200;
        //계열포인트 1당 경험치
        public int JobPointEXP = 1000;
    }
    [System.Serializable]
    public class UnitReceipt
    {
        //탱커 1레벨 경험치
        public int TankerEXP;
        //탱커
        public int TankerEXPup;
        //전사
        public int WarriorEXP = 50;
        public int WarriorEXPUp;
        //아처
        public int ArchorEXP = 50;
        //아처
        public int ArchorEXPUp;
        //법사    
        public int MageEXP;
        //법사
        public int MageEXPUp;
    }

    [System.Serializable]
    public class Archor
    {
        public int ArchorATK = 140;
        public int ArchorATKup = 5;
        public int ArchorATKMAX = 10;
        public int ArchorBasicHP = 800;
        public int ArchorHPup = 40;
        public int ArchorHPMAX = 80;
        public float ArchorATKspeed = 1.0f;

        public float ArchorSkillCost = 0.1f;
        public float ArchorSkillDamage = 1.8f;
        public float ArchorSkillCostup = 0.06f;

        public void ArchorSkill()
        {

        }
    }

    [System.Serializable]
    public class Tanker
    {
        //탱커 1레벨 경험치
        public int TankerATK;
        public int TankerATKup = 4;
        public int TankerATKMAX = 8;
        public int TankerBasicHP = 1200;
        public int TankerHPup = 75;
        public int TankerHPMAX = 150;
        public float TankerATKspeed = 0.8f;

        public int TankerSkillCost = 10;
        public float TankerSkillDamage = 1.5f;
        public float TankerSkillHeal = 2.0f;
        public int TankerSkillCostup = 1;

        public void TankerSkill()
        {

        }
    }

    [System.Serializable]
    public class Warrior
    {
        public int WarriorATK = 200;
        public int WarriorATKup = 4;
        public int WarriorATKMAX = 8;
        public int WarriorBasicHP = 1000;
        public int WarriorHPup = 50;
        public int WarriorHPMAX = 100;
        public float WarriorATKspeed = 0.9f;

        public float WarriorSkillCost = 0.1f;
        public float WarriorSkillDamage = 2.0f;
        public float WarriorSkillCostup = 0.04f;

        public void WarriorSkill()
        {

        }
    }

    [System.Serializable]
    public class Mage
    {
        public int MageATK = 200;
        public int MageATKup = 6;
        public int MageATKMAX = 12;
        public int MageBasicHP = 800;
        public int MageHPup = 40;
        public int MageHPMAX = 80;
        public float MageATKspeed = 0.7f;

        public int MageSkillCost = 10;
        public float MageSkillDamage = 4.0f;
        public int MageSkillCostup = 2;

        public void MageSkill()
        {

        }
    }


    // �������� ���� ������ �޾ƿ���
    /*public void GetUserData(string uid, System.Action callback)
    {
        //Server.Instance.GetUserDataDB(uid, callback);
    }

    // �������� ���� �κ� ���� �޾ƿ���
    public void GetUserInven(string uid, System.Action callback)
    {
        Server.Instance.GetUserInvenDB(uid, callback);
    }*/
}
