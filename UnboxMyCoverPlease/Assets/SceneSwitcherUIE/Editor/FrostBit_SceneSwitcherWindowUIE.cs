using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.IO;
using UnityEditor.SceneManagement;

public class FrostBit_SceneSwitcherWindowUIE : EditorWindow
{

    VisualElement root;

    [System.Serializable]
    public class FavoriteScenes
    {
        public List<string> scenes = new List<string>();
    }

    private FavoriteScenes favorites = new FavoriteScenes();

    private const string FAVORITE_SCENES_PREF_KEY = "SceneSwitcherWindowUIE.FavoriteScenes";

    VisualElement favoriteScenesContainer;
    VisualElement buildScenesContainer;
    VisualElement allScenesContainer;

    ToolbarSearchField filterField;
    ToolbarToggle editModeToggle;


    [MenuItem("Tools/Scene Switcher UIE")]
    public static void ShowWindow()
    {
        var window = GetWindow<FrostBit_SceneSwitcherWindowUIE>();

        window.titleContent = new GUIContent("Scene Switcher");
    }

    private void OnEnable()
    {
        LoadPrefs();

        root = rootVisualElement;

        root.styleSheets.Add(Resources.Load<StyleSheet>("SceneSwitcher_Style"));

        if (EditorGUIUtility.isProSkin)
        {
            root.AddToClassList("dark-theme");
        }

        VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("SceneSwitcher_Main");
        visualTree.CloneTree(root);
        
        //These two lines need be here to make root element focusable (so the KeyUpEvent fires)
        root.focusable = true;
        root.pickingMode = PickingMode.Position;
        root.RegisterCallback<KeyUpEvent>(OnKeyUpEvent);

        ToggleEditMode(false);

        filterField = root.Q<ToolbarSearchField>("sw-toolbar__filter");
        filterField.RegisterValueChangedCallback(OnFilterChanged);

        editModeToggle = root.Q<ToolbarToggle>("sw-toolbar__editmode-toggle");
        editModeToggle.value = false;
        editModeToggle.RegisterValueChangedCallback(OnEditModeToggleChange);

        var refreshButton = root.Q<ToolbarButton>("sw-toolbar__refresh-btn");

        refreshButton.clicked += () =>
        {
            RefreshLists();
        };

        var createButton = root.Q<ToolbarButton>("sw-toolbar__create-btn");
        createButton.clicked += () => {
            CreateNewScene();
        };

        favoriteScenesContainer = root.Q<VisualElement>("favorite-scenes");
        buildScenesContainer = root.Q<VisualElement>("build-scenes");
        allScenesContainer = root.Q<VisualElement>("all-scenes");

        RefreshLists();
    }

    /// <summary>
    /// Toggle edit mode on or off
    /// </summary>
    /// <param name="isOn"></param>
    private void ToggleEditMode(bool isOn)
    {       
        if (isOn)
        {
            root.AddToClassList("edit-mode");
            root.RemoveFromClassList("normal-mode");
        }
        else
        {
            root.RemoveFromClassList("edit-mode");
            root.AddToClassList("normal-mode");
        }
    }

    /// <summary>
    /// Refresh all the scene lists
    /// </summary>
    private void RefreshLists()
    {
        Texture2D tex = (Texture2D)EditorGUIUtility.Load("d_Favorite");

        VisualTreeAsset sceneRowTemplate = Resources.Load<VisualTreeAsset>("SceneRow");

        //Favorite scenes

        string[] favoriteScenes = favorites.scenes.ToArray();
        CreateSceneElementsToContainer(favoriteScenes, favoriteScenesContainer, sceneRowTemplate, true);

        //Scenes in build
        string[] buildSceneGuids = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) 
        {
            buildSceneGuids[i] = EditorBuildSettings.scenes[i].guid.ToString();
        }

        CreateSceneElementsToContainer(buildSceneGuids, buildScenesContainer, sceneRowTemplate);


        //All scenes
        string[] guids = AssetDatabase.FindAssets("t:Scene");

        CreateSceneElementsToContainer(guids, allScenesContainer, sceneRowTemplate);

