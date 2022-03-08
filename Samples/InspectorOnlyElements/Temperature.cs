using System;
using MUtility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility_Examples
{
	public partial class Temperature : MonoBehaviour
	{
		[Serializable]
		class Direction2D : InspectorEnum<MUtility.Direction2D> { }

		const float zeroKelvinInCelsius = -273.15f;
		const float kelvinToFahrenheitMultiplier = 1.8f;
		const float zeroCelsiusZeroFahrenheitDifference = 32f;
		const float epsilon =  0.0001f;

		[SerializeField, Range2(xMin: -1,xMax: 0,yMin:-1,yMax:1)] Vector2 rangedVector1; 
		[SerializeField, Range2(xMin: -10, xMax: 10, yMin: -2, yMax: 2)] Vector2Int rangedVector2;
		 
		[SerializeField] bool iAmAmerican = false;

		[SerializeField] TemperatureKelvin temperatureInKelvin = 
			new TemperatureKelvin {Value = -zeroKelvinInCelsius, valueChanged = TemperatureChanged};
		
		[SerializeField] TemperatureCelsius temperatureInCelsius;
		[SerializeField] TemperatureFahrenheit temperatureInFahrenheit;
		[SerializeField] ResetTemperatureButton resetTemperature;
		[SerializeField] InspectorButton increaseTemperature =
			new InspectorButton {onClicked = IncreaseTemperatureClicked};

		[FormerlySerializedAs("testEnumProperty")]
		[Space]
		[SerializeField] Direction2D testEnum = new Direction2D {valueChanged = OnEnumChanged};

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
	}
}