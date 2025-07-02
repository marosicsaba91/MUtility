using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System;

namespace MUtility
{
	public class ModularStopwatch
	{
		long _lastStart;

		readonly Dictionary<string, long> _modules = new();
		string _currentModuleName;

		public bool IsRunning { get; private set; }
		public string Name { get; set; }

		public ModularStopwatch(string name)
		{
			Name = name;
		}

		public ModularStopwatch()
		{
			Name = "Unnamed Benchmark";
		}

		public void Clear()
		{
			_currentModuleName = null;
			_modules.Clear();
			IsRunning = false;
		}

		public void Start()
		{
			if (_currentModuleName == null)
				throw new Exception("No Module is Selected");

			StartModule(_currentModuleName);
		}

		public void ClearAndStart(string moduleName)
		{
			Clear();
			StartModule(moduleName);
		}

		public void StartModule(string moduleName)
		{
			if (moduleName != _currentModuleName)
			{
				SetModule(moduleName);
			}

			if (!IsRunning)
			{
				IsRunning = true;
				_lastStart = Stopwatch.GetTimestamp();
			}
		}

		public void SetModule(string moduleName)
		{
			long now = Stopwatch.GetTimestamp();

			if (moduleName == null)
				return;
			if (moduleName == _currentModuleName)
				return;

			if (!_modules.ContainsKey(moduleName))
				_modules.Add(moduleName, 0);

			if (!IsRunning)
			{
				_currentModuleName = moduleName;
			}
			else
			{
				if (_currentModuleName != null)
				{
					long duration = now - _lastStart;
					_modules[_currentModuleName] += duration;
				}

				_currentModuleName = moduleName;
				_lastStart = Stopwatch.GetTimestamp();
			}
		}

		public void Stop()
		{
			long duration = Stopwatch.GetTimestamp() - _lastStart;
			if (!IsRunning)
				return;

			IsRunning = false;
			_modules[_currentModuleName] += duration;
		}

		public long GetModuleDuration(string moduleName)
		{
			long now = Stopwatch.GetTimestamp();
			long duration = _modules[moduleName];

			if (IsRunning)
			{
				duration += now - _lastStart;
				_lastStart = Stopwatch.GetTimestamp();
			}

			return duration;
		}

		public IEnumerable<string> GetModule() => _modules.Keys;

		static readonly StringBuilder _stringBuilder = new();

		public override string ToString()
		{
			bool didRun = IsRunning;
			if (didRun)
				Stop();

			_stringBuilder.Clear();
			double allTicks = GetTotalTicks();
			double allMs = allTicks * ticksToMs;
			_stringBuilder.AppendLine($"{Name} - Total Time:\t {allMs:F4} ms");
			_stringBuilder.AppendLine("------------------------------------");
			foreach (KeyValuePair<string, long> module in _modules)
			{
				double ticks = module.Value;
				double percent = ticks / allTicks * 100;
				_stringBuilder.AppendLine($"{module.Key}:\t {ticks * ticksToMs:F4} ms\t ({percent:F2} %)");
			}
			string result = _stringBuilder.ToString();

			if (didRun)
				Start();
			return result;
		}

		public double GetTotalTicks()
		{
			double allDuration = 0;
			foreach (KeyValuePair<string, long> module in _modules)
				allDuration += module.Value;
			return allDuration;
		}

		public double GetTotalMilliseconds()
		{
			double allDuration = 0;
			foreach (KeyValuePair<string, long> module in _modules)
				allDuration += module.Value;

			return allDuration * ticksToMs;
		}

		const double ticksToMs = 0.0001;
	}
}