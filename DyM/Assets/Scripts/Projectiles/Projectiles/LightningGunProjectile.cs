using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public class LightningGunProjectile : ProjectileBase
	{
		private Vector3 positionStorage;
		private float smoothCurve = 0.0f;
		private float bulletTimer = 0.0f;
		private float smoothCurveChange = 100f;

		private static int bulletLane = 0;
		private int lane;

		public LightningGunProjectile() :
			base("LightningGunBullet", "Cube", 40f)
		{
			damage = 1;
		}

		public override void SetUpProjectile(Vector3 position)
		{
			if (bulletLane == 3)
			{
				if (smoothCurveChange < 0.0f)
					smoothCurveChange = -smoothCurveChange;
				bulletLane = 0;
			}
			lane = bulletLane++;

			base.SetUpProjectile(position);
		}

		//There was a bug that caused the bullets to explode by using: 
		//fireDirection = -Player.GunModel.transform.up + new Vector3(0f, 0f, 0.5f);
		//might be interesting to make a gun that shoots outwards and then explodes in
		//4 directions
		public override Vector3 ProjectilePattern()
		{
			bulletTimer += Time.deltaTime;
			positionStorage = speed * fireDirection;

			if (lane == 2 && smoothCurveChange > 0.0f)
			{
				smoothCurveChange = -smoothCurveChange;
			}

			if (lane == 2 || lane == 0)
			{
				determineCurve();
			}
			
			return positionStorage;
		}

		private void determineCurve()
		{
			if (Mathf.Abs(smoothCurve) < 40.0f)
			{
				positionStorage.z = smoothCurve = smoothCurveChange*bulletTimer;
			}
			else if (Mathf.Abs(smoothCurve) > 40f)
			{
				positionStorage.z = 0.0f;
			}

			if(Mathf.Abs(smoothCurve) > 70f)
				Debug.Log(smoothCurve);
		}

		public override void DeactivateProjectile()
		{
			smoothCurve = 0f;
			bulletTimer = 0f;
			base.DeactivateProjectile();
		}
	}
}
