using Unity.Netcode;
using UnityEngine;

public abstract class Singleton<T> : NetworkBehaviour where T:Component {

	private static T _instance;
	public static T Instance => _instance;

	[Header("Singleton")]
	[SerializeField] private bool m_DoNotDestroyGameObjectOnLoad;

	protected virtual void Awake()
	{
		if (_instance != null) Destroy(gameObject);
		else _instance = this as T;

		if (m_DoNotDestroyGameObjectOnLoad) DontDestroyOnLoad(gameObject);
	}
}
