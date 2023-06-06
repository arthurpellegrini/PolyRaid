using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMainMenuEvent : SDD.Events.Event
{
}
public class GameCreditsEvent : SDD.Events.Event
{
}
public class GameCreateSessionEvent : SDD.Events.Event
{
}
public class GameJoinSessionEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GamePausedEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public float eKill { get; set; }
	public float eScore { get; set; }
	public float eMag { get; set; }
	public float eMunition { get; set; }
	public float eGrenade { get; set; }
	public bool eIsCrouching { get; set; }
	public int eLife { get; set; }
}
#endregion

#region MenuManager Events
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
public class CreditsButtonClickedEvent : SDD.Events.Event
{
}
public class CreateSessionButtonClickedEvent : SDD.Events.Event
{
}
public class JoinSessionButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class QuitButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region Score Event
public class ScoreItemEvent : SDD.Events.Event
{
	public float eScore;
}
#endregion
