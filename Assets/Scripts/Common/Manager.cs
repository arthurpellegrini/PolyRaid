using System.Collections;
using UnityEngine;

public abstract class Manager<T> : SingletonGameStateObserver<T> where T:Component{

	protected bool m_IsReady = false;
	public bool IsReady => m_IsReady;

	protected abstract IEnumerator InitCoroutine();
	
	protected virtual IEnumerator Start () {
		m_IsReady = false;
		yield return StartCoroutine(InitCoroutine());
		m_IsReady = true;
	}
}
