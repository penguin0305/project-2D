	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	namespace YoungCameraFollow

	{
		public class CameraFollow : MonoBehaviour
		{
			[SerializeField]
			public Transform target;

			public Vector3 offset = new Vector3(0, 0, -10f);


		private void LateUpdate()
			{
				if (target == null) return;

			transform.position = target.position + offset;
		}

			public void SetTarget(Transform targetToSet)
			{
				target = targetToSet;
			}
		}
	}