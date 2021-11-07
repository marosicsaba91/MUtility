using System;
using System.Collections.Generic;
using MUtility;
using UnityEngine;

namespace Utility_Examples
{
	public partial class Temperature
	{
		[Serializable]
		class TemperatureKelvinProperty : FloatProperty<Temperature>
		{
			protected override float GetValue(Temperature parentObject) => Math.Abs(value) < epsilon ? 0 : value;

			protected override void SetValue(Temperature parentObject, float newValue)  
			{
				if (value == newValue) return; 
				value = Mathf.Max(newValue, 0);
			}

		}

		[Serializable]
		class TemperatureFahrenheitProperty : FloatProperty<Temperature>
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
		class TemperatureCelsiusProperty : FloatProperty<Temperature>
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
		class OneDigitProperty : IntProperty<Temperature>
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