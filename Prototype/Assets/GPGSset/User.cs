using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    // �����忡 ������ �̱��� ����
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
        public string index;
        public string uid;
        public string transaction_id;
        public string product_id;
        public string platform;
        public string price;
        public string date;
        public string givecheck;
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
