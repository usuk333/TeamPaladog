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

using System.IO;
using System.Threading.Tasks;
using TMPro;

public class RewardManager : MonoBehaviour
{
    //파이어베이스
    FirebaseDatabase firebaseDatabase;
    FirebaseApp firebaseApp;
    private DatabaseReference reference;
    DataSnapshot snapshot;

    [SerializeField] public string Userid;
    //NaIxowYCsaSqdaYaWtWbIYErkqM2


    [SerializeField] private int StageNumber;
    //보스 인지 변수 int 아니어도됨
    //수인 이지, 노말 하드 = 1, 2, 3
    //버섯 이지, 노말 하드 = 4, 5, 6
    //스님 이지, 노말 하드 = 7, 8, 9
    //석상 이지, 노말 하드 = 10, 11, 12

    private string TankerPoints = string.Empty;
    private string WarriorPoints = string.Empty;
    private string ADPoints = string.Empty;
    private string MagePoints = string.Empty;

    private string Gold = string.Empty;

    private string EXP = string.Empty;
    private string Lv = string.Empty;

    private int MAXEXP;

    [SerializeField] private GameObject rewardpanel;
    [SerializeField] private Text rewardtext;

    void Awake()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start()
    {
        reference.Child("users").Child(Userid).Child("Info").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log("데이터 접속");

                StartCoroutine(Startuiupdate());
            }
        });
    }

    private IEnumerator Startuiupdate()
    {
        yield return null;

        if (snapshot == null)
        {
            Start();
        }

        Lv = snapshot.Child("Level").Value.ToString();
        EXP = snapshot.Child("EXP").Value.ToString();

        Gold = snapshot.Child("Gold").Value.ToString();
        TankerPoints = snapshot.Child("Points").Child("TankerPoints").Value.ToString();
        WarriorPoints = snapshot.Child("Points").Child("WarriorPoints").Value.ToString();
        ADPoints = snapshot.Child("Points").Child("ADPoints").Value.ToString();
        MagePoints = snapshot.Child("Points").Child("MagePoints").Value.ToString();

        MAXEXP = 800 + (int.Parse(Lv) / 5) * 100;

        Debug.Log(MAXEXP);
        GetReward(StageNumber);
    }

    public void GetReward(int i)
    {
        switch(i)
        {
            case 1:
                int NEXP = int.Parse(EXP) + 200;
                int NGold = int.Parse(Gold) + Random.Range(240, 260);
                int NWP = int.Parse(WarriorPoints) + 1;
                int NAP = int.Parse(ADPoints) + 1;

                Dictionary<string, object> updatea = new Dictionary<string, object>();
                Dictionary<string, object> updateb = new Dictionary<string, object>();
                Dictionary<string, object> updatec = new Dictionary<string, object>();
                updatea.Add("Gold", NGold);
                updateb.Add("WarriorPoints", NWP);
                updatec.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatea);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateb);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatec);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "수인\n이지\nEXP : 200 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 전사 1, 궁수 1";
                break;
            case 2:
                NEXP = int.Parse(EXP) + 400;
                NGold = int.Parse(Gold) + Random.Range(440, 520);
                NWP = int.Parse(WarriorPoints) + 2;
                NAP = int.Parse(ADPoints) + 2;

                Dictionary<string, object> updatee = new Dictionary<string, object>();
                Dictionary<string, object> updatef = new Dictionary<string, object>();
                Dictionary<string, object> updateg = new Dictionary<string, object>();
                updatee.Add("Gold", NGold);
                updatef.Add("WarriorPoints", NWP);
                updateg.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatee);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatef);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateg);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "수인\n노말\nEXP : 400 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 전사 2, 궁수 2";
                break;
            case 3:
                 NEXP = int.Parse(EXP) + 800;
                 NGold = int.Parse(Gold) + Random.Range(740, 840);
                int NTP = int.Parse(TankerPoints) + 1;
                NWP = int.Parse(WarriorPoints) + 3;
                int NMP = int.Parse(MagePoints) + 1;
                NAP = int.Parse(ADPoints) + 3;

                Dictionary<string, object> updateh = new Dictionary<string, object>();
                Dictionary<string, object> updatei = new Dictionary<string, object>();
                Dictionary<string, object> updatej = new Dictionary<string, object>();
                Dictionary<string, object> updatell = new Dictionary<string, object>();
                Dictionary<string, object> updatemm = new Dictionary<string, object>();
                updateh.Add("Gold", NGold);
                updatell.Add("TankerPoints", NTP);
                updatei.Add("WarriorPoints", NWP);
                updatemm.Add("MagePoints", NMP);
                updatej.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updateh);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatell);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatei);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatemm);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatej);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "수인\n하드\nEXP : 800 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커1, 전사 3, 마법사 1, 궁수 3";
                break;
            case 4:
                 NEXP = int.Parse(EXP) + 250;
                 NGold = int.Parse(Gold) + Random.Range(245, 265);
                 NTP = int.Parse(TankerPoints) + 1;
                 NMP = int.Parse(MagePoints) + 1;

                Dictionary<string, object> updatek = new Dictionary<string, object>();
                Dictionary<string, object> updatel = new Dictionary<string, object>();
                Dictionary<string, object> updatem = new Dictionary<string, object>();
                updatek.Add("Gold", NGold);
                updatel.Add("TankerPoints", NTP);
                updatem.Add("MagePoints", NMP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatek);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatel);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatem);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "버섯\n이지\nEXP : 250 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 1, 법사 1";
                break;
            case 5:
                 NEXP = int.Parse(EXP) + 500;
                 NGold = int.Parse(Gold) + Random.Range(465, 595);
                 NTP = int.Parse(TankerPoints) + 2;
                 NMP = int.Parse(MagePoints) + 2;

                Dictionary<string, object> updaten = new Dictionary<string, object>();
                Dictionary<string, object> updateo = new Dictionary<string, object>();
                Dictionary<string, object> updatep = new Dictionary<string, object>();
                updaten.Add("Gold", NGold);
                updateo.Add("TankerPoints", NTP);
                updatep.Add("MagePoints", NMP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updaten);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateo);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatep);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "수인\n노말\nEXP : 500 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 2, 법사 2";
                break;
            case 6:
                 NEXP = int.Parse(EXP) + 900;
                 NGold = int.Parse(Gold) + Random.Range(780, 900);
                NTP = int.Parse(TankerPoints) + 3;
                NWP = int.Parse(WarriorPoints) + 1;
                NMP = int.Parse(MagePoints) + 3;
                NAP = int.Parse(ADPoints) + 1;

                Dictionary<string, object> updateq = new Dictionary<string, object>();
                Dictionary<string, object> updater = new Dictionary<string, object>();
                Dictionary<string, object> updates = new Dictionary<string, object>();
                Dictionary<string, object> updatenn = new Dictionary<string, object>();
                Dictionary<string, object> updateoo = new Dictionary<string, object>();
                updateq.Add("Gold", NGold);
                updatenn.Add("TankerPoints", NTP);
                updater.Add("WarriorPoints", NWP);
                updateoo.Add("MagePoints", NMP);
                updates.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updateq);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatenn);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updater);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateoo);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updates);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "버섯\n하드\nEXP : 900 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 3, 전사 1, 마법사 3, 궁수 1";
                break;
                //스님
            case 7:
                 NEXP = int.Parse(EXP) + 300;
                 NGold = int.Parse(Gold) + Random.Range(250, 270);
                 NWP = int.Parse(WarriorPoints) + 1;
                 NMP = int.Parse(MagePoints) + 1;

                Dictionary<string, object> updatet = new Dictionary<string, object>();
                Dictionary<string, object> updateu = new Dictionary<string, object>();
                Dictionary<string, object> updatev = new Dictionary<string, object>();
                updatet.Add("Gold", NGold);
                updateu.Add("WarriorPoints", NWP);
                updatev.Add("MagePoints", NMP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateu);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatev);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "스님\n이지\nEXP : 300 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 전사 1, 법사 1";
                break;
            case 8:
                 NEXP = int.Parse(EXP) + 600;
                 NGold = int.Parse(Gold) + Random.Range(485, 620);
                 NWP = int.Parse(WarriorPoints) + 2;
                NMP = int.Parse(MagePoints) + 2;

                Dictionary<string, object> updatew = new Dictionary<string, object>();
                Dictionary<string, object> updatex = new Dictionary<string, object>();
                Dictionary<string, object> updatey = new Dictionary<string, object>();
                updatew.Add("Gold", NGold);
                updatex.Add("WarriorPoints", NWP);
                updatey.Add("MagePoints", NMP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatew);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatex);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatey);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "스님\n노말\nEXP : 600 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 전사 2, 법사 2";
                break;
            case 9:
                 NEXP = int.Parse(EXP) + 1000;
                 NGold = int.Parse(Gold) + Random.Range(800, 950);
                NTP = int.Parse(TankerPoints) + 1;
                NWP = int.Parse(WarriorPoints) + 3;
                NMP = int.Parse(MagePoints) + 3;

                Dictionary<string, object> updatez = new Dictionary<string, object>();
                Dictionary<string, object> updatepp = new Dictionary<string, object>();
                Dictionary<string, object> updateaa = new Dictionary<string, object>();
                Dictionary<string, object> updatebb = new Dictionary<string, object>();
                updatez.Add("Gold", NGold);
                updatepp.Add("TankerPoints", NTP);
                updateaa.Add("WarriorPoints", NWP);
                updatebb.Add("MagePoints", NMP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatez);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatepp);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateaa);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatebb);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "스님\n하드\nEXP : 1000 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 1, 전사 3, 법사 3, 궁수 1";
                break;
            case 10:
                 NEXP = int.Parse(EXP) + 350;
                 NGold = int.Parse(Gold) + Random.Range(260, 280);
                 NTP = int.Parse(TankerPoints) + 1;
                 NAP = int.Parse(ADPoints) + 1;

                Dictionary<string, object> updatecc = new Dictionary<string, object>();
                Dictionary<string, object> updatedd = new Dictionary<string, object>();
                Dictionary<string, object> updateee = new Dictionary<string, object>();
                updatecc.Add("Gold", NGold);
                updatedd.Add("TankerPoints", NTP);
                updateee.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatecc);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatedd);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateee);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "석상\n이지\nEXP : 350 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 1, 궁수 1";
                break;
            case 11:
                 NEXP = int.Parse(EXP) + 700;
                 NGold = int.Parse(Gold) + Random.Range(520, 700);
                 NTP = int.Parse(TankerPoints) + 2;
                 NAP = int.Parse(ADPoints) + 2;

                Dictionary<string, object> updateff = new Dictionary<string, object>();
                Dictionary<string, object> updategg = new Dictionary<string, object>();
                Dictionary<string, object> updatehh = new Dictionary<string, object>();
                updateff.Add("Gold", NGold);
                updategg.Add("TankerPoints", NTP);
                updatehh.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updateff);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updategg);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatehh);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "석상\n노말\nEXP : 700 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 2, 궁수 2";
                break;
            case 12:
                 NEXP = int.Parse(EXP) + 1200;
                 NGold = int.Parse(Gold) + Random.Range(900, 1100);
                NTP = int.Parse(TankerPoints) + 3;
                NWP = int.Parse(WarriorPoints) + 1;
                NMP = int.Parse(MagePoints) + 1;
                NAP = int.Parse(ADPoints) + 3;

                Dictionary<string, object> updateii = new Dictionary<string, object>();
                Dictionary<string, object> updateqq = new Dictionary<string, object>();
                Dictionary<string, object> updatejj = new Dictionary<string, object>();
                Dictionary<string, object> updaterr = new Dictionary<string, object>();
                Dictionary<string, object> updatekk = new Dictionary<string, object>();
                updateii.Add("Gold", NGold);
                updateqq.Add("TankerPoints", NTP);
                updatejj.Add("WarriorPoints", NWP);
                updaterr.Add("MagePoints", NMP);
                updatekk.Add("ADPoints", NAP);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updateii);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updateqq);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatejj);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updaterr);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatekk);

                if (NEXP >= MAXEXP)
                {
                    NEXP -= MAXEXP;
                    int NLv = int.Parse(Lv) + 1;

                    Dictionary<string, object> update = new Dictionary<string, object>();
                    Dictionary<string, object> updated = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    updated.Add("Level", NLv);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updated);
                }
                else
                {
                    Dictionary<string, object> update = new Dictionary<string, object>();
                    update.Add("EXP", NEXP);
                    reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(update);
                }
                rewardpanel.SetActive(true);
                rewardtext.text = "석상\n하드\nEXP : 1200 획득\n현재 Gold : " + NGold + "\n계열 포인트 : 탱커 3, 전사 1, 법사 1, 궁수 3";
                break;
        }
    }
}
