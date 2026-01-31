using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FrostBit
{
    /// <summary>
    /// A wrapper that provides the means to safely serialize Scene Asset References.
    /// </summary>
    [System.Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {

        #region Editor-only Variables

#if UNITY_EDITOR

        // What we use in editor to select the scene
        [SerializeField] private SceneAsset sceneAsset = null;

#endif

        #endregion

        #region Variables

        // This should only ever be set during serialization/deserialization!
        [SerializeField]
        private string scenePath = string.Empty;

        #endregion

        #region Editor-only Properties
#if UNITY_EDITOR


        /// <summary>
        /// Check if the scene is a valid scene asset
        /// </summary>
        private bool IsValidSceneAsset
        {
            get
            {
                // if (sceneAsset == null)
                //     return false;
                // return sceneAsset.GetType().Equals(typeof(SceneAsset));

                //Casted to bool (basically a glorified null-check)
                return sceneAsset;
            }
        }

        /// <summary>
        /// Get or set the scene path. Has different property versions for editor and runtime
        /// </summary>
        public string ScenePath
        {
            get
            {
                // In editor we always use the asset's path
                return GetScenePathFromAsset();
            }

            set
            {
                scenePath = value;
                sceneAsset = GetSceneAssetFromPath();
            }
        }

#endif
        #endregion

        #region Runtime-only Properties
#if !UNITY_EDITOR

        /// <summary>
        /// Runtime version of the property. Get or set the scene path
        /// </summary>
        public string ScenePath
        {
            get
            {
                // At runtime we rely on the stored path value which we assume was serialized correctly at build time.
                // See OnBeforeSerialize and OnAfterDeserialize
                return scenePath;
            }
            set
            {
                scenePath = value;
            }
        }

#endif

        #endregion

        #region Static Methods

        /// <summary>
        /// Implicit string operator
        /// </summary>
        /// <param name="sceneReference"></param>
        public static implicit operator string(SceneReference sceneReference)
        {
            return sceneReference.ScenePath;
        }

        #endregion

        #region Private Methods

        #endregion


        #region Private Editor-only Methods

#if UNITY_EDITOR

        /// <summary>
        /// Get the corresponding scene asset reference
        /// </summary>
        /// <returns></returns>
        private SceneAsset GetSceneAssetFromPath()
        {
            if (string.IsNullOrEmpty(scenePath))
                return null;

            return AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        }

        /// <summary>
        /// Get the sceneAsset's asset path
        /// </summary>
        /// <returns></returns>
        private string GetScenePathFromAsset()
        {
            if (sceneAsset == null)
                return string.Empty;

            return AssetDatabase.GetAssetPath(sceneAsset);
        }

        /// <summary>
        /// Setup before serialization. Tries to set the scenePath based on sceneAsset.
        /// </summary>
        private void HandleBeforeSerialize()
        {
            // Asset is invalid but have Path to try and recover from
            if (!IsValidSceneAsset && !string.IsNullOrEmpty(scenePath))
            {
                sceneAsset = GetSceneAssetFromPath();
                if (sceneAsset == null)
                    scenePath = string.Empty;

                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
            // Asset takes precendence and overwrites Path
            else
            {
                scenePath = GetScenePathFromAsset();
            }

        }

        /// <summary>
        /// Tries to load the scene asset reference after deserialization
        /// </summary>
        private void HandleAfterDeserialize()
        {
            EditorApplication.delayCall -= HandleAfterDeserialize;

            // Asset is invalid but have path to try and recover from
            if (!IsValidSceneAsset && !string.IsNullOrEmpty(scenePath))
            {
                sceneAsset = GetSceneAssetFromPath();

                // No asset found, path was invalid. Make sure we don't carry over the old invalid path
                if (sceneAsset == null)
                    scenePath = string.Empty;

                if (!Application.isPlaying)
                    UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
        }
#endif

        #endregion

        #region ISerializationCallbackReceiver Callbacks

        // Called to prepare this data for serialization. Stubbed out when not in editor.
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            HandleBeforeSerialize();
#endif
        }

        // Called to set up data for deserialization. Stubbed out when not in editor.
        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            // We sadly cannot touch assetdatabase during serialization, so defer by a bit.
            EditorApplication.delayCall += HandleAfterDeserialize;
#endif
        }

        #endregion

    }
}