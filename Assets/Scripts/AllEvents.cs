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

public class GameTimerChangedEvent : SDD.Events.Event
{
	public float eTimer { get; set; }
}
public class PlayerHealthChangedEvent : SDD.Events.Event
{
	public int eHealth { get; set; }
}
public class PlayerScoreChangedEvent : SDD.Events.Event
{
	public int eScore { get; set; }
}
public class SessionIDChangedEvent : SDD.Events.Event
{
	public string eSessionID { get; set; }
}
public class FpsChangedEvent : SDD.Events.Event
{
	public int eFps { get; set; }
}
public class PlayerMagChangedEvent : SDD.Events.Event
{
	public int eMag { get; set; }
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
