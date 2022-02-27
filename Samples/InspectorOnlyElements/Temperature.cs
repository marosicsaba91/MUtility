using System;
using MUtility;
using UnityEngine; 
using System.Collections.Generic; 
using UnityEngine.Serialization;

namespace Utility_Examples
{
public partial class Temperature : MonoBehaviour
{
	[Serializable]
	class Direction2D : InspectorEnum<MUtility.Direction2D>
	{
	}

	const float zeroKelvinInCelsius = -273.15f;
	const float kelvinToFahrenheitMultiplier = 1.8f;
	const float zeroCelsiusZeroFahrenheitDifference = 32f;
	const float epsilon = 0.0001f;

	[SerializeField] bool iAmAmerican = false;

	[SerializeField] TemperatureKelvin temperatureInKelvin =
		new TemperatureKelvin { Value = -zeroKelvinInCelsius, valueChanged = TemperatureChanged };

	[SerializeField] TemperatureCelsius temperatureInCelsius;
	[SerializeField] TemperatureFahrenheit temperatureInFahrenheit;
	[SerializeField] ResetTemperatureButton resetTemperature;

	[SerializeField] InspectorButton increaseTemperature =
		new InspectorButton { onClicked = IncreaseTemperatureClicked };

	[FormerlySerializedAs("testEnumProperty")] [Space] [SerializeField]
	Direction2D testEnum = new Direction2D { valueChanged = OnEnumChanged };

	static void OnEnumChanged(object parent, MUtility.Direction2D oldValue, MUtility.Direction2D newValue)
	{
		Debug.Log($"Enum Changed:     {oldValue}  =>  {newValue}");
	}


	static void IncreaseTemperatureClicked(object parentObject) =>
		((Temperature)parentObject).temperatureInCelsius.Value++;

	[SerializeField] InspectorInt someInspectorInt;
	[SerializeField] OneDigit oneDigit;

	static void TemperatureChanged(Temperature parent, float oldValue, float newValue) =>
		Debug.Log($"New temperature: {newValue}");


	public float TemperatureInKelvin
	{
		get => temperatureInKelvin.Value;
		set => temperatureInKelvin.Value = value;
	}

	public float TemperatureInCelsius
	{
		get
		{
			float result = temperatureInKelvin + zeroKelvinInCelsius;
			return Math.Abs(result) < epsilon ? 0 : result;
		}
		set => temperatureInKelvin.Value = value - zeroKelvinInCelsius;
	}

	public float TemperatureInFahrenheit
	{
		get
		{
			float result = (TemperatureInCelsius * kelvinToFahrenheitMultiplier) + zeroCelsiusZeroFahrenheitDifference;
			return Math.Abs(result) < epsilon ? 0 : result;
		}
		set => temperatureInKelvin.Value =
			((value - zeroCelsiusZeroFahrenheitDifference) / kelvinToFahrenheitMultiplier) - zeroKelvinInCelsius;
	}

	bool IsDefault => Math.Abs(temperatureInKelvin + zeroKelvinInCelsius) < epsilon;

	void ResetTemperature() => temperatureInKelvin.Value = -zeroKelvinInCelsius;


	[Serializable]
	class TemperatureKelvin : InspectorFloat<Temperature>
	{
		protected override float GetValue(Temperature parentObject) => Math.Abs(value) < epsilon ? 0 : value;

		protected override void SetValue(Temperature parentObject, float newValue)
		{
			if (value == newValue) return;
			value = Mathf.Max(newValue, 0);
		}

	}

	[Serializable]
	class TemperatureFahrenheit : InspectorFloat<Temperature>
	{
		protected override float GetValue(Temperature parentObject) => parentObject.TemperatureInFahrenheit;

		protected override void SetValue(Temperature parentObject, float newValue)
		{
			if (parentObject.TemperatureInFahrenheit == newValue) return;
			parentObject.TemperatureInFahrenheit = newValue;
		}

		protected override bool IsVisible(Temperature parentObject) => parentObject.iAmAmerican;
	}

	[Serializable]
	class TemperatureCelsius : InspectorFloat<Temperature>
	{
		protected override float GetValue(Temperature parentObject) => parentObject.TemperatureInCelsius;

		protected override void SetValue(Temperature parentObject, float newValue)
		{
			if (parentObject.TemperatureInCelsius == newValue) return;
			parentObject.TemperatureInCelsius = newValue;
		}
	}

	[Serializable]
	class ResetTemperatureButton : InspectorButton<Temperature>
	{
		protected override void OnClick(Temperature parentObject) => parentObject.ResetTemperature();
		protected override bool IsEnabled(Temperature parentObject) => !parentObject.IsDefault;

		protected override string WarningMessage(Temperature parentObject) =>
			"Do You really really change the value to 0 °C?";
	}

	[Serializable]
	class OneDigit : InspectorInt<Temperature>
	{
		protected override IList<int> PopupElements(Temperature container)
		{
			const int numbers = 10;
			var results = new List<int>(numbers);
			for (var i = 0; i < numbers; i++)
				results.Add(i);
			return results;
		}
	}
}
}