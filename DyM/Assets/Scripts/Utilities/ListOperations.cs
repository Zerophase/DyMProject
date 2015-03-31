using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities
{
	public static class ListOperations
	{
		public static void Swap<T>(this IList<T> list, int index1, int index2)
		{
			if(index2 == -1)
				return;

			var temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}
	}
}
