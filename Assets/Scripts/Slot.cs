using Creative;
using DG.Tweening;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
	public ColorType colorType;

	public List<CardBundle> slotCards = new();
	[SerializeField] MeshRenderer model;


	private void Start()
	{
		model.sharedMaterial = ColorData.Instance.GetSlotMaterial(colorType);
	}
	public void AddBundle(CardBundle cardBundle)
	{
		Sequence bundleSequence = DOTween.Sequence();

		Destroy(cardBundle.GetComponent<Rigidbody>()); // Remove Rigidbody if it exists to prevent physics interference

		Vector3 targetPos = ColorData.Instance.GetStackedPosition(slotCards.Count, transform.position, ColorData.Instance.BundleOffset);

		//cardBundle.transform.SetPositionAndRotation(targetPos, Quaternion.identity);

		// First sequence: Scale up the bundle
		var cardBundleHighlight = cardBundle.GetComponentInChildren<HighlightEffect>();
		cardBundleHighlight.highlighted = true;

        bundleSequence
			.Append(cardBundle.transform.DOMove(cardBundle.transform.position + new Vector3(0, 0.5f, -0.5f), ColorData.Instance.PickDuration).SetEase(Ease.InOutQuad))
			.Append(cardBundle.transform.DOScale(Vector3.one * 1.25f, ColorData.Instance.PickDuration).SetEase(Ease.OutBack))
			.Join(cardBundle.transform.DORotate(new Vector3(0, 0, 0), ColorData.Instance.TileRotationDuration).SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					cardBundle.UnParentChildren();
					cardBundleHighlight.highlighted = false;
                }
			));

		// Create a sequence for the card animations
		Sequence cardsSequence = DOTween.Sequence();


        for (int i = 0; i < cardBundle.cards.Count; i++)
        {
            GameObject card = cardBundle.cards[i];
            Vector3 cardTargetPos = ColorData.Instance.GetStackedPosition(i, targetPos, ColorData.Instance.SlotOffset);
			//cardTargetPos.z -= 0.15f;

            Sequence cardSequence = DOTween.Sequence();
            cardSequence
                //.AppendInterval(ColorData.Instance.TileInterval * i)
                // Drop animation from current position
                .Append(card.transform.DOMove(cardTargetPos, ColorData.Instance.TileJumpDuration).SetEase(Ease.InOutQuart))
                // Bounce effect at the end
                .AppendCallback(() => 
                {
                    // Store current position for the bounce
                    Vector3 bouncePos = card.transform.position;
                    card.transform.DOMove(bouncePos + Vector3.up * 0.005f, 0.05f).SetEase(Ease.OutQuad)
                        .OnComplete(() => 
                        {
                            card.transform.DOMove(bouncePos, 0.025f).SetEase(Ease.InBounce);
							card.transform.DOMove(bouncePos + Vector3.forward * 0.05f, 0.05f).SetEase(Ease.InQuad);
                        });
                })
                // Rotation during the drop
                .Insert(ColorData.Instance.TileInterval * i,
                    card.transform.DORotate(Vector3.zero, ColorData.Instance.TileRotationDuration)
                    .SetEase(Ease.Linear));

            cardsSequence.Join(cardSequence);
        }

        // Append the cards sequence after the scale-up animation with a small delay
        bundleSequence
			.Append(cardsSequence);

		bundleSequence.OnComplete(() =>
		{
			cardBundle.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
			cardBundle.RepositionChildren();
            StartCoroutine(TryMatch());
        });

		slotCards.Add(cardBundle);
	}

    private IEnumerator TryMatch()
    {
        if (slotCards.Count < ColorData.Instance.MatchCount) yield break;

        yield return new WaitForSeconds(0.1f); // Wait for the last card to settle

        Creative.CharacterController.Instance.MoveForward();
        colorType = ColorData.Instance.GetRandomColorType();
        Vector3 scale = model.transform.localScale;
        Sequence scaleSequence = DOTween.Sequence();

        // Create sequence for card scale-down animations
        Sequence cardsScaleSequence = DOTween.Sequence();
        for (int i = slotCards.Count - 1; i >= 0; i--)
        {
            CardBundle bundle = slotCards[i];
            for (int j = bundle.cards.Count - 1; j >= 0; j--)
            {
                GameObject card = bundle.cards[j];
                // Join instead of Append to make all cards scale simultaneously
                cardsScaleSequence.AppendInterval(j * 0.0001f);
                cardsScaleSequence.Join(
                    card.transform.DOScale(Vector3.zero, 0.025f).SetEase(Ease.InBack)
                );
            }
        }

        scaleSequence
            .Append(cardsScaleSequence)
            .Append(model.transform.DOScale(0, ColorData.Instance.SlotScaleDownDuration))
            .OnComplete(() =>
            {
                // Clean up after animations complete
                for (int i = 0; i < slotCards.Count; i++)
                {
                    slotCards[i].DestroyChildren();
                    slotCards[i].gameObject.SetActive(false);
                }
                slotCards.Clear();
            });

        scaleSequence.Append(model.transform.DOScale(scale, ColorData.Instance.SlotPopUpDuration)
            .OnStart(() =>
            {
                model.sharedMaterial = ColorData.Instance.GetSlotMaterial(colorType);
            })
        );
    }

    private void OnDrawGizmos()
	{
		for (int i = 0; i < 8; i++)
		{
			Vector3 targetPos = ColorData.Instance.GetStackedPosition(i, transform.position, ColorData.Instance.BundleOffset);

			Gizmos.DrawSphere(targetPos, 0.01f);
		}
	}
}
