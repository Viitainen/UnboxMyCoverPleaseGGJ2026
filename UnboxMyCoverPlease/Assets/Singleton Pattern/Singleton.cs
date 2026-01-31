using System;
using UnityEngine;

namespace FrostBit
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        #region Variables

        private bool isPersistent = true;

        public static bool IsInstantiated { get; private set; }

        public static bool IsDestroyed { get; private set; }

        #endregion

        #region Properties

        public bool IsPersistent { get { return isPersistent; } set { isPersistent = value; } }

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!IsInstantiated) CreateInstance();
                return _instance;
            }
        }

        #endregion

        private static void CreateInstance()
        {
            if (IsDestroyed) return;

            var type = typeof(T);
            var objects = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (objects.Length > 0)
            {
                if (objects.Length > 1)
                {
                    Debug.LogWarning("There is more than one instance of Singleton of type \"" + type +
                                        "\". Keeping the first one. Destroying the others.");
                    for (var i = 1; i < objects.Length; i++) Destroy(objects[i].gameObject);
                }

                _instance = objects[0];
                _instance.gameObject.SetActive(true);

                IsInstantiated = true;
                IsDestroyed = false;
                return;
            }

            string prefabName = type.ToString();
            GameObject gameObject = null;

            //If the singleton class is marked with SingletonPrefabAttribute-attribute, fetch the name from it
            SingletonPrefabAttribute attribute = Attribute.GetCustomAttribute(type, typeof(SingletonPrefabAttribute)) as SingletonPrefabAttribute;


            if (attribute != null && !string.IsNullOrEmpty(attribute.prefabName))
            {
                prefabName = attribute.prefabName;

                gameObject = Instantiate(Resources.Load<GameObject>(prefabName));

                if (gameObject == null)
                {
                    Debug.LogError($"Could not find Prefab for \"{prefabName}\" from the Resources for {type}-singleton");
                }
            }

            if (gameObject == null)
            {
                gameObject = new GameObject();
            }

            gameObject.name = prefabName;

            if (_instance == null)
                _instance = gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();

            IsInstantiated = true;
            IsDestroyed = false;
        }


        #region Inherited Methods

        protected virtual void Reset()
        {
#if UNITY_EDITOR

            var objects = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (objects.Length > 1)
            {
                Debug.LogWarning("There is more than one instance of Singleton of type \"" + typeof(T));
            }
#endif
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                if (IsPersistent)
                {
                    CreateInstance();
                    DontDestroyOnLoad(gameObject);
                }
                return;
            }
            if (GetInstanceID() != _instance.GetInstanceID()) Destroy(gameObject);
            if (IsPersistent) DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            IsDestroyed = true;
            IsInstantiated = false;
        }

        #endregion
    }
}
