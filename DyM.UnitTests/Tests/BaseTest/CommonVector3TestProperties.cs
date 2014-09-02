using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utilities;

namespace DyM.UnitTests.Tests.BaseTest
{
	public class CommonVector3TestProperties
	{
		// If vector3s aren't testing correctly add this with syntax:
		// Assert.That(expected, Is.EqualTo(actual).Using(vector3EqualityComparerWithTolerance));
		protected readonly Vector3EqualityComparerWithTolerance vector3EqualityComparerWithTolerance
			= new Vector3EqualityComparerWithTolerance();
	}
}
