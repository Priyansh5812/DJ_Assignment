using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SceneManagement : MonoBehaviour
{
    private static SceneManagement _instance;
    public static SceneManagement Instance
    {
        get
        {
            return _instance;
        }
    }

    public event Action OnActivateLoadedScene;

    public AsyncOperation op { get; private set; }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void OnEnable()
    {
        OnActivateLoadedScene += AllowActiveSceneTrigger;
    }


    public void ChangeScene(int sceneNum)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNum);
    }

    public void ChangeSceneAsync(int sceneNum)
    {
        if (op != null) // If there is an existing loaded scene
        {
            Debug.LogError("Already a Scene is Queued for Activation.\n Consider trigger it first");
            return;
        }

        StartCoroutine(ChangeSceneCoroutine(sceneNum));
    }

    // todo : Async Function from Coroutine
    private IEnumerator ChangeSceneCoroutine(int sceneNum)
    {
        Debug.Log("Scene Loading Started");
        op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneNum);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("Async Scene Load Completed " + sceneNum);
        yield return null;
    }

    private void OnDisable()
    {
        OnActivateLoadedScene -= AllowActiveSceneTrigger;
    }

    public void TriggerLoadedScene()
    {
        OnActivateLoadedScene.Invoke();
    }

    private void AllowActiveSceneTrigger()
    {
        //UnityEngine.SceneManagement.SceneManager.

        if (op != null)
        {
            op.allowSceneActivation = true;
            op = null;
        }
        else
        {
            Debug.LogError("Illegal Memory Access for a Scene Activation");
        }
    }
}
