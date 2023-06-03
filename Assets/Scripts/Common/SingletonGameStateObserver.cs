using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;

public abstract class SingletonGameStateObserver<T> :  Singleton<T>,IEventHandler where T:Component
{
	public virtual void SubscribeEvents()
	{
		EventManager.Instance.AddListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.AddListener<GameCreditsEvent>(GameCredits);
		EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.AddListener<GamePauseEvent>(GamePause);
		EventManager.Instance.AddListener<GameResumeEvent>(GameResume);
		EventManager.Instance.AddListener<GameRespawnEvent>(GameRespawn);
		EventManager.Instance.AddListener<GameDeadEvent>(GameDead);
		EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
	}

	public virtual void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<GameMenuEvent>(GameMenu);
		EventManager.Instance.RemoveListener<GameCreditsEvent>(GameCredits);
		EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
		EventManager.Instance.RemoveListener<GamePauseEvent>(GamePause);
		EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);
		EventManager.Instance.RemoveListener<GameRespawnEvent>(GameRespawn);
		EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
	}

	protected override void Awake()
	{
		base.Awake();
		SubscribeEvents();
	}

	protected virtual void OnDestroy()
	{
		UnsubscribeEvents();
	}

	protected virtual void GameMenu(GameMenuEvent e)
	{
	}
	
	protected virtual void GameCredits(GameCreditsEvent e)
	{
	}

	protected virtual void GamePlay(GamePlayEvent e)
	{ 
	}

	protected virtual void GamePause(GamePauseEvent e)
	{
	}

	protected virtual void GameResume(GameResumeEvent e)
	{
	}
	
	protected virtual void GameRespawn(GameRespawnEvent e)
	{
	}
	
	protected virtual void GameDead(GameDeadEvent e)
	{
	}

	protected virtual void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
	}
}