        if (filterField != null) {
            string filterText = filterField.value;
            if (!string.IsNullOrWhiteSpace(filterText)) {
                FilterScenesLists(filterText);
            }
        }
    }

    /// <summary>
    /// Create scene element rows to parentContainer
    /// </summary>
    /// <param name="guids">Scene guids</param>
    /// <param name="parentContainer">Container to add created scene rows</param>
    /// <param name="sceneRowTemplate">Scene row template</param>
    /// <param name="isFavorites">Are these the favorite scenes</param>
    private void CreateSceneElementsToContainer(string[] guids, VisualElement parentContainer, VisualTreeAsset sceneRowTemplate, bool isFavorites = false) {
        parentContainer.Clear();

        Texture2D tex = (Texture2D)EditorGUIUtility.Load("d_Favorite");

        for (int i = 0; i < guids.Length; i++)
        {
            string sceneGuid = guids[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            string sceneName = Path.GetFileNameWithoutExtension(assetPath);

            VisualElement sceneRow = sceneRowTemplate.CloneTree();
            sceneRow.userData = sceneName;
            sceneRow.name = sceneName;

            var favoriteButton = sceneRow.Q<Button>("favorite-scene-btn");

            favoriteButton.clicked += () =>
            {
                ToggleFavorite(sceneGuid);
                RefreshLists();
            };

            var favoriteButtonIcon = favoriteButton.Q<VisualElement>("scene-row-favorite-btn-icon");

            favoriteButtonIcon.style.backgroundImage = tex;

            if (IsFavorite(sceneGuid))
            {
                favoriteButton.tooltip = "Remove from favorites";
                favoriteButtonIcon.AddToClassList("favorited");
            }
            else
            {
                favoriteButton.tooltip = "Add to favorites";
                favoriteButtonIcon.RemoveFromClassList("favorited");
            }

            var loadSceneButton = sceneRow.Q<Button>("load-scene-btn");

            loadSceneButton.clicked += () =>
            {
                LoadScene(assetPath);
            };

            loadSceneButton.text = sceneName;

            var deleteSceneButton = sceneRow.Q<ToolbarButton>("remove-scene");

            deleteSceneButton.clicked += () => {
                DeleteScene(assetPath);
            };

            var renameTextField = sceneRow.Q<TextField>("scene-rename-textfield");
            renameTextField.isDelayed = true;
            renameTextField.value = sceneName;
            renameTextField.RegisterCallback<ChangeEvent<string>, string>(OnRenameTextFieldChange, assetPath);

            if (isFavorites) {
                var moveUpBtn = sceneRow.Q<ToolbarButton>("favorite-up");
                moveUpBtn.clicked += () =>
                {
                    MoveFavoriteUp(sceneGuid);
                    RefreshLists();
                };

                var moveDownBtn = sceneRow.Q<ToolbarButton>("favorite-down");
                moveDownBtn.clicked += () =>
                {
                    MoveFavoriteDown(sceneGuid);
                    RefreshLists();
                };
            }

            parentContainer.Add(sceneRow);
        }
    }

    /// <summary>
    /// Event handler for keyup event
    /// </summary>
    /// <param name="evt"></param>
    private void OnKeyUpEvent(KeyUpEvent evt)
    {
        bool stopEvent = true;

        if (evt.keyCode == KeyCode.F && evt.ctrlKey) {
            FocusSearchField();
        } else if (evt.keyCode == KeyCode.R && evt.ctrlKey) {
            ForceRefresh();
        } else if (evt.keyCode == KeyCode.E && evt.ctrlKey) {
            if (editModeToggle != null)
                editModeToggle.value = !editModeToggle.value;
        } else {
            stopEvent = false;
        }

        if (stopEvent) {
            evt.StopPropagation();
        }
    }

    /// <summary>
    /// Event handler for edit mode toggle change
    /// </summary>
    /// <param name="evt"></param>
    private void OnEditModeToggleChange(ChangeEvent<bool> evt)
    {
        ToggleEditMode(evt.newValue);

    }

    /// <summary>
    /// Event handler for scene row rename text field change
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="sceneAssetPath"></param>
    private void OnRenameTextFieldChange(ChangeEvent<string> evt, string sceneAssetPath)
    {
        RenameScene(sceneAssetPath, evt.newValue);
        RefreshLists();
    }

    /// <summary>
    /// Event handler for filter text change
    /// </summary>
    /// <param name="evt"></param>
    public void OnFilterChanged(ChangeEvent<string> evt)
    {
        string filterText = evt.newValue.ToLower();

        FilterScenesLists(filterText);
    }

    /// <summary>
    /// Filter all scenes lists based on filter text
    /// </summary>
    /// <param name="filterText"></param>
    public void FilterScenesLists(string filterText) {
        FilterScenesList(favoriteScenesContainer, filterText);
        FilterScenesList(buildScenesContainer, filterText);
        FilterScenesList(allScenesContainer, filterText);
    }

    /// <summary>
    /// Filter the scene row children of the parentContainer by filterText
    /// </summary>
    /// <param name="parentContainer"></param>
    /// <param name="filterText"></param>
    public void FilterScenesList(VisualElement parentContainer, string filterText)
    {
        foreach (var scene in parentContainer.Children())
        {
            if (string.IsNullOrWhiteSpace(filterText) || ((scene.userData as string).ToLower().Contains(filterText)))
            {
                scene.RemoveFromClassList("scene-row--hidden");
            }
            else
            {
                scene.AddToClassList("scene-row--hidden");
            }
        }
    }

    /// <summary>
    /// Force refresh
    /// </summary>
    private void ForceRefresh() {
        RefreshLists();
    }

    /// <summary>
    /// Moves focus to the search field
    /// </summary>
    private void FocusSearchField() {
        if (filterField != null) {
            filterField.Q("unity-text-input")?.Focus();
        }
    }

    /// <summary>
    /// Starts new scene creation process
    /// </summary>
    private void CreateNewScene() {
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        if (EditorSceneManager.SaveScene(scene)) {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshLists();
        }
    }

    /// <summary>
    /// Load a scene in the editor
    /// </summary>
    /// <param name="scenePath"></param>
    public void LoadScene(string scenePath)
    {
        if (scenePath != "") {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorSceneManager.OpenScene(scenePath);
            }
        }

        Debug.Log($"Loading scene with guid {scenePath}");
    }

    /// <summary>
    /// Delete a scene asset
    /// </summary>
    /// <param name="assetPath"></param>
    public void DeleteScene(string assetPath)
    {
        if (EditorUtility.DisplayDialog("Deletion confirmation", $"Are you sure you want to delete scene at {assetPath}", "Yes, delete", "No, do not delete"))
        {
            string guid = AssetDatabase.AssetPathToGUID(assetPath);

            bool wasRemoved = AssetDatabase.DeleteAsset(assetPath);
            if (wasRemoved)
            {
                Debug.Log($"Deleted scene at {assetPath}");
                if (IsFavorite(guid))
                {
                    ToggleFavorite(guid);
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                ShowNotification(new GUIContent("Deleted scene"));
                RefreshLists();
            }
            else
            {
                Debug.LogError($"Failed to delete scene at {assetPath}");
                ShowNotification(new GUIContent("Failed to delete scene"));
            }
        }
    }

    /// <summary>
    /// Rename a scene asset
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="newSceneName"></param>
    public void RenameScene(string assetPath, string newSceneName)
    {
        string statusMessage = AssetDatabase.RenameAsset(assetPath, newSceneName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (statusMessage == "")
        {
            //Everything went ok
            Debug.Log($"Renamed {assetPath} to {newSceneName}");
            ShowNotification(new GUIContent("Renamed scene"));
        }
        else
        {
            //There was an error
            Debug.LogError($"Error renaming scene: {statusMessage}");
            ShowNotification(new GUIContent("Error renaming scene"));
        }
    }

    /// <summary>
    /// Moves the favorite one place up in the favorites list
    /// </summary>
    /// <param name="guid"></param>
    private void MoveFavoriteUp(string guid)
    {
        int indexOfFavorite = favorites.scenes.IndexOf(guid);

        //If the favorite was in the list and was not the first one on the list
        if (indexOfFavorite != -1 && indexOfFavorite != 0)
        {
            favorites.scenes.RemoveAt(indexOfFavorite);
            favorites.scenes.Insert(indexOfFavorite - 1, guid);
        }
        SaveFavoritesToPrefs();
    }

    /// <summary>
    /// Moves the favorite one place down in the favorites list
    /// </summary>
    /// <param name="guid"></param>
    private void MoveFavoriteDown(string guid)
    {
        int indexOfFavorite = favorites.scenes.IndexOf(guid);

        //If the favorite was in the list and was not the last one on the list
        if (indexOfFavorite != -1 && indexOfFavorite != favorites.scenes.Count - 1)
        {
            favorites.scenes.RemoveAt(indexOfFavorite);
            favorites.scenes.Insert(indexOfFavorite + 1, guid);
        }

        SaveFavoritesToPrefs();
    }

    /// <summary>
    /// Checks if a guid is favorited
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>Is favorite</returns>
    private bool IsFavorite(string guid) {
        return favorites != null && favorites.scenes != null && favorites.scenes.IndexOf(guid) != -1;
    }

    /// <summary>
    /// Toggle favorite status of scene
    /// </summary>
    /// <param name="guid"></param>
    private void ToggleFavorite(string guid)
    {
        int index = favorites.scenes.IndexOf(guid);

        if (index != -1)
        {
            favorites.scenes.RemoveAt(index);
        }
        else
        {
            favorites.scenes.Add(guid);
        }

        SaveFavoritesToPrefs();
    }

    /// <summary>
    /// Save favorites and user preferences
    /// </summary>
    private void SavePrefs()
    {
        SaveFavoritesToPrefs();
    }

    /// <summary>
    /// Saves favorites to prefs
    /// </summary>
    private void SaveFavoritesToPrefs()
    {
        if (favorites != null && favorites.scenes.Count > 0)
        {
            string favoritesAsJson = EditorJsonUtility.ToJson(favorites);
            PlayerPrefs.SetString(FAVORITE_SCENES_PREF_KEY, favoritesAsJson);
        }
        else
        {
            PlayerPrefs.DeleteKey(FAVORITE_SCENES_PREF_KEY);
        }
    }

    /// <summary>
    /// Load favorites and window preferences
    /// </summary>
    private void LoadPrefs()
    {
        if (PlayerPrefs.HasKey(FAVORITE_SCENES_PREF_KEY))
        {
            string favoritesAsJson = PlayerPrefs.GetString(FAVORITE_SCENES_PREF_KEY);

            if (favoritesAsJson != "")
            {
                try
                {
                    EditorJsonUtility.FromJsonOverwrite(favoritesAsJson, favorites);
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }

    private void OnLostFocus()
    {
        SavePrefs();
    }
}
