using System;
using System.Collections.Generic;
using MUtility;
using UnityEngine;

namespace Utility_Examples
{
	public class Temperature : MonoBehaviour
	{

		const float zeroKelvinInCelsius = -273.15f;
		const float kelvinToFahrenheitMultiplier = 1.8f;
		const float zeroCelsiusZeroFahrenheitDifference = 32f;
		const float epsilon = 0.0001f;

		[SerializeField, Range2(xMin: -1, xMax: 0, yMin: -1, yMax: 1)] Vector2 rangedVector1;
		[SerializeField, Range2(xMin: -10, xMax: 10, yMin: -2, yMax: 2)] Vector2Int rangedVector2;

		[SerializeField] FloatRange rangedFloat;
		[SerializeField] IntRange rangedInt;
		[SerializeField] Position3 pos;
		[SerializeField] List<Position3> positions;

		[SerializeField] bool iAmAmerican;

		[SerializeField] DisplayMember temperatureInKelvin = new DisplayMember(nameof(TemperatureInKelvin));

		[HideIf(nameof(iAmAmerican))]
		[SerializeField] DisplayMember temperatureInCelsius = new DisplayMember(nameof(TemperatureInCelsius));
		[ShowIf(nameof(iAmAmerican))]
		[SerializeField] DisplayMember temperatureInFahrenheit = new DisplayMember(nameof(TemperatureInFahrenheit));
		[SerializeField] DisplayMember increaseTemperature = new DisplayMember(nameof(IncreaseTemperatureClicked));
		[SerializeField] DisplayMessage legacyInspectorMessage = new DisplayMessage("AAAA");

		[SerializeField] DisplayMember testReference = new DisplayMember(nameof(testCollider));

		Collider testCollider;
		[Space]
		[SerializeField] Direction2D testEnum;

		[SerializeField, HideInInspector] float temperatureInK;

		public float TemperatureInKelvin
		{
			get => temperatureInK;
			set => temperatureInK = Mathf.Max(0, value);
		}

		public float TemperatureInCelsius
		{
			get
			{
				float result = temperatureInK + zeroKelvinInCelsius;
				return Math.Abs(result) < epsilon ? 0 : result;
			}
			set => temperatureInK = value - zeroKelvinInCelsius;
		}

		public float TemperatureInFahrenheit
		{
			get
			{
				float result = TemperatureInCelsius * kelvinToFahrenheitMultiplier + zeroCelsiusZeroFahrenheitDifference;
				return Math.Abs(result) < epsilon ? 0 : result;
			}
			set => temperatureInK =
				(value - zeroCelsiusZeroFahrenheitDifference) / kelvinToFahrenheitMultiplier - zeroKelvinInCelsius;
		}


		void IncreaseTemperatureClicked() => TemperatureInCelsius++;

		static void TemperatureChanged(Temperature parent, float oldValue, float newValue) =>
			Debug.Log($"New temperature: {newValue}");


		void ResetTemperature() => TemperatureInKelvin = -zeroKelvinInCelsius;
	}
}