using System.Collections.Generic;
using MUtility;
using UnityEngine;

public class NormalDistributionTest : MonoBehaviour
{
	[SerializeField] NormalDistributionFunction normalDistributionFunction = default;
	[Space]
	[SerializeField] int sampleCount;

	Dictionary<int, int> _valueDictionary = new Dictionary<int, int>();
	readonly Rect _rect = new Rect(-5, -2, 10, 4);


	void Update()
	{
		for (int i = 0; i < 100; i++)
			NewSample();
	}

	void NewSample()
	{
		float iq = normalDistributionFunction.GetRandom();
		int intIQ = Mathf.RoundToInt(iq);
		if (!_valueDictionary.ContainsKey(intIQ))
			_valueDictionary.Add(intIQ, 1);
		else
			_valueDictionary[intIQ]++;
	}

	void OnDrawGizmosSelected()
	{
		int minKey = _valueDictionary.MinKey(int.MaxValue);
		int maxKey = _valueDictionary.MaxKey(int.MinValue);
		int minValue = _valueDictionary.MinValue(int.MaxValue);
		int maxValue = _valueDictionary.MaxValue(int.MinValue);

		foreach (KeyValuePair<int, int> iqCounts in _valueDictionary)
		{
			float x = MathHelper.Lerp(iqCounts.Key, _rect.xMin, _rect.xMax, minKey, maxKey);
			float y1 = _rect.yMin;
			float y2 = MathHelper.Lerp(iqCounts.Value, _rect.yMin, _rect.yMax, minValue, maxValue);
			var lineSegment = new LineSegment(new Vector3(x, y1), new Vector3(x, y2));
			lineSegment.DrawDebug(Color.yellow);
		}
	}

	public int AllSampleCount => _valueDictionary.Aggregate((sum, key, value) => sum + value, 0);
}