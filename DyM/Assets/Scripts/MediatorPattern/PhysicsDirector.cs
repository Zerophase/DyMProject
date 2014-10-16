using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class PhysicsDirector : Director
	{
		private List<PhysicsMediator> physicsMediators = new List<PhysicsMediator>();
		private List<Ground> grounds = new List<Ground>();
 
		private bool assignGravity;

		private Vector3 gravity = new Vector3(0f, -9.8f, 0f);

		void Update()
		{
			gravityAssignment();
		}

		private void gravityAssignment()
		{
			if (assignGravity)
			{
				foreach (PhysicsMediator physicsMed in physicsMediators)
				{
					physicsMed.Gravity = gravity;
				}

				assignGravity = false;
			}

			if (grounds != null)
			{
				foreach (var physicsMed in physicsMediators)
				{
					foreach (var ground in grounds)
					{
						if (physicsMed.GetOurCollider.CheckBounds(ground.collider.bounds))
							physicsMed.Gravity = Vector3.zero;
						else if (!physicsMed.GetOurCollider.CheckBounds(ground.collider.bounds))
							physicsMed.Gravity = gravity;
					}
				}
			}
		}

		public override void Receive(ITelegram telegram)
		{
			if (telegram.Receiver == this)
			{
				if(telegram.Message is PhysicsMediator)
				{
					physicsMediators.Add((PhysicsMediator)telegram.Message);
					assignGravity = true;
				}
				else if(telegram.Message is Ground)
					grounds.Add((Ground)telegram.Message);
			}
		}
	}
}
