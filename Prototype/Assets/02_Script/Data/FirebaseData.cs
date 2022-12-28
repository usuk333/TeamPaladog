using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

namespace Data
{

    public class FirebaseData
    {
        private DatabaseReference reference;
        private FirebaseDatabase firebaseDatabase;
        private FirebaseApp firebaseApp;

        public enum LoginType
        {
            None = 0,
            google = 1
        }

        public string UID;

        private bool snapshotLoadComplete = false;
        public bool dataLoadComplete = false;

        //플레이어 정보 데이터
        /// <summary>
        /// 0 = HP, 1 = MP, 2 = Level, 3 = EXP, 4 = Gold, 5 = WarriorPoint, 6 = AssasinPoint, 7 = MagePoint, 8 = ADPoint, 9 = Nickname
        /// </summary>
        private string[] infoPathArray = { "Level", "EXP", "Gold", "WarriorPoints", "AssassinPoints", "MagicianPoints", "ArchorPoints", "Nickname" };
        private Dictionary<string, object> infoDictionary = new Dictionary<string, object>();

        //스킬 데이터
        /// <summary>
        /// 0 = Attack, 1 = Barrior, 2 = Heal, 3 = PowerUp, 4 = SkillPoints
        /// </summary>
        private string[] skillPathArray = { "HP", "MP", "Attack", "Barrior", "Heal", "PowerUp", "SkillPoints" };
        private Dictionary<string, object> skillDictionary = new Dictionary<string, object>();

        private string[] stagePathArray = { "S1EClear", "S1NClear", "S1HClear", "S1EStar", "S1NStar", "S1HStar",
        "S2EClear", "S2NClear", "S2HClear", "S2EStar", "S2NStar", "S2HStar",
        "S3EClear", "S3NClear", "S3HClear", "S3EStar", "S3NStar", "S3HStar",
        "S4EClear", "S4NClear", "S4HClear", "S4EStar", "S4NStar", "S4HStar" };
        private Dictionary<string, object> stageDictionary = new Dictionary<string, object>();

        //유닛 데이터
        private string[] unitPathArray =
        {  "WarriorATK", "WarriorEXP", "WarriorHP", "WarriorLevel",
         "ArchorATK", "ArchorEXP", "ArchorHP", "ArchorLevel",
         "MagicianATK", "MagicianEXP", "MagicianHP", "MagicianLevel",
         "AssassinATK", "AssassinEXP", "AssassinHP", "AssassinLevel"  };
        private Dictionary<string, object> unitDictionary = new Dictionary<string, object>();

        //모든 딕셔너리를 담은 딕셔너리. 데이터 저장 시 사용
        private Dictionary<string, Dictionary<string, object>> allDictionarys = new Dictionary<string, Dictionary<string, object>>();


        private Skill[] skillArray = new Skill[6];
        /// <summary>
        /// 생성자 호출 동시에 DB내부 UID의 데이터 로딩 / 객체의 스냅샷에 데이터 읽어온 후 딕셔너리에 저장
        /// </summary>
        /// <param name="UID"></param>
        public FirebaseData(string UID)
        {
            this.UID = UID;
            LoadData();
        }

        private DataSnapshot snapshot;

        private DataSnapshot Infosnapshot;
        private DataSnapshot Unitsnapshot;
        private DataSnapshot Skillsnapshot;
        private DataSnapshot Stagesnapshot;

