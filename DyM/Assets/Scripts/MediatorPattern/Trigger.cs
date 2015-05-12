using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class Trigger : PhysicsMediator
	{

		private SplineController splineController;
		private SplineInterpolator splineInterpolator;

		protected override void Start()
		{
			splineController = Camera.main.GetComponent<SplineController>();
			splineInterpolator = Camera.main.GetComponent<SplineInterpolator>();

			base.Start();
		}

		public void Tripped()
		{
			splineInterpolator.enabled = true;
			splineController.enabled = true;
		}
	}
}