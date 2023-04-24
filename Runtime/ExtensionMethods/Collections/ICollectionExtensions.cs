using System.Collections.Generic;

namespace MUtility
{
	public static class CollectionExtensions
	{
		public static bool Any<T>(this ICollection<T> source)
			=> source.Count != 0;

		public static bool IsEmpty<T>(this ICollection<T> source)
			=> source.Count == 0;

		public static bool IsNullOrEmpty<T>(this ICollection<T> source)
			=> source == null || source.Count == 0;
	}
}