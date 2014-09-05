using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;

namespace Assets.Scripts.Character
{
	public class TestCharacter : ICharacter
	{
		private List<BaseWeapon> weapons = new List<BaseWeapon>(); 

		private IWeapon weapon;
		public IWeapon Weapon
		{
			get { return weapon; }
		}

		public void Equip(IWeapon weapon)
		{
			this.weapon = weapon;
		}


		public void SwitchWeapon()
		{
			weapons.Sort();
			Equip(weapons.Find(x => x != weapon));
		}

		public void AddWeapon(BaseWeapon weapon)
		{
			weapons.Add(weapon);
		}
	}
}
