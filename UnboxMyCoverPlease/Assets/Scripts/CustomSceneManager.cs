using System;
using System.Collections;
using System.Collections.Generic;
using FrostBit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : Singleton<CustomSceneManager>
{

    [SerializeField]
    private SceneReference baseScene;

    [SerializeField]
    private SceneReference gameScene;

    [SerializeField]
    private SceneReference mainMenuScene;

    public event Action OnGameSceneLoaded;
    public event Action OnMainMenuSceneLoaded;

    private bool isLoadingMainMenuScene = false;

    private bool isLoadingDungeonScene = false;

    private bool isLoadingGameScene = false;

    AsyncOperation dungeonLoad;
    protected override void Awake()
    {
        isLoadingDungeonScene = false;
        isLoadingMainMenuScene = false;
        isLoadingGameScene = false;

        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // LoadGameScene();
        LoadMainMenuScene();
    }



    public void RestartGame()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseScene.ScenePath));

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(gameScene.ScenePath);

        if (unloadAsync != null)
        {
            unloadAsync.completed += OnRestartGameUnloadCompleted;
        }
        else
        {
            LoadGameScene();
        }
    }

    private void OnRestartGameUnloadCompleted(AsyncOperation operation)
    {
        LoadMainMenuScene();
    }

    public AsyncOperation LoadMainMenuScene()
    {
        if (isLoadingMainMenuScene) return null;

        isLoadingMainMenuScene = true;

        if (IsSceneLoaded(gameScene))
        {
            AsyncOperation unloadGameSceneOperation = SceneManager.UnloadSceneAsync(gameScene.ScenePath);
        }

        if (!IsSceneLoaded(mainMenuScene))
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(mainMenuScene.ScenePath, LoadSceneMode.Additive);

            if (asyncOperation != null)
            {
                asyncOperation.completed += OnMainMenuSceneLoadedHandler;
            }

            return asyncOperation;
        }

        return null;

    }

    public AsyncOperation LoadGameScene()
    {
        if (isLoadingGameScene) return null;

        AsyncOperation loadOp = null;

        isLoadingGameScene = true;

        if (IsSceneLoaded(mainMenuScene))
        {
            AsyncOperation unloadMainMenuSceneOperation = SceneManager.UnloadSceneAsync(mainMenuScene.ScenePath);
        }

        if (!IsSceneLoaded(gameScene))
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(gameScene.ScenePath, LoadSceneMode.Additive);

            loadOp = asyncOperation;

            if (asyncOperation != null)
            {
                asyncOperation.completed += OnGameSceneLoadedHandler;
            }
        }

        return loadOp;
    }

    public AsyncOperation LoadDungeon(SceneReference sceneRef)
    {
        if (isLoadingDungeonScene) return null;


        isLoadingDungeonScene = true;
        dungeonLoad = LoadSceneAsync(sceneRef);
        dungeonLoad.completed += (AsyncOperation op) =>
        {
            OnDungeonSceneLoadedHandler(op);
        };



        return dungeonLoad;
    }

    private void OnDungeonSceneLoadedHandler(AsyncOperation operation)
    {
        isLoadingDungeonScene = false;
    }

    public AsyncOperation UnloadDungeon(SceneReference sceneRef)
    {
        return UnloadSceneAsync(sceneRef);
    }

    private void OnGameSceneLoadedHandler(AsyncOperation operation)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(gameScene.ScenePath));
        isLoadingGameScene = false;
        OnGameSceneLoaded?.Invoke();
    }

    private void OnMainMenuSceneLoadedHandler(AsyncOperation operation)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(mainMenuScene.ScenePath));
        isLoadingMainMenuScene = false;
        OnMainMenuSceneLoaded?.Invoke();
    }

    public AsyncOperation LoadSceneAsync(SceneReference sceneRef)
    {
        return SceneManager.LoadSceneAsync(sceneRef.ScenePath, LoadSceneMode.Additive);
    }

    public AsyncOperation UnloadSceneAsync(SceneReference sceneRef)
    {
        return SceneManager.UnloadSceneAsync(sceneRef.ScenePath);
    }

    public bool IsSceneLoaded(SceneReference sceneRef)
    {
        return IsSceneLoaded(sceneRef.ScenePath);
    }

    public bool IsSceneLoaded(string scenePath)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            // Debug.Log($"{scene.path} == {scenePath}");

            if (scene.path == scenePath && scene.isLoaded)
            {
                return true;
            }
        }
        return false;
    }
}
