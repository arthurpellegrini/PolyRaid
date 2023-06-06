using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:Component {

	static T _instance;
	public static T Instance => _instance;

	[Header("Singleton")]
	[SerializeField]
	private bool m_DoNotDestroyGameObjectOnLoad;

	protected virtual void Awake()
	{
		if (_instance != null)
			Destroy(gameObject);
		else _instance = this as T;

		if (m_DoNotDestroyGameObjectOnLoad)
			DontDestroyOnLoad(gameObject);
	}
}
