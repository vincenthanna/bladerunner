using UnityEngine;
using System.Collections;

public enum enumGameStatus
{
	PRE_INIT,
	INIT,
	START,
	RUNNING,
	END
};

public class BRGameStatus
{
	// global singleton object impl.
	static BRGameStatus __instance;

	static BRGameStatus ()
	{
		__instance = new BRGameStatus ();
	}

	public static BRGameStatus Instance ()
	{
		return __instance;
	}
	
	
	// general :
	enumGameStatus _gameStatus;
	
	BRGameStatus ()
	{
		_gameStatus = enumGameStatus.START;
	}

	public enumGameStatus gameStatus ()
	{
		return _gameStatus;
	}
	
	public void setGameStatus (enumGameStatus status)
	{
		_gameStatus = status;
	}
	
	
}