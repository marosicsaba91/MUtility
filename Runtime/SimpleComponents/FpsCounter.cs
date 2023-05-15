using UnityEngine;
using TMPro;

namespace MarosiUtility
{
	[RequireComponent(typeof(TMP_Text))]
	public class FpsCounter : MonoBehaviour
	{
		[SerializeField] TMP_Text uiText;
		[SerializeField] bool smooth = true;
		[SerializeField] string suffixText = " FPS";

		void OnValidate()
		{
			if(uiText == null)
				uiText = GetComponent<TMP_Text>();
		}

		void Update()
		{
			float deltaTime = smooth ? Time.smoothDeltaTime : Time.deltaTime;
			string text = (1f / deltaTime).ToString("F0");

			if(suffixText != null && suffixText != string.Empty)
				text += suffixText;

			uiText.text = text;
		}


	}
}