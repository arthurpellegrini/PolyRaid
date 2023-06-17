using UnityEngine;
using System.Collections.Generic;

public class CustomTimer : MonoBehaviour {

	//static
	static Dictionary<string, CustomTimer> dicoCustomTimers;
	public static CustomTimer GetCustomTimer(string name)
	{
		CustomTimer customTimer = null;
		dicoCustomTimers.TryGetValue(name, out customTimer);
		return customTimer;
	}

	public bool IsRunning{get;private set;}

	float m_Time=0;

	private float m_TimeScale;

	public float TimeScale 
	{
		get => m_TimeScale;
		set => m_TimeScale = value;
	}
	public float DeltaTime => IsRunning ? UnityEngine.Time.deltaTime* m_TimeScale : 0;

	public float FixedDeltaTime => IsRunning ? UnityEngine.Time.fixedDeltaTime* m_TimeScale : 0;


	public float Time
	{
		get => m_Time;
		private set => m_Time = value;
	}

	public void StopTimer()
	{
		IsRunning = false;
	}

	public void StartTimer()
	{
		IsRunning = true;
	}

	public void Reset()
	{
		Time = 0;
	}

	public void Reset(bool startTimer)
	{
		Time = 0;
		IsRunning = startTimer;
	}

	public void ResetAndStart()
	{
		Reset (true);
	}

	public void ResetAndStop()
	{
		Reset (false);
	}

	private void Awake()
	{
		if(dicoCustomTimers==null)
		{
			dicoCustomTimers = new Dictionary<string, CustomTimer>();
			CustomTimer[] customTimers = GameObject.FindObjectsOfType<CustomTimer>();
			foreach (var item in customTimers)
			{
				dicoCustomTimers.Add(item.name, item);
			}
		}
	}

	
	void Start () {
		Reset(false);
	}
	
	void Update () {
		Time+=DeltaTime;
	}
}
