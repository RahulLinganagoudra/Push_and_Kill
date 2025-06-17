using Creative;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
	public ColorType colorType;

	public List<CardBundle> slotCards = new();
	[SerializeField] MeshRenderer model;


	private void Start()
	{
		model.sharedMaterial = ColorData.Instance.GetMaterial(colorType);
	}
	public void AddBundle(CardBundle cardBundle)
	{
		Sequence bundleSequence = DOTween.Sequence();

		Destroy(cardBundle.GetComponent<Rigidbody>()); // Remove Rigidbody if it exists to prevent physics interference

		Vector3 targetPos = ColorData.Instance.GetStackedPosition(slotCards.Count, transform.position, ColorData.Instance.BundleOffset);

		//cardBundle.transform.SetPositionAndRotation(targetPos, Quaternion.identity);

		// First sequence: Scale up the bundle
		bundleSequence
			.Append(cardBundle.transform.DOMove(cardBundle.transform.position + new Vector3(0, 0.5f, -0.5f), 0.15f).SetEase(Ease.InOutQuad))
			.Append(cardBundle.transform.DOScale(Vector3.one * 1.25f, 0.15f).SetEase(Ease.OutBack))
			.Join(cardBundle.transform.DORotate(new Vector3(0, 0, 0), 0.15f).SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					cardBundle.UnParentChildren();
				}
			));

		// Create a sequence for the card animations
		Sequence cardsSequence = DOTween.Sequence();

		cardsSequence.AppendInterval(0.1f); // Small delay before starting card animations

		for (int i = 0; i < cardBundle.cards.Count; i++)
		{
			GameObject card = cardBundle.cards[i];

			Sequence cardSequence = DOTween.Sequence();
			cardSequence
				.AppendInterval(ColorData.Instance.TileInterval * i)
				.Append(
					card.transform.DOJump(
						ColorData.Instance.GetStackedPosition(i, targetPos, ColorData.Instance.SlotOffset),
						1f, 1, ColorData.Instance.TileJumpDuration).SetEase(Ease.OutBack))
				.Join(
					card.transform.DORotate(Vector3.zero, ColorData.Instance.TileRotationDuration).SetEase(Ease.Linear)
				)
				.Join(card.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Flash));

			cardsSequence.Join(cardSequence);
		}

		// Append the cards sequence after the scale-up animation with a small delay
		bundleSequence
			.Append(cardsSequence);

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
		Creative.CharacterController.Instance.MoveForward();
		slotCards.Clear();
		colorType = ColorData.Instance.GetRandomColorType();
		Vector3 scale = model.transform.localScale;
		Sequence scaleSequence = DOTween.Sequence();
		scaleSequence.Append(model.transform.DOScale(0, .1f));
		scaleSequence.Append(model.transform.DOScale(scale, .1f).OnStart(() =>
		{
			model.sharedMaterial = ColorData.Instance.GetMaterial(colorType);
		}));
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < 5; i++)
		{
			Vector3 targetPos = ColorData.Instance.GetStackedPosition(i, transform.position, ColorData.Instance.BundleOffset);

			Gizmos.DrawSphere(targetPos, 0.01f);
		}

	}
}
