using UnityEngine;

namespace C_Script.Base.Template
{
    public class NormSingleton<T> :MonoBehaviour where T :NormSingleton<T>
    { 
        [SerializeField] private bool isSaveNextScene; 
        private static T instance;
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
            if (isSaveNextScene)
                DontDestroyOnLoad(gameObject);
        }
    }
}