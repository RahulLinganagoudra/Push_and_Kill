using DG.Tweening;
using UnityEngine;

namespace Creative
{
	public class CharacterController : Singleton<CharacterController>
	{
		[SerializeField] Transform startPosition, endPosition;
		[SerializeField] float backSpeed = .01f;
		[SerializeField] float forwardSpeed = .1f;
		[SerializeField] float moveForwardAmount = .3f;

		float time;
		bool canUpdate = true;

		private void Start()
		{
			time = 0;
			transform.position = Vector3.Lerp(startPosition.position, endPosition.position, time);
		}
		private void Update()
		{
			if (canUpdate)
			{
				if (time < 1)
				{
					time += backSpeed * Time.deltaTime;
				}
				else
				{
					print("GameOver");
					canUpdate = false;
				}

			}
			transform.position = Vector3.Lerp(startPosition.position, endPosition.position, time);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				MoveForward();
			}
		}
		public void MoveForward()
		{
			canUpdate = false;
			//time += moveForwardAmount;
			float timeL = this.time;
			DOTween.To(
				getter: () => time,
				setter: (x) => time = Mathf.Clamp01(x),
				endValue: timeL - moveForwardAmount,
				duration: .5f).OnComplete(() => canUpdate = true);
		}
	}
}