using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI
{
	public class AbilityMessage
	{
		private float message;
		public float Message { get { return message; } }
		public AbilityMessage(float message)
		{
			this.message = message;
		}
	}
}
