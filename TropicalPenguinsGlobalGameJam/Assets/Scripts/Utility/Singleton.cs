using UnityEngine;

// Be aware this will not prevent a non singleton constructor
//   such as `T myT = new T();`
// To prevent that, add `protected T () {}` to your singleton class.
// 
// As a note, this is made as MonoBehaviour because we need Coroutines.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	[SerializeField] bool dontDestroyOnLoad;

	protected static T _instance;
	protected static object _lock = new object();

	protected void SetInstance(T instance)
	{
		_instance = instance;
	}

	public static T Instance
	{
		get
		{
			_instance = (T)FindObjectOfType(typeof(T));
			if (FindObjectsOfType(typeof(T)).Length > 1)
			{
				Debug.LogError("[Singleton] Something went really wrong " +
								" - there should never be more than 1 singleton!" +
								" Reopenning the scene might fix it.");
			}
			return _instance;
			//if (applicationIsQuitting)
			//{
			//    Debug.LogWarning("[Singleton] Instance '" + typeof (T) +
			//                        "' already destroyed on application quit." +
			//                        " Won't create again - returning null.");

			//    return null;
			//}

			//lock (_lock)
			//{
			//    if (_instance == null)
			//    {
			//        _instance = (T) FindObjectOfType(typeof (T));

			//        if (FindObjectsOfType(typeof (T)).Length > 1)
			//        {
			//            Debug.LogError("[Singleton] Something went really wrong " +
			//                            " - there should never be more than 1 singleton!" +
			//                            " Reopenning the scene might fix it.");
			//            return _instance;
			//        }

			//        if (_instance == null)
			//        {
			//            if (Application.isPlaying)
			//            {
			//                /*
			//                GameObject singleton = new GameObject();
			//                _instance = singleton.AddComponent<T>();
			//                singleton.name = "(singleton) " + typeof (T).ToString();
			//                */
			//                //DontDestroyOnLoad(singleton);

			//                Debug.LogError("[Singleton] An instance of " + typeof (T) + " is needed in the scene.");
			//            }
			//        }
			//        else
			//        {
			//            // Using instance as well

			//            // Debug.Log("[Singleton] Using instance already created: " +
			//            //           _instance.gameObject.name);
			//        }
			//    }

			//    return _instance;
			//}
		}
	}

	static bool applicationIsQuitting = false;

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	public virtual void OnDestroy()
	{
		applicationIsQuitting = true;
	}

	protected virtual void Awake()
	{
		if (dontDestroyOnLoad)
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}