using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.CustomExceptions
{
	public class NoReceiverAttachedException : Exception
	{
		public NoReceiverAttachedException()
		{
			
		}

		public NoReceiverAttachedException(string message)
			: base(message)
		{
			
		}

		public NoReceiverAttachedException(string message, Exception inner)
			: base(message, inner)
		{
			
		}
	}
}
