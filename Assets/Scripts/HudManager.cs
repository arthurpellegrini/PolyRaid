using System.Collections;
using System.Collections.Generic;
using SDD.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : Manager<HudManager>
{
	[Header("HudManager")]
	#region Labels & Values
	[Header("Game")]
	[SerializeField] private TMP_Text _timer;
	private int minutes;
	private int seconds;
	[SerializeField] private TMP_Text _score;
	[SerializeField] private TMP_Text _health;
	
	[Space(10)]
	[Header("Session")]
	[SerializeField] private TMP_Text _sessionID;
	[SerializeField] private TMP_Text _fps;
	
	[Space(10)]
	[Header("Player")]
	[SerializeField] private TMP_Text _mag;
	[SerializeField] private TMP_Text _munition;
	[SerializeField] private TMP_Text _grenade;
	[SerializeField] private GameObject _weaponGO;
	private Image _weapon;
	[SerializeField] private List<Sprite> _weaponSpriteList;
	[SerializeField] private GameObject _crouching;
	#endregion
	
	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion
	// public override void SubscribeEvents()
	// {
	// 	base.SubscribeEvents();
	// 	EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
	// 	EventManager.Instance.AddListener<SessionStatisticsChangedEvent>(SessionStatisticsChanged);
	// 	EventManager.Instance.AddListener<PlayerStatisticsChangedEvent>(PlayerStatisticsChanged);
	// }
	//
	// public override void UnsubscribeEvents()
	// {
	// 	base.UnsubscribeEvents();
	// 	EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
	// 	EventManager.Instance.RemoveListener<SessionStatisticsChangedEvent>(SessionStatisticsChanged);
	// 	EventManager.Instance.RemoveListener<PlayerStatisticsChangedEvent>(PlayerStatisticsChanged);
	// }

	void RefreshGameUI(float timer, int score, int health)
	{
		minutes = (int) timer / 60000 ;
		seconds = (int) timer / 1000 - 60 * minutes;
		_timer.text = timer.ToString(string.Format("{0:00}:{1:00}", minutes, seconds));
		_score.text = "SCORE:" + score.ToString();
		_health.text = health.ToString();
	}

	void RefreshSessionUI(string sessionID, int fps)
	{
		_sessionID.text = "#" + sessionID;
		_fps.text = "FPS:" + fps.ToString();
	}

	void RefreshPlayerUI(bool isCrouched, Weapon weaponID, int mag, int munition, int grenade)
	{
		_crouching.SetActive(isCrouched);
		_weapon.sprite = _weaponSpriteList[(int) weaponID];
		_mag.text = mag.ToString();
		_munition.text = munition.ToString();
		_grenade.text = grenade.ToString();
	}
	
	#region Callbacks to GameManager events
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		RefreshGameUI(e.eTimer, e.eScore, e.eHealth);
	}
	protected override void SessionStatisticsChanged(SessionStatisticsChangedEvent e)
	{
		RefreshSessionUI(e.eSessionID, e.eFps);
	}
	protected override void PlayerStatisticsChanged(PlayerStatisticsChangedEvent e)
	{
		RefreshPlayerUI(e.eIsCrouching, e.eWeaponID, e.eMag, e.eMunition, e.eGrenade);
	}
	#endregion
}
