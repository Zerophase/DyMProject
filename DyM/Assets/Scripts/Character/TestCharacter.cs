using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using UnityEngine;

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

		private IReceiver receiver;

		public Vector3 Position { get; set; }

		public TestCharacter()
		{
			
		}

		public TestCharacter(IReceiver receiver)
		{
			this.receiver = receiver;
			this.receiver.Owner = this;
			receiver.SubScribe();
		}

		public void Equip(IWeapon weapon)
		{
			this.weapon = weapon;
		}

		public void Receive(Telegram telegram)
		{
			AddWeapon((TestWeapon)telegram.Message);
		}

		public void SwitchWeapon()
		{
			weapons.Sort();
			Equip(weapons.Find(x => x != weapon));
		}

		public void AddWeapon(BaseWeapon weapon)
		{
			weapons.Add(weapon);
			if(weapons.Count < 2)
				Equip(weapon);
		}
	}
}
