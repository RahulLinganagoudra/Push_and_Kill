using Creative;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
	public ColorType colorType;

	public List<CardBundle> slotCards = new List<CardBundle>();

	public void AddBundle(CardBundle cardBundle)
	{
		Sequence bundleSequence = DOTween.Sequence();

		Destroy(cardBundle.GetComponent<Rigidbody>()); // Remove Rigidbody if it exists to prevent physics interference

		Vector3 targetPos = ColorData.Instance.GetStackedPosition(slotCards.Count, transform.position, ColorData.Instance.BundleOffset);
		cardBundle.UnParentChildren();
		cardBundle.transform.SetPositionAndRotation(targetPos, Quaternion.identity);

		for (int i = 0; i < cardBundle.cards.Count; i++)
		{
			Sequence cardSequence = DOTween.Sequence();

			GameObject card = cardBundle.cards[i];

			cardSequence.AppendInterval(ColorData.Instance.TileInterval * i)
			.Append(
				card.transform.DOJump(
					ColorData.Instance.GetStackedPosition(i, targetPos, ColorData.Instance.SlotOffset),
					1f, 1, ColorData.Instance.TileJumpDuration).SetEase(Ease.OutBounce))
			.Append(
				card.transform.DORotate(new Vector3(0, 0, 0), ColorData.Instance.TileJumpDuration).SetEase(Ease.OutBack)
				);

			bundleSequence.Join(cardSequence);
		}

		bundleSequence.OnComplete(() =>
		{
			cardBundle.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
			cardBundle.RepositionChildren();
			TryMatch();
		});

		slotCards.Add(cardBundle);
	}

	private void TryMatch()
	{
		if (slotCards.Count < ColorData.Instance.MatchCount) return;

		for (int i = 0; i < slotCards.Count; i++)
		{
			slotCards[i].DestroyChildren();
			slotCards[i].gameObject.SetActive(false);
		}
		//TODO: Play Confetti;

		slotCards.Clear();
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < 5; i++)
		{
			Vector3 targetPos = ColorData.Instance.GetStackedPosition(i, transform.position, ColorData.Instance.BundleOffset);

			Gizmos.DrawSphere(targetPos, 0.1f);
		}

	}
}
