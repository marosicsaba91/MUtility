using System;
using System.Collections.Generic;
using System.Text;
using MUtility;
using UnityEngine;

namespace MUtility
{
	public struct BenchmarkResult
	{
		readonly Dictionary<string, SingleBenchmarkResult> _results;
		readonly int _runCount;
		string _text;

		public IReadOnlyDictionary<string, SingleBenchmarkResult> Results => _results;

		public BenchmarkResult(Dictionary<string, SingleBenchmarkResult> results, int runCount)
		{
			_results = results;
			_runCount = runCount;
			_text = null;
		}

		public override string ToString() => GetTextVersion();

		public string GetTextVersion()
		{
			if (_text != null)
				return _text;

			if (_results == null)
				return "No results yet";

			var columns = new List<List<string>>();
			var nameColumn = new List<string>();
			var timeColumn = new List<string>();
			var memoryColumn = new List<string>();
			columns.Add(nameColumn);
			columns.Add(timeColumn);
			columns.Add(memoryColumn);
			nameColumn.Add("");
			timeColumn.Add("Time:");
			memoryColumn.Add("Memory:");

			foreach (KeyValuePair<string, SingleBenchmarkResult> pair in _results)
			{
				nameColumn.Add(pair.Key);
				timeColumn.Add(TimeToText(pair.Value.milliseconds));
				memoryColumn.Add(MemoryToText(pair.Value.bytes));
			}

			Font font = null;
			try
			{
				font = GUI.skin.font;
			}
			catch (ArgumentException)
			{
			}

			FixLength(nameColumn, font);
			FixLength(timeColumn, font);
			FixLength(memoryColumn, font);

			StringBuilder messageBuilder = new StringBuilder();
			messageBuilder.Append(_runCount.ToString("000"));
			messageBuilder.Append(" Runs");

			int w = columns.Count;
			int h = columns[0].Count;
			for (int y = 0; y < h; y++)
			{
				messageBuilder.AppendLine();
				for (int x = 0; x < w; x++)
					messageBuilder.Append(columns[x][y]);
			}

			_text = messageBuilder.ToString();
			return _text;
		}

		void FixLength(List<string> strings, Font font, int minimumMargin = 10)
		{
			int maxWidth = 0;
			foreach (string s in strings)
			{
				int width = s.Width(font);
				maxWidth = Math.Max(maxWidth, width);
			}

			for (int i = 0; i < strings.Count; i++)
			{
				string s = strings[i];
				int width = s.Width(font);
				while (width < maxWidth + minimumMargin)
				{
					s += " ";
					width = s.Width(font);
					if (width < 0)
						break;
				}

				strings[i] = s + "\t";
			}
		}

		string TimeToText(double milliseconds)
		{
			string unit = " ms";
			double num = milliseconds;

			if (milliseconds < 1f)
			{
				num *= 1000;
				unit = " µs";
			}

			if (num < 1f)
			{
				num *= 1000;
				unit = " ns";
			}

			string s = num.ToString("#0.###");
			return s + unit;
		}

		string MemoryToText(long bytes)
		{
			string unit = " bytes";
			double num = bytes;

			if (bytes > 1024f)
			{
				num /= 1024;
				unit = " Kb";
			}

			if (num > 1024f)
			{
				num /= 1024f;
				unit = " Mb";
			}

			string s = num.ToString("#0.###");
			return s + unit;
		}
	}
}