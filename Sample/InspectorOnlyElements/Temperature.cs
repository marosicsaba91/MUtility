using System;
using MUtility;
using UnityEngine;

namespace Utility_Examples
{
	public partial class Temperature : MonoBehaviour
	{
		[Serializable]
		class Direction2DProperty : EnumProperty<Direction2D> { }

		const float zeroKelvinInCelsius = -273.15f;
		const float kelvinToFahrenheitMultiplier = 1.8f;
		const float zeroCelsiusZeroFahrenheitDifference = 32f;
		const float epsilon =  0.0001f;
		 
		[SerializeField] bool iAmAmerican = false;

		[SerializeField] TemperatureKelvinProperty temperatureInKelvin = 
			new TemperatureKelvinProperty {Value = -zeroKelvinInCelsius, valueChanged = TemperatureChanged};
		
		[SerializeField] TemperatureCelsiusProperty temperatureInCelsius;
		[SerializeField] TemperatureFahrenheitProperty temperatureInFahrenheit;
		[SerializeField] ResetTemperatureButton resetTemperature;
		[SerializeField] InspectorButton increaseTemperature =
			new InspectorButton {onClicked = IncreaseTemperatureClicked};

		[Space]
		[SerializeField] Direction2DProperty testEnumProperty = new Direction2DProperty {valueChanged = OnEnumChanged};

		static void OnEnumChanged(object parent, Direction2D oldValue, Direction2D newValue)
		{
			Debug.Log($"Enum Changed:     {oldValue}  =>  {newValue}");
		}


		static void IncreaseTemperatureClicked(object parentObject) =>
			((Temperature)parentObject).temperatureInCelsius.Value++;

		[SerializeField] IntProperty someInt; 
		[SerializeField] OneDigitProperty oneDigit; 
		
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