using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

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

    void Awake()
    {
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";

        reference.Child("users").Child(Userid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                Debug.Log("컴플릿떳음");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    //Dictionary<string, string> userinfo = (Dictionary<string, string>)data.Value;
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                    if(data.Key == "Gold")
                    {
                        Golds = data.Value.ToString();
                        Debug.LogFormat("씨발value는 {0}", data.Value);
                        Debug.Log("좆같은Golds는 " + Golds);//내일은 미쳐날뛰는 링크로 해보자
                    }
                }
            }
        });
        isLoad = true;
        Debug.Log("끝!");
        //StartCoroutine(UpdateData());
        GoUpdate();
    }
    IEnumerator UpdateData()
    {
        yield return new WaitUntil(() => isLoad);

        Gold.text = "Gold : " + Golds;
        isLoad = false;
    }

    void GoUpdate()
    {
        Debug.Log("Golds는 " + Golds);
        Gold.text = "Gold : " + Golds;
    }
}
