using UnityEngine;
using System.Collections;

public class FlagsManager : Singleton<FlagsManager> {

	public void SetFlag(string flagName,bool state)
	{
		string key = this.GetType().ToString()+"_"+flagName;
		PlayerPrefs.SetInt(key,state?1:0);
	}

	public bool FlagExists(string flagName)
	{
		string key = this.GetType().ToString()+"_"+flagName;
		return PlayerPrefs.HasKey(key);
	}

	public bool GetFlag(string flagName,bool defaultFlagIfNotDefined = false)
	{
		string key = this.GetType().ToString()+"_"+flagName;
		if(!PlayerPrefs.HasKey(key)) SetFlag(flagName,defaultFlagIfNotDefined);
		return PlayerPrefs.GetInt(key)==1;
	}

	public bool InvertFlag(string flagName)
	{
		SetFlag(flagName,!GetFlag(flagName));
		return GetFlag(flagName);
	}
}
