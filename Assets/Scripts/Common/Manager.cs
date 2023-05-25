using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : SingletonGameStateObserver<T> where T:Component{

	protected bool m_IsReady = false;
	public bool IsReady { get { return m_IsReady; } }

	protected abstract IEnumerator InitCoroutine();

	// Use this for initialization
	protected virtual IEnumerator Start () {
		m_IsReady = false;
		yield return StartCoroutine(InitCoroutine());
		m_IsReady = true;
	}
}
