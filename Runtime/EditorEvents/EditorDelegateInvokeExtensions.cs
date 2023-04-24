using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MUtility
{
	public static class EditorDelegateInvokeExtensions
	{
		// Actions
		public static void EditorInvoke(this Action action) => action.DynamicEditorInvoke();

		public static void EditorInvoke<T1>(this Action<T1> action, T1 param1) =>
			action.DynamicEditorInvoke(param1);

		public static void EditorInvoke<T1, T2>(this Action<T1, T2> action, T1 param1, T2 param2) =>
			action.DynamicEditorInvoke(param1, param2);

		public static void EditorInvoke<T1, T2, T3>(
			this Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3) =>
			action.DynamicEditorInvoke(param1, param2, param3);

		public static void EditorInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4>
			action, T1 param1, T2 param2, T3 param3, T4 param4) =>
			action.DynamicEditorInvoke(param1, param2, param3, param4);


		// Functions 
		public static TOut EditorInvoke<TOut>(this Func<TOut> function) =>
			(TOut)function.DynamicEditorInvoke();

		public static TOut EditorInvoke<T1, TOut>(this Func<T1, TOut> function, T1 param1) =>
			(TOut)function.DynamicEditorInvoke(param1);

		public static TOut EditorInvoke<T1, T2, TOut>(this Func<T1, T2, TOut> function, T1 param1, T2 param2) =>
			(TOut)function.DynamicEditorInvoke(param1, param2);

		public static TOut EditorInvoke<T1, T2, T3, TOut>(this Func<T1, T2, T3, TOut> function,
			T1 param1, T2 param2, T3 param3) =>
			(TOut)function.DynamicEditorInvoke(param1, param2, param3);

		public static TOut EditorInvoke<T1, T2, T3, T4, TOut>(this Func<T1, T2, T3, T4, TOut> function,
			T1 param1, T2 param2, T3 param3, T4 param4) =>
			(TOut)function.DynamicEditorInvoke(param1, param2, param3, param4);

		internal static object DynamicEditorInvoke(this Delegate del, params object[] args)
		{
#if !UNITY_EDITOR
            return del?.DynamicInvoke(args);
#else
			if (Application.isPlaying)
				return del?.DynamicInvoke(args);


			// Subscribe all Subscribers 
			foreach (MonoBehaviour monoBehaviour in Object.FindObjectsOfType<MonoBehaviour>())
			{
				if (!(monoBehaviour is IEditorSubscriber subscriber))
					continue;
				if (subscriber.IsSubscribedAlready)
					continue;
				subscriber.Subscribe();
			}

			//Invoke 
			return del?.DynamicInvoke(args);
#endif
		}
	}
}