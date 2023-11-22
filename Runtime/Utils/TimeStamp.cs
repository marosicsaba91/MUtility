using System;
using UnityEngine;

namespace MUtility
{

	[Serializable]
	public struct TimeStamp
	{
		[SerializeField] long systemTimeInTicks;
		[SerializeField] float gameTime;
		[SerializeField] int frameCount;
		[SerializeField] float unscaledTime;
		[SerializeField] float fixedTime;
		[SerializeField] float fixedUnscaledTime;

		public long SystemTimeInTicks => systemTimeInTicks;
		public float GameTime => gameTime;
		public int FrameCount => frameCount;
		public float UnscaledTime => fixedTime;
		public float FixedTime => default;
		public float FixedUnscaledTime => fixedUnscaledTime;

		public string SystemTimeShortString => TicksToShortString(systemTimeInTicks);
		public string GameTimeShortString => SecondsToShortString(gameTime);
		public string UnscaledTimeShortString => SecondsToShortString(unscaledTime);
		public string FixedTimeShortString => SecondsToShortString(fixedTime);
		public string FixedUnscaledTimeShortString => SecondsToShortString(fixedUnscaledTime);

		public DateTime DateTime => new DateTime(systemTimeInTicks);
		string TicksToShortString(long ticks)
		{
			var time = new DateTime(ticks);
			int hours = time.Hour;
			int minutes = time.Minute;
			float seconds = time.Second + time.Millisecond / 1000f;
			return ToHeaderShortString(hours, minutes, seconds);
		}

		string SecondsToShortString(float seconds)
		{
			int hours = (int)seconds / 60 / 60;
			seconds -= hours * 60 * 60;
			int minutes = (int)seconds / 60;
			seconds -= minutes * 60;
			return ToHeaderShortString(hours, minutes, seconds);
		}

		string ToHeaderShortString(int hours, int minutes, float seconds)
		{
			if (hours != 0)
				return $"{hours}:{minutes:00}:{seconds:00}";
			if (minutes != 0)
				return $"{minutes:00}:{seconds:00.00}";
			if (seconds != 0)
				return $"{seconds:0.00}";
			return "0";
		}

		public static TimeStamp Now() => new TimeStamp
		{
			systemTimeInTicks = DateTime.Now.Ticks,
			gameTime = Time.time,
			frameCount = Time.frameCount,
			unscaledTime = Time.unscaledTime,
			fixedTime = Time.fixedTime,
			fixedUnscaledTime = Time.fixedUnscaledTime
		};
	}
}