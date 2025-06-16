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

        Vector3 targetPos = (slotCards.Count > 0) ?
            ColorData.Instance.GetStackedPosition(slotCards.Count, slotCards[^1].transform.position, ColorData.Instance.BundleOffset) + transform.position
            : transform.position;

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

        //bundleSequence.OnComplete(() =>
        //{
        //    cardBundle.transform.SetPositionAndRotation(targetPos, Quaternion.identity);

        //    for (int i = 0; i < slotCards.Count; i++)
        //    {
        //        CardBundle card = slotCards[i];

        //        card.transform.SetPositionAndRotation(ColorData.Instance.GetStackedPosition(i, targetPos, ColorData.Instance.BundleOffset), Quaternion.identity);
        //    }
        //});

        slotCards.Add(cardBundle);
    }
}
