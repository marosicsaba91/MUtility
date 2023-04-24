using MUtility;
using UnityEngine;

namespace Utility_Examples
{
	public class EasingTest : MonoBehaviour
	{
#pragma warning disable 414
		[SerializeField] ElasticEasingFunction elasticEasingFunction = default;
		[SerializeField] CircularEasingFunction circularEasingFunction = default;
		[SerializeField] ExponentialEasingFunction exponentialEasingFunction = default;
		[SerializeField] SinusoidalEasingFunction sinusoidalEasingFunction = default;
		[SerializeField] InfiniteEasingFunction infiniteEasingFunction = default;
#pragma warning restore 414
	}

}