        public Dictionary<string, object> InfoDictionary { get => infoDictionary; }
        public Dictionary<string, object> SkillDictionary { get => skillDictionary; }
        public Dictionary<string, object> StageDictionary { get => stageDictionary; }
        public Dictionary<string, object> UnitDictionary { get => unitDictionary; }
        public Skill[] SkillArray { get => skillArray; set => skillArray = value; }
        #region --데이터 이니셜라이징--
        public IEnumerator Co_InitData()
        {
            yield return new WaitUntil(() => snapshotLoadComplete);
            yield return new WaitUntil(() => Recursive_InitData(Infosnapshot, infoDictionary, infoPathArray));
            yield return new WaitUntil(() => Recursive_InitData(Skillsnapshot, skillDictionary, skillPathArray));
            yield return new WaitUntil(() => Recursive_InitData(Stagesnapshot, stageDictionary, stagePathArray));
            yield return new WaitUntil(() => Recursive_InitData(Unitsnapshot, unitDictionary, unitPathArray));

            InitSkillArray();
            dataLoadComplete = true;
        }
        private void InitSkillArray()
        {
            byte i = 0;
            skillArray[i] = new SkillMaxHp(Convert.ToInt32(skillDictionary[skillPathArray[i++]]));
            skillArray[i] = new SkillMaxMp(Convert.ToInt32(skillDictionary[skillPathArray[i++]]));
            skillArray[i] = new SkillAttack(Convert.ToInt32(skillDictionary[skillPathArray[i++]]));
            skillArray[i] = new SkillBarrior(Convert.ToInt32(skillDictionary[skillPathArray[i++]]));
            skillArray[i] = new SkillHeal(Convert.ToInt32(skillDictionary[skillPathArray[i++]]));
            skillArray[i] = new SkillPowerUp(Convert.ToInt32(skillDictionary[skillPathArray[i]]));
        }
        private void LoadData()
        {
            firebaseApp = FirebaseApp.DefaultInstance;
            firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
            reference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(UID);
            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("failed reading...");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Complete");
                    snapshot = task.Result;
                    Debug.Log("모든 데이터 snapshot 읽어오기");

                    Infosnapshot = snapshot.Child("Info");
                    Unitsnapshot = snapshot.Child("Unit");
                    Skillsnapshot = snapshot.Child("Skill");
                    Stagesnapshot = snapshot.Child("Stage");

                    snapshotLoadComplete = true;
                }
            });
        }
        /// <summary>
        /// 매개변수로 받은 dictionary에 snapshot의 값들을 넣어줌
        /// path는 스냅샷의 key값 배열
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="dictionary"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool Recursive_InitData(DataSnapshot snapshot, Dictionary<string, object> dictionary, string[] path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (dictionary.ContainsKey(path[i]))
                {
                    continue;
                }
                dictionary.Add(path[i], snapshot.Child(path[i]).Value);
                Debug.Log($"{snapshot.Key}.{path[i]} : {dictionary[path[i]]}");
                if (dictionary[path[i]] == null)
                {
                    Debug.Log("데이터 로딩에 실패하여 데이터를 다시 불러옵니다");
                    return Recursive_InitData(snapshot, dictionary, path);
                }
            }
            allDictionarys.Add(snapshot.Key, dictionary);
            return true;
        }
        #endregion
        /// <summary>
        /// dictionaryKey = 최상위 경로 (Unit, Skill, Stage, Info)
        /// dataPath = 자식 경로 (경로명은 FirebaseData 클래스에 있음)
        /// value = 수정할 값
        /// </summary>
        /// <param name="dictionaryKey"></param>
        /// <param name="dataPath"></param>
        /// <param name="value"></param>
        public void SaveData(string dictionaryKey, string dataPath, object value)
        {
            if (!allDictionarys.ContainsKey(dictionaryKey) || !allDictionarys[dictionaryKey].ContainsKey(dataPath)) return;

            allDictionarys[dictionaryKey][dataPath] = value;
            reference.Child(dictionaryKey).Child(dataPath).SetValueAsync(value);
        }
        public void SetReward(Reward reward)
        {
            int gold = Convert.ToInt32(infoDictionary["Gold"]) + reward.gold;
            int exp = Convert.ToInt32(infoDictionary["EXP"]) + reward.exp;
            int warriorP = Convert.ToInt32(infoDictionary["WarriorPoints"]) + reward.warriorPoint;
            int assasinP = Convert.ToInt32(infoDictionary["AssassinPoints"]) + reward.assassinPoint;
            int magicianP = Convert.ToInt32(infoDictionary["MagicianPoints"]) + reward.magicianPoint;
            int archerP = Convert.ToInt32(infoDictionary["ArchorPoints"]) + reward.archorPoint;
            string[] pathArray = { "Gold", "EXP", "WarriorPoints", "AssassinPoints", "MagicianPoints", "ArchorPoints" };

            if (exp >= GetPlayerMaxExpByLevel())
            {
                int levelUp = GetLevelUpCount(exp);
                for (int i = 0; i < levelUp; i++)
                {
                    exp -= GetPlayerMaxExpByLevel();
                }
                SaveData("Info", "Level", Convert.ToInt32(infoDictionary["Level"]) + levelUp);
                SaveData("Skill", "SkillPoints", Convert.ToInt32(skillDictionary["SkillPoints"]) + levelUp);
            }
            int[] valueArray = { gold, exp, warriorP, assasinP, magicianP, archerP };
            for (int i = 0; i < valueArray.Length; i++)
            {
                SaveData("Info", pathArray[i], valueArray[i]);
            }

        }
        public int GetLevelUpCount(int exp)
        {
            int up = 0;
            int value = exp;
            while (value >= GetPlayerMaxExpByLevel())
            {
                value -= GetPlayerMaxExpByLevel();
                up++;
            }
            return up;
        }
        public int GetUnitMaxAtkByLevel(string unitName)
        {
            int a;
            int level = Convert.ToInt32(unitDictionary[$"{unitName}Level"]) - 1;
            switch (unitName)
            {
                case "Warrior":
                    a = 380;
                    a = a + (16 * level);
                    break;
                case "Assassin":
                    a = 400;
                    a = a + (16 * level);
                    break;
                case "Magician":
                    a = 400;
                    a = a + (24 * level);
                    break;
                case "Archor":
                    a = 380;
                    a = a + (20 * level);
                    break;
                default:
                    a = 0;
                    break;
            }
            return a;
        }
        public int GetUnitMaxHpByLevel(string unitName)
        {
            int a;
            int level = Convert.ToInt32(unitDictionary[$"{unitName}Level"]) - 1;
            switch (unitName)
            {
                case "Warrior":
                    a = 1200;
                    a = a + (150 * level);
                    break;
                case "Assassin":
                    a = 1000;
                    a = a + (100 * level);
                    break;
                case "Magician":
                    a = 800;
                    a = a + (80 * level);
                    break;
                case "Archor":
                    a = 800;
                    a = a + (80 * level);
                    break;
                default:
                    a = 0;
                    break;
            }
            return a;
        }
        public int GetUnitSkillConditionByLevel(string unitName)
        {
            int a;
            int level = Convert.ToInt32(unitDictionary[$"{unitName}Level"]);
            int count = level / 10;
            switch (unitName)
            {
                case "Warrior":
                    a = 10;
                    a = a - count;
                    break;
                case "Assassin":
                    a = 10;
                    a = a + (4 * count);
                    break;
                case "Magician":
                    a = 10;
                    a = a - count;
                    break;
                case "Archor":
                    a = 10;
                    a = a + (6 * count);
                    break;
                default:
                    a = 0;
                    break;
            }
            return a;
        }
        public int GetPlayerMaxExpByLevel()
        {
            int level = Convert.ToInt32(infoDictionary["Level"]);
            int max = 800 + (100 * (level / 5));
            return max;
        }
        public int GetUnitMaxExpByLevel(string unitName)
        {
            int level = Convert.ToInt32(unitDictionary[$"{unitName}Level"]);
            int max = 800;
            for (int i = 1; i <= level / 5; i++)
            {
                max += 200 * i;
            }
            return max;
        }
        public int GetUnitUpgradeGoldByLeve(string unitName)
        {
            int level = Convert.ToInt32(unitDictionary[$"{unitName}Level"]) - 1;
            int gold = 50;
            for (int i = 1; i <= level / 5; i++)
            {
                gold += 50 * i;
            }
            return gold;
        }
    }
}
