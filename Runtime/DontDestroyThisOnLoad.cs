
using UnityEngine;

namespace MUtility
{
	public class DontDestroyThisOnLoad : MonoBehaviour
	{
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}