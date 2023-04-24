namespace MUtility
{
	public readonly struct SingleBenchmarkResult
	{
		public readonly double milliseconds;
		public readonly long bytes;

		public SingleBenchmarkResult(double milliseconds, long bytes)
		{
			this.milliseconds = milliseconds;
			this.bytes = bytes;
		}
	}
}