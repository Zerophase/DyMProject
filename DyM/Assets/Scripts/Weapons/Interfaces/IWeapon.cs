using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Weapons.Interfaces
{
	public interface IWeapon : IOwner
	{
		Vector3 Position { get; set; }
	}
}
