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
                Debug.Log("ÄÄÇÃ¸´¶¸À½");
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
    IEnumerator UIupdate()
    {
        yield return null;

        Debug.Log("UI¾÷µ¥ÀÌÆ®½ÃÀÛ");

        Unitpoints = snapshot.Child("UnitPoints").Value.ToString();
        Warrior_level = snapshot.Child("Warrior").Child("Warrior_Level").Value.ToString();
        Archor_level = snapshot.Child("Archor").Child("Archor_Level").Value.ToString();
        Shielder_level = snapshot.Child("Shielder").Child("Shielder_Level").Value.ToString();
        Magician_level = snapshot.Child("Magician").Child("Magician_Level").Value.ToString();

        Debug.Log("½º³À¼¦ ÀÚ½Ä ¼ö´Â " + snapshot.ChildrenCount);

        Gold.text = "UnitPoints : " + Unitpoints;
        FLv.text = "LV : " + Warrior_level;
        TLv.text = "LV : " + Shielder_level;
        MksLv.text = "LV : " + Archor_level;
        MLv.text = "LV : " + Magician_level;
    }

}
