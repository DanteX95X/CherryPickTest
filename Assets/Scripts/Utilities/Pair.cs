using System;

namespace Assets.Scripts.Utilities
{
	public class Pair<T1, T2>
	{
		public T1 first { get; set; }
		public T2 second { get; set; }


		public Pair()
		{

		}

		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}
	}
}