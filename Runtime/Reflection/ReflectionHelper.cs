using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MUtility
{
	public class ReflectionHelper
	{
		/// <summary>
		/// Relevant Assemblies are:
		///     Executing Assembly (The assembly being called from),
		///     The Unity Global Assembly (Scripts with no Assembly Definition),
		///     Any Assembly that references the the Executing Assembly
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Assembly> FindRelevantAssemblies(bool includeExecutingAssembly = true)
		{
			var currentAssembly = Assembly.GetExecutingAssembly();
			string currentAssemblyFullName = currentAssembly.FullName;
			const string globalAssembly = "Assembly-CSharp";
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				string assemblyName = assembly.GetName().Name;
				bool isCurrent = includeExecutingAssembly && assembly == currentAssembly;

				if (isCurrent)
				{
					yield return assembly;
					continue;
				}

				bool isGlobal = assemblyName == globalAssembly;
				if (isGlobal)
				{
					yield return assembly;
					continue;
				}

				AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				bool isReferencingCurrent =
					referencedAssemblies.Select(an => an.FullName).Contains(currentAssemblyFullName);

				if (isReferencingCurrent)
					yield return assembly;
			}
		}
	}
}