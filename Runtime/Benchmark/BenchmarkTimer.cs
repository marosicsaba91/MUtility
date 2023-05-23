using System;
using System.Collections.Generic;
using System.Text;

namespace MUtility
{
	public class BenchmarkTimer
	{
		DateTime _lastStart;
		readonly Dictionary<string, TimeSpan> _modules = new();
		string _currentModuleName;
		bool _isRunning;

		public bool IsRunning => _isRunning;
		public string Name { get; set; }

		public BenchmarkTimer(string name)
		{

			Name = name;
		}

		public BenchmarkTimer()
		{
			Name = "Unnamed Benchmark";
		}

		public void Clear()
		{
			_currentModuleName = null;
			_modules.Clear();
			_isRunning = false;
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

			if (!_isRunning)
			{
				_isRunning = true;
				_lastStart = DateTime.Now;
			}
		}

		public void SetModule(string moduleName)
		{
			if (moduleName == null)
				return;
			if (moduleName == _currentModuleName)
				return;

			if (!_modules.ContainsKey(moduleName))
				_modules.Add(moduleName, TimeSpan.Zero);

			if (!_isRunning)
			{
				_currentModuleName = moduleName;
			}
			else
			{
				DateTime time = DateTime.Now;

				if (_currentModuleName != null)
				{
					TimeSpan duration = time - _lastStart;
					_modules[_currentModuleName] += duration;
				}

				_currentModuleName = moduleName;
				_lastStart = time;
			}
		}

		public void Stop()
		{
			if (!_isRunning)
				return;

			_isRunning = false;
			TimeSpan duration = DateTime.Now - _lastStart;
			_modules[_currentModuleName] += duration;
		}

		public bool TryGetModuleDuration(string moduleName, out TimeSpan duration)
		{
			bool success = _modules.TryGetValue(moduleName, out duration);
			if (!success)
				return false;

			if (_isRunning)
				duration += DateTime.Now - _lastStart;

			return true;
		}

		public TimeSpan GetModuleDuration(string moduleName)
		{
			TimeSpan duration = _modules[moduleName];

			if (_isRunning)
				duration += DateTime.Now - _lastStart;

			return duration;
		}


		public IEnumerable<string> GetModule() => _modules.Keys;

		static readonly StringBuilder _stringBuilder = new();

		public override string ToString()
		{
			bool didRun = _isRunning;
			if (didRun)
				Stop();

			_stringBuilder.Clear();
			double allDuration = GetTotalTotalMilliseconds();
			_stringBuilder.AppendLine($"{Name} - Total Time:\t {allDuration:F2} ms");
			_stringBuilder.AppendLine("------------------------------------");
			foreach (KeyValuePair<string, TimeSpan> module in _modules)
			{
				double duration = module.Value.TotalMilliseconds;
				double percent = duration / allDuration * 100;
				_stringBuilder.AppendLine($"{module.Key}:\t {duration:F2} ms\t ({percent:F2} %)");
			}
			string result = _stringBuilder.ToString();

			if (didRun)
				Start();
			return result;

		}

		public double GetTotalTotalMilliseconds() 
		{
			double allDuration = 0;
			foreach (KeyValuePair<string, TimeSpan> module in _modules)
				allDuration += module.Value.TotalMilliseconds;
			return allDuration;
		}
	}
}