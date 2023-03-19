using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager loadingManager;

    [SerializeField] GameObject loadingSceneCamera; // use untill other scene has loaded in.
    [SerializeField] Image loadingBackground;
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject ContiuneBTN;

    public void Awake()
    {
        if (loadingManager == null)
        {
            loadingManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSceneAdditive("GameManagers");
        LoadSceneAdditive("MenuMain");
        UnloadScene("GameManagers");
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));   
    }

    public void LoadSceneAdditive(string sceneName, bool waitForPlayer = false)
    {
        DisplayLoadingWindow(waitForPlayer);
        StartCoroutine(LoadSceneAsyncAdditive(sceneName, waitForPlayer));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
       
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.fillAmount = loadingProgress;

            yield return null;
        }
        
    }
    
    IEnumerator LoadSceneAsyncAdditive(string sceneName, bool waitForPlayer)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.fillAmount = loadingProgress;

            yield return null;
            loadingSceneCamera.SetActive(false);

        }

        if (waitForPlayer == false)
        {
            DismissLoadingWindow();
        }else if (waitForPlayer == true)
        {
            ContiuneBTN.SetActive(true);
        }
    }

    public void DisplayLoadingWindow(bool waitForPlayer)
    {
        loadingBackground.enabled = true;
        loadingBar.enabled = true; ;
        ContiuneBTN.SetActive(waitForPlayer);
    }

    public void DismissLoadingWindow()
    {
        loadingBackground.enabled = false;
        loadingBar.enabled = false;
        ContiuneBTN.SetActive(false);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
