using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer; // DI 프레임워크

namespace SupanthaPaul
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField]
		private float smoothSpeed = 0.125f;
		public Vector3 offset;
		[Header("Camera bounds")]
		public Vector3 minCamerabounds;
		public Vector3 maxCamerabounds;

		//플레이어 추적 할당을 DI로 받도록 수정
		private Transform target;
        private PlayerProvider _playerProvider;
        [Inject]
        public void Construct(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
			target = _playerProvider.playerTransform;
        }


        private void FixedUpdate()
		{
			if (target == null) return;

			Vector3 desiredPosition = target.localPosition + offset;
			var localPosition = transform.localPosition;
			Vector3 smoothedPosition = Vector3.Lerp(localPosition, desiredPosition, smoothSpeed);
			localPosition = smoothedPosition;

			// clamp camera's position between min and max
			localPosition = new Vector3(
				Mathf.Clamp(localPosition.x, minCamerabounds.x, maxCamerabounds.x),
				Mathf.Clamp(localPosition.y, minCamerabounds.y, maxCamerabounds.y),
				Mathf.Clamp(localPosition.z, minCamerabounds.z, maxCamerabounds.z)
				);
			transform.localPosition = localPosition;
		}

		public void SetTarget(Transform targetToSet)
		{
			target = targetToSet;
		}
	}
}
