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

    private string Golds = "999";
    private string TLvs;
    private string THps;
    private string FLvs;
    private string FHps;
    private string MksLvs;
    private string MksHps;
    private string MLvs;
    private string MHps;

    [SerializeField] private Text Gold;
    [SerializeField] private Text TLv;
    [SerializeField] private Text THp;
    [SerializeField] private Text FLv;
    [SerializeField] private Text FHp;
    [SerializeField] private Text MksLv;
    [SerializeField] private Text MksHp;
    [SerializeField] private Text MLv;
    [SerializeField] private Text MHp;

    private string[] str;
    private long strLen;
    int count = 0;

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
        reference.Child("users").Child(Userid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                string Usersinfo = snapshot.Child("NaIxowYCsaSqdaYaWtWbIYErkqM2").Child("Gold").Value.ToString();

                //Debug.Log("Gold : " + Usersinfo);

                //Debug.Log(snapshot.ChildrenCount);

                Gold.text = "Gold : " + Usersinfo;
                Debug.Log("ÄÄÇÃ¸´¶¸À½");

                /*foreach (DataSnapshot data in snapshot.Children)
                {
                    //Dictionary<string, string> userinfo = (Dictionary<string, string>)data.Value;
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                    if(data.Key == "Gold")
                    {
                        Golds = data.Value.ToString();
                        Debug.LogFormat("value´Â {0}", data.Value);
                        Debug.Log("Golds´Â " + Golds);//h
                        Gold.text = "Gold : " + Golds;
                    }
                }*/
            }
        });
        isLoad = true;
        Debug.Log("³¡!");
        //StartCoroutine(UpdateData());
        //GoUpdate();
    }
    IEnumerator UpdateData()
    {
        yield return new WaitUntil(() => isLoad);

        Gold.text = "Gold : " + Golds;
        isLoad = false;
    }

    public void GoUpdate()
    {
        Debug.Log("Golds´Â " + Golds);
        Gold.text = "Gold : " + Golds;
    }
}
