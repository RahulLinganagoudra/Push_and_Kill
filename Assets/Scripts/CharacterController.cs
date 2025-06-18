using DG.Tweening;
using HighlightPlus;
using UnityEngine;

namespace Creative
{
	public class CharacterController : Singleton<CharacterController>
	{
		[SerializeField] Transform startPosition, endPosition;
		[SerializeField] float backSpeed = .01f;
		[SerializeField] float forwardDuration = .1f;
		[SerializeField] float moveForwardAmount = .3f;

		Animator labubiAnimator;

		float time;
		bool canUpdate = true;

        private void Awake()
        {
            labubiAnimator = GetComponentInChildren<Animator>();
        }

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
				duration: forwardDuration)
				.OnStart(() => labubiAnimator.SetFloat("speed", 1f))
				.OnUpdate(() =>
				{
					if (time < .05f)
					{ 
						canUpdate = true;
                        labubiAnimator.SetFloat("speed", 0f);
                    }
                })
                .OnComplete(() => 
					{
						canUpdate = true;
						labubiAnimator.SetFloat("speed", 0f);
                    }
				);
		}

		public void HighlightCharacter()
		{
			var highlightEffect = GetComponentInChildren<HighlightEffect>();
			highlightEffect.highlighted = true;
        }
	}
}