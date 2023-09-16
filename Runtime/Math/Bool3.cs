namespace MUtility
{
	public struct Bool3
	{
		public bool x, y, z;

		public static readonly Bool3 falseValue = false;
		public static readonly Bool3 trueValue = true;

		public Bool3(bool x, bool y, bool z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Bool3(bool b)
		{
			x = b;
			y = b;
			z = b;
		}

		public static implicit operator Bool3(bool b) => new(b, b, b);
	}
}