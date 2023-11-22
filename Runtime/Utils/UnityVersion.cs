using System;
using UnityEngine;

namespace MUtility
{
	public struct UnityVersion : IComparable<UnityVersion>, IComparable<string>
	{
		static bool _isCached = false;
		static UnityVersion _cached;

		public enum Type
		{
			Alpha,
			Beta,
			ReleaseCandidate,
			Final
		}

		public int major;
		public int minor1;
		public int minor2;
		public Type releaseType;
		public int patch;
		public bool isValid;

		public static UnityVersion Get()
		{
			if (!_isCached)
				_cached = new UnityVersion(Application.unityVersion);
			return _cached;
		}

		public UnityVersion(string versionNumber)
		{
			string[] versions = versionNumber.Split('.');

			major = 0;
			minor1 = 0;

			isValid = versions.Length == 3;
			if (versions.Length >= 1)
				if (!int.TryParse(versions[0], out major))
					isValid = false;
			if (versions.Length >= 2)
				if (!int.TryParse(versions[1], out minor1))
					isValid = false;
			if (versions.Length < 3)
			{
				minor2 = 0;
				releaseType = Type.Alpha;
				patch = 0;
			}
			else
			{
				string lastText = versions[2];
				if (lastText.Contains("a"))
					LastBit(lastText, "a", Type.Alpha, out minor2, out releaseType, out patch, ref isValid);
				else if (lastText.Contains("b"))
					LastBit(lastText, "b", Type.Beta, out minor2, out releaseType, out patch, ref isValid);
				else if (lastText.Contains("rc"))
					LastBit(lastText, "rc", Type.ReleaseCandidate, out minor2, out releaseType, out patch, ref isValid);
				else if (lastText.Contains("f"))
					LastBit(lastText, "f", Type.Final, out minor2, out releaseType, out patch, ref isValid);
				else
				{
					isValid = false;
					minor2 = 0;
					releaseType = Type.Alpha;
					patch = 0;
				}
			}

			void LastBit(
				string lastText, string testText, Type releaseT,
				out int minor2, out Type releaseType, out int patch,
				ref bool isValid)
			{
				releaseType = releaseT;
				int index = lastText.IndexOf(testText, StringComparison.Ordinal);
				if (!int.TryParse(lastText.Substring(0, index), out minor2))
					isValid = false;
				if (!int.TryParse(lastText.Substring(index + testText.Length), out patch))
					isValid = false;
			}
		}

		public bool IsHigherOrEqualThan(UnityVersion other) => CompareTo(other) >= 0;
		public bool IsHigherThan(UnityVersion other) => CompareTo(other) == 1;
		public bool IsLowerOrEqualThan(UnityVersion other) => CompareTo(other) <= 0;
		public bool IsLowerThan(UnityVersion other) => CompareTo(other) == -1;

		public bool IsHigherOrEqualThan(string other) => IsHigherOrEqualThan(new UnityVersion(other));
		public bool IsHigherThan(string other) => IsHigherThan(new UnityVersion(other));
		public bool IsLowerOrEqualThan(string other) => IsLowerOrEqualThan(new UnityVersion(other));
		public bool IsLowerThan(string other) => IsLowerThan(new UnityVersion(other));
		public int CompareTo(UnityVersion other)
		{
			if (major != other.major)
				return major > other.major ? 1 : -1;
			if (minor1 != other.minor1)
				return minor1 > other.minor1 ? 1 : -1;
			if (minor2 != other.minor2)
				return minor2 > other.minor2 ? 1 : -1;
			if (releaseType != other.releaseType)
				return releaseType > other.releaseType ? 1 : -1;
			if (patch != other.patch)
				return patch > other.patch ? 1 : -1;
			if (patch != other.patch)
				return patch > other.patch ? 1 : -1;
			if (isValid != other.isValid)
				return isValid ? 1 : -1;
			return 0;
		}

		public int CompareTo(string other) => CompareTo(new UnityVersion(other));
	}
}