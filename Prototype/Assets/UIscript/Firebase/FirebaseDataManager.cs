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
using TMPro;

public class FirebaseDataManager : MonoBehaviour
{
    [SerializeField] private string Userid;
    //NaIxowYCsaSqdaYaWtWbIYErkqM2

    private DatabaseReference reference;

    DataSnapshot Unitsnapshot;

    DataSnapshot Skillsnapshot;

    void Awake()
    {
        reference.Child("users").Child(Userid).Child("Unit").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("ÄÄÇÃ¸´¶¸À½");
                Unitsnapshot = task.Result;

                StartCoroutine(InitData());
            }
        });

        reference.Child("users").Child(Userid).Child("Skill").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("ÄÄÇÃ¸´¶¸À½");
                Skillsnapshot = task.Result;

                StartCoroutine(InitData());
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator InitData()
    {
        yield return null;

        if (Unitsnapshot == null)
        {
            Start();
        }
        Debug.Log("Data ¼³Ä¡");

        //Unitpoints = snapshot.Child("UnitPoints").Value.ToString();

        //snapshot.Child("Unit")
    }
}
