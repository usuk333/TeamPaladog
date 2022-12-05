using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

[System.Serializable]
public class FirebaseData
{
    private DatabaseReference reference;
    FirebaseDatabase firebaseDatabase;
    FirebaseApp firebaseApp;

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
    private string[] infoPathArray = { "HP", "MP", "Level", "EXP", "Gold", "WarriorPoints", "AssassinPoints", "MagePoints", "ADPoints", "Nickname" };
    private Dictionary<string, object> infoDictionary = new Dictionary<string, object>();

    //스킬 데이터
    /// <summary>
    /// 0 = Attack, 1 = Barrior, 2 = Heal, 3 = PowerUp, 4 = SkillPoints
    /// </summary>
    private string[] skillPathArray = { "Attack", "Barrior", "Heal", "PowerUp", "SkillPoints" };
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
    public IEnumerator InitData()
    {
        yield return new WaitUntil(() => snapshotLoadComplete);
        yield return new WaitUntil(() => Recursive_InitData(Infosnapshot, infoDictionary, infoPathArray));
        yield return new WaitUntil(() => Recursive_InitData(Skillsnapshot, skillDictionary, skillPathArray));
        yield return new WaitUntil(() => Recursive_InitData(Stagesnapshot, stageDictionary, stagePathArray));
        yield return new WaitUntil(() => Recursive_InitData(Unitsnapshot, unitDictionary, unitPathArray));

        dataLoadComplete = true;
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
            Debug.Log(dictionary[path[i]]);
            if (dictionary[path[i]] == null)
            {
                Debug.Log("데이터 로딩에 실패하여 데이터를 다시 불러옵니다");
                return Recursive_InitData(snapshot, dictionary, path);
            }
        }
        allDictionarys.Add(snapshot.Key, dictionary);
        return true;
    }

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
}
