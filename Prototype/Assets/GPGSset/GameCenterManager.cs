using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;


public class GameCenterManager : MonoBehaviour
{
    public string FireBaseId = string.Empty;
    FirebaseAuth auth;

    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .RequestIdToken()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        auth = FirebaseAuth.DefaultInstance;
    }

    public void GoogleLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    StartCoroutine(TryFirebaseLogin());
                }
                else // 실패하면
                {
                    Debug.Log("실패!");
                }
            });
        }
    }

    public void GoogleLogout()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
            auth.SignOut();
        }
    }

    public void GoBattle()
    {
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        string accessToken = null;


        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
}