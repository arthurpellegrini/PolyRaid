using UnityEngine;

#region GameManager Events
public class GameMainMenuEvent : SDD.Events.Event { }
public class GameCreditsEvent : SDD.Events.Event { }
public class GameCreateSessionEvent : SDD.Events.Event { }
public class GameJoinSessionEvent : SDD.Events.Event { }
public class GameResumeEvent : SDD.Events.Event { }
public class GamePausedEvent : SDD.Events.Event { }
public class GameOverEvent : SDD.Events.Event { }
public class GameChangeMapEvent : SDD.Events.Event { }
public class GameErrorEvent : SDD.Events.Event
{
	public string eErrorTitle { get; set; }
	public string eErrorDescription { get; set; }
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	// public int eKill { get; set; }
	// public int eDead { get; set; }
	public int eScore { get; set; }
	public float eTimer { get; set; }
	public int eHealth { get; set; }
}
public class SessionStatisticsChangedEvent : SDD.Events.Event
{
	public string eSessionID { get; set; }
	public int eFps { get; set; }
}

public class PlayerStatisticsChangedEvent : SDD.Events.Event
{
	public bool eIsCrouching { get; set; }
	public Weapon eWeaponID { get; set; }
	public int eMag { get; set; }
	public int eMunition { get; set; }
	public int eGrenade { get; set; }
}
#endregion

#region MenuManager Events
public class MainMenuButtonClickedEvent : SDD.Events.Event { }
public class CreditsButtonClickedEvent : SDD.Events.Event { }
public class CreateSessionButtonClickedEvent : SDD.Events.Event { }
public class JoinSessionButtonClickedEvent : SDD.Events.Event { }
public class ResumeButtonClickedEvent : SDD.Events.Event { }
public class EscapeButtonClickedEvent : SDD.Events.Event { }
public class QuitButtonClickedEvent : SDD.Events.Event { }
#endregion


#region GameEvent
public class EnemyHasBeenHitEvent : SDD.Events.Event
{
	public GameObject eEnemyGO;
}
#endregion
