using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MUtility
{
	public static class KeyCodeExtensions
	{
		public static bool TryGetValue(
			this KeyCode key, out int value, 
			bool enableAlphaNumeric = true, bool enableNumpad = true, bool enableFunctionKeys = false)
		{			
			if (enableAlphaNumeric)
			{
				value = key switch
				{
					KeyCode.Alpha1 => 1,
					KeyCode.Alpha2 => 2,
					KeyCode.Alpha3 => 3,
					KeyCode.Alpha4 => 4,
					KeyCode.Alpha5 => 5,
					KeyCode.Alpha6 => 6,
					KeyCode.Alpha7 => 7,
					KeyCode.Alpha8 => 8,
					KeyCode.Alpha9 => 9,
					KeyCode.Alpha0 => 0,
					_ => -1
				};
				if(value >= 0) 
					return true;
			}

			if (enableNumpad)
			{
				value = key switch
				{
					KeyCode.Keypad1 => 1,
					KeyCode.Keypad2 => 2,
					KeyCode.Keypad3 => 3,
					KeyCode.Keypad4 => 4,
					KeyCode.Keypad5 => 5,
					KeyCode.Keypad6 => 6,
					KeyCode.Keypad7 => 7,
					KeyCode.Keypad8 => 8,
					KeyCode.Keypad9 => 9,
					KeyCode.Keypad0 => 0,
					_ => -1
				};
				if (value >= 0)
					return true;
			}

			if(enableFunctionKeys)
			{
				value = key switch
				{
					KeyCode.F1 => 1,
					KeyCode.F2 => 2,
					KeyCode.F3 => 3,
					KeyCode.F4 => 4,
					KeyCode.F5 => 5,
					KeyCode.F6 => 6,
					KeyCode.F7 => 7,
					KeyCode.F8 => 8,
					KeyCode.F9 => 9,
					KeyCode.F10 => 10,
					KeyCode.F11 => 11,
					KeyCode.F12 => 12,
					KeyCode.F13 => 13,
					KeyCode.F14 => 14,
					KeyCode.F15 => 15,					
					_ => -1
				};
				if (value >= 0)
					return true;
			}

			value = -1;
			return false;
		}
	}
}
