using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;


public enum SceneNames { 

    StartScene,
    Gameplay,
    EndScene
}
public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    public SceneController CurrentScene => currentScene;
    public bool IsLoading => isLoading;

    SceneController currentScene;
    bool isLoading;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        EnterCurrentScene();
    }

    void Update()
    {
        if (!isLoading && currentScene != null)
        {
            currentScene.UpdateState();
        }
    }

    public void ChangeScene(SceneNames nextScene)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        StartCoroutine(LoadNextScene(nextScene));
    }

    public void RestartScene()
    {
        if (currentScene != null)
        {
            ChangeScene(currentScene.SceneName);
        }
    }

    IEnumerator LoadNextScene(SceneNames scene)
    {
        try
        {
            if (currentScene != null)
            {
                currentScene.ExitState();
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.inputActions?.Disable();
            }

            AsyncOperation loading = UnitySceneManager.LoadSceneAsync((int)scene);
            currentScene = null;
            yield return loading;

            EnterCurrentScene();
        }
        finally
        {
            isLoading = false;
        }
    }

    void EnterCurrentScene()
    {
        currentScene = FindFirstObjectByType<SceneController>();
        if (currentScene == null) { Debug.LogError("No SceneController"); return; }
        currentScene.EnterState();

    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
