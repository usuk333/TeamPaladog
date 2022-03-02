using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

using Google;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;


public class ShopManager : MonoBehaviour
{
    public class UserInfo
    {
        public int Gold = 0;
    }

    [SerializeField] private string Userid;

    private DatabaseReference reference;

    private bool isLoad;

    private string Unitpoints = string.Empty;
    private string Warrior_level = string.Empty;
    private string Shielder_level = string.Empty;
    private string Archor_level = string.Empty;
    private string Magician_level = string.Empty;

    [SerializeField] private Text Gold;
    [SerializeField] private Text TLv;
    [SerializeField] private Text THp;
    [SerializeField] private Text FLv;
    [SerializeField] private Text FHp;
    [SerializeField] private Text MksLv;
    [SerializeField] private Text MksHp;
    [SerializeField] private Text MLv;
    [SerializeField] private Text MHp;

    DataSnapshot snapshot;

    FirebaseDatabase firebaseDatabase;

    FirebaseApp firebaseApp;

    void Awake()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";
        //reference.Child("users").Child(Userid).GetValueAsync().ContinueWith
        reference.Child("users").Child(Userid).Child("Unit").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("컴플릿떳음");
                snapshot = task.Result;

                StartCoroutine(UIupdate());
            }
        });
    }
    IEnumerator UpdateData()
    {
        yield return new WaitUntil(() => isLoad);


        isLoad = false;
    }
    private IEnumerator UIupdate()
    {
        yield return null;

        Debug.Log("UI업데이트시작");

        Unitpoints = snapshot.Child("UnitPoints").Value.ToString();
        Warrior_level = snapshot.Child("Warrior").Child("Warrior_Level").Value.ToString();
        Archor_level = snapshot.Child("Archor").Child("Archor_Level").Value.ToString();
        Shielder_level = snapshot.Child("Shielder").Child("Shielder_Level").Value.ToString();
        Magician_level = snapshot.Child("Magician").Child("Magician_Level").Value.ToString();

        Debug.Log("스냅샷 자식 수는 " + snapshot.ChildrenCount);

        Gold.text = "UnitPoints : " + Unitpoints;
        FLv.text = "LV : " + Warrior_level;
        TLv.text = "LV : " + Shielder_level;
        MksLv.text = "LV : " + Archor_level;
        MLv.text = "LV : " + Magician_level;

        //HP 공식 추가되면 함수 만들기
    }

    public void ShopGoMain()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void TankerUp()
    {
        int SL = int.Parse(Shielder_level);
        int SLs = SL + 1;
        Shielder_level = SLs.ToString();
        TLv.text = "LV : " + SLs;
        //reference.Child("users").Child(Userid).Child("Unit").SetValueAsync(json);
    }

    public void WarriorUp()
    {
        int WL = int.Parse(Warrior_level);
        int WLs = WL + 1;
        Warrior_level = WLs.ToString();
        FLv.text = "LV : " + WLs;
    }

    public void MarksmanUp()
    {
        int AL = int.Parse(Archor_level);
        int ALs = AL + 1;
        Archor_level = ALs.ToString();
        TLv.text = "LV : " + ALs;
    }

    public void MagicianUp()
    {
        int SL = int.Parse(Shielder_level);
        int SLs = SL + 1;
        Shielder_level = SLs.ToString();
        TLv.text = "LV : " + SLs;
    }
}
