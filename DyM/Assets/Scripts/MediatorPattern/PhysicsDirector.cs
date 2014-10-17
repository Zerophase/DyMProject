using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Utilities;
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
			
			if (grounds != null)
			{
				foreach (var physicsMed in physicsMediators)
				{
					foreach (var ground in grounds)
					{
						
						if (physicsMed.collider.CheckBounds(ground.collider))
						{
							physicsMed.Gravity = Vector3.zero;
							Debug.Log("Position of Player: " + physicsMed.collider.transform.position +
							          "Position of Ground: " + ground.collider.transform.position + 
							          " Gravity is Zero");
						}	
						
						else if (!physicsMed.collider.CheckBounds(ground.collider))
						{
							physicsMed.Gravity = gravity;
							Debug.Log("Position of Player: " + physicsMed.collider.transform.position +
							          "Position of Ground: " + ground.collider.transform.position + 
							          " Gravity is -9.8");
						}
					}
				}
			}
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
