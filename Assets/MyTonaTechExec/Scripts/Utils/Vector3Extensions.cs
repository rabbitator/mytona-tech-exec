using UnityEngine;

namespace MyTonaTechExec.Utils
{
	public static class Vector3Extensions
	{
		public static Vector3 Flat(this Vector3 vector3)
		{
			vector3.y = 0;
			return vector3;
		}
	}
}