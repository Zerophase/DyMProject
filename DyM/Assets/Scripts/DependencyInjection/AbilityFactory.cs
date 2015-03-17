using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using ModestTree.Zenject;
using Assets.Scripts.Abilities.PlayerSkills;

namespace Assets.Scripts.DependencyInjection
{
	public class AbilityFactory
	{
		private IFactory<AbilityBase> factory;
		private Instantiator instantiator;

		public AbilityFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public AbilityBase Create(AbilityTypes abilityTypes)
		{
			switch (abilityTypes)
			{
				case AbilityTypes.SLOW_TIME:
					return instantiator.Instantiate<SlowTime>();
					break;
                case AbilityTypes.SPEED_UP_TIME:
                    return instantiator.Instantiate<BoostTime>();
                    break;
				default:
					throw new ArgumentOutOfRangeException("abilityTypes");
			}
		}
	}
}
