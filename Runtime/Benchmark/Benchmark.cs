using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
	public class Benchmark
	{
		readonly List<Action> _actions = new List<Action>();
		int _runCount = 1_000;

		public int RunCount
		{
			get => _runCount;
			set => _runCount = Mathf.Max(1, value);
		}

		public void AddAction(Action action) => _actions.Add(action);
		public void RemoveAction(Action action) => _actions.Remove(action);

		public BenchmarkResult Run()
		{
			var result = new Dictionary<string, SingleBenchmarkResult>();
			foreach (Action action in _actions)
			{
				// Time
				DateTime start = DateTime.Now;
				for (int i = 0; i < _runCount; i++)
				{
					action();
				}

				double fullTimeSpan = (DateTime.Now - start).TotalMilliseconds;

				// Note relevant Time:  for + method calls
				start = DateTime.Now;
				for (int i = 0; i < _runCount; i++)
				{
					Empty();
				}

				double notRelevantTimeSpan = (DateTime.Now - start).TotalMilliseconds;

				fullTimeSpan -= notRelevantTimeSpan;
				fullTimeSpan = Math.Max(0, fullTimeSpan);
				double durationPerRun = fullTimeSpan / _runCount;

				// Memory 
				long memory = GC.GetTotalMemory(false);
				action();
				long memoryAllocated = GC.GetTotalMemory(false) - memory;

				result.Add(action.Method.Name, new SingleBenchmarkResult(durationPerRun, memoryAllocated));
			}

			return new BenchmarkResult(result, _runCount);
		}

		void Empty()
		{
		}
	}
}