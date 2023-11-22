using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PlayModeState
{
	Stopped,
	Playing,
	Paused
}

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class PlayMode
{
	public static PlayModeState CurrentState { get; private set; } = PlayModeState.Stopped;

	static PlayMode()
	{
#if UNITY_EDITOR
		EditorApplication.playModeStateChanged += OnUnityPlayModeChanged;
#endif
	}


	public static event Action<PlayModeState, PlayModeState> PlayModeChanged;

	public static void Play()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = true;
#endif
	}

	public static void Pause()
	{
#if UNITY_EDITOR
		EditorApplication.isPaused = true;
#endif
	}

	public static void Stop()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#endif
	}


	static void OnPlayModeChanged(PlayModeState currentState, PlayModeState changedState)
	{
		if (PlayModeChanged != null)
			PlayModeChanged(currentState, changedState);
	}


#if UNITY_EDITOR
	static void OnUnityPlayModeChanged(PlayModeStateChange obj)
	{
		PlayModeState changedState = PlayModeState.Stopped;
		switch (CurrentState)
		{
			case PlayModeState.Stopped:
				if (EditorApplication.isPlayingOrWillChangePlaymode)
				{
					changedState = PlayModeState.Playing;
				}

				break;
			case PlayModeState.Playing:
				if (EditorApplication.isPaused)
				{
					changedState = PlayModeState.Paused;
				}
				else
				{
					changedState = PlayModeState.Stopped;
				}

				break;
			case PlayModeState.Paused:
				if (EditorApplication.isPlayingOrWillChangePlaymode)
				{
					changedState = PlayModeState.Playing;
				}
				else
				{
					changedState = PlayModeState.Stopped;
				}

				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		// Fire PlayModeChanged event.
		OnPlayModeChanged(CurrentState, changedState);

		// Set current state.
		CurrentState = changedState;
	}
#endif

}