using System;
using System.Collections;
using System.Collections.Generic;
using SDD.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class HudManager : Manager<HudManager>
{

	[Header("HudManager")]
	#region Labels & Values
	[Header("Game Statistics")]
	[SerializeField] private TMP_Text _timer;
	private int minutes;
	private int seconds;
	[SerializeField] private TMP_Text _score;
	[SerializeField] private TMP_Text _health;
	[SerializeField] private TMP_Text _fps;
	[SerializeField] private TMP_Text _ping;
	
	[Space(10)]
	[Header("Player Statistics")]
	[SerializeField] private TMP_Text _mag;
	[SerializeField] private TMP_Text _munition;
	[SerializeField] private TMP_Text _grenade;
	[SerializeField] private GameObject _weaponGO;
	[SerializeField] private List<Sprite> _weaponSpriteList;
	private Image _weapon;
	[SerializeField] private GameObject _crouching;



	#endregion
	
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
		EventManager.Instance.AddListener<PlayerStatisticsChangedEvent>(PlayerStatisticsChanged);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
		EventManager.Instance.RemoveListener<PlayerStatisticsChangedEvent>(PlayerStatisticsChanged);
	}

	void RefreshGameUI(float timer, int score, int health, int fps, int ping)
	{
		minutes = (int) timer / 60000 ;
		seconds = (int) timer / 1000 - 60 * minutes;
		_timer.text = timer.ToString(string.Format("{0:00}:{1:00}", minutes, seconds));
		_score.text = "SCORE" + score.ToString();
		_health.text = health.ToString();
		_fps.text = "FPS:" + fps.ToString();
		_ping.text = "PING:" + ping.ToString();
	}

	void RefreshPlayerUI(bool isCrouched, Weapon weaponID, int mag, int munition, int grenade)
	{
		_crouching.SetActive(isCrouched);
		_weapon.sprite = _weaponSpriteList[(int) weaponID];
		_mag.text = mag.ToString();
		_munition.text = munition.ToString();
		_grenade.text = grenade.ToString();
	}
	

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Callbacks to GameManager events

	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		RefreshGameUI(e.eTimer, e.eScore, e.eHealth, e.eFps, e.ePing);
	}

	protected override void PlayerStatisticsChanged(PlayerStatisticsChangedEvent e)
	{
		RefreshPlayerUI(e.eIsCrouching, e.eWeaponID, e.eMag, e.eMunition, e.eGrenade);
	}
	#endregion

}