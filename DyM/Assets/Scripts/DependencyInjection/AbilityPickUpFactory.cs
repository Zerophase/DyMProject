using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using ModestTree.Zenject;

namespace Assets.Scripts.DependencyInjection
{
	public class AbilityPickUpFactory
	{
		private Instantiator instantiator;

		[Inject]
		private AbilityFactory abilityFactory;

		public AbilityPickUpFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public AbilityPickUp Create(AbilityTypes abilityTypes)
		{
			switch (abilityTypes)
			{
				case AbilityTypes.SLOW_TIME:
					return instantiator.Instantiate<AbilityPickUp>(
						abilityFactory.Create(abilityTypes));
                case AbilityTypes.SPEED_UP_TIME:
                    return instantiator.Instantiate<AbilityPickUp>(
                        abilityFactory.Create(abilityTypes));
			}

			return null;
		}
	}
}
