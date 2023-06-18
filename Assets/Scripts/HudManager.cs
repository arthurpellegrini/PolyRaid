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
	[SerializeField] private TMP_Text _timer;
	private int minutes;
	private int seconds;
	
	[SerializeField] private TMP_Text _score;
	[SerializeField] private TMP_Text _health;
	
	[SerializeField] private TMP_Text _sessionID;
	[SerializeField] private TMP_Text _fps;
	// [SerializeField] private TMP_Text _ping;
	
	[SerializeField] private TMP_Text _mag;
	// [SerializeField] private TMP_Text _munition;
	// [SerializeField] private TMP_Text _grenade;
	// [SerializeField] private GameObject _weaponGO;
	// private Image _weapon;
	// [SerializeField] private List<Sprite> _weaponSpriteList;
	// [SerializeField] private GameObject _crouching;
	
	// NOTE FOR NEXT USAGE : 
	// _crouching.SetActive(isCrouched);
	// _weapon.sprite = _weaponSpriteList[(int) weaponID];
	// _munition.text = munition.ToString();
	// _grenade.text = grenade.ToString();
	#endregion
	
	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	private void RefreshTimerUI(float timer)
	{
		minutes = (int) timer / 60000 ;
		seconds = (int) timer / 1000 - 60 * minutes;
		_timer.text = timer.ToString(string.Format("{0:00}:{1:00}", minutes, seconds));
	}
	void RefreshPlayerHealth(int health) { _health.text = health.ToString(); }
	void RefreshPlayerScore(int score) { _score.text = "SCORE:" + score.ToString(); }
	void RefreshSessionIDUI(string sessionID) { _sessionID.text = "#" + sessionID; }
	void RefreshFpsUI(int fps) { _fps.text = "FPS:" + fps.ToString(); }
	void RefreshMagUI(int mag) { _mag.text = mag.ToString(); }

	#region Callbacks to GameManager events
	protected override void GameTimerChanged(GameTimerChangedEvent e) { RefreshTimerUI(e.eTimer); }
	protected override void PlayerHealthChanged(PlayerHealthChangedEvent e) { RefreshPlayerHealth(e.eHealth); }
	protected override void PlayerScoreChanged(PlayerScoreChangedEvent e) { RefreshPlayerScore(e.eScore); }
	protected override void SessionIDChanged(SessionIDChangedEvent e) { RefreshSessionIDUI(e.eSessionID); }
	protected override void FpsChanged(FpsChangedEvent e) { RefreshFpsUI(e.eFps); }
	protected override void PlayerMagChanged(PlayerMagChangedEvent e) { RefreshMagUI(e.eMag); }
	#endregion
}
