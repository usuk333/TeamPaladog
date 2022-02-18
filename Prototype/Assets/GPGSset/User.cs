using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    // 스레드에 안전한 싱글톤 선언
    private static bool shuttingDown = false;
    private static object Lock = new object();
    private static User instance;

    public static User Instance
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
                    instance = (User)FindObjectOfType(typeof(User));

                    if (instance == null)
                    {
                        var userObject = new GameObject();
                        instance = userObject.AddComponent<User>();
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

    // 게임에서 사용할 유저 정보를 저장해두는 곳
    public userLoginData mainUser;
    // 유저의 인벤토리
    public userGoodsData mainInventory;

    // 유저 정보 클래스
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

    // 유저 재화 보유 클래스
    [System.Serializable]
    public class userGoodsData
    {
        public string uid;
        public int coin;
    }

    // 유저 영수증 정보
    [System.Serializable]
    public class userReceipt
    {
        public string index;
        public string uid;
        public string transaction_id;
        public string product_id;
        public string platform;
        public string price;
        public string date;
        public string givecheck;
    }


    // 서버에서 유저 데이터 받아오기
    /*public void GetUserData(string uid, System.Action callback)
    {
        Server.Instance.GetUserDataDB(uid, callback);
    }

    // 서버에서 유저 인벤 정보 받아오기
    public void GetUserInven(string uid, System.Action callback)
    {
        Server.Instance.GetUserInvenDB(uid, callback);
    }*/
}
