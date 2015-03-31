using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.StatusEffects
{
	[System.Flags]
	public enum StatusEffect
	{
		NONE = 1 << 0, // 1
		TEST =  1 << 1, // 2
		TESTTWO =  1 << 2, // 4
		SLOW_TIME =  1 << 3, // 8
        BOOST_TIME = 1 << 4 // 16
	};
}
