using Creative;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotManager : Singleton<SlotManager>
{
    public List<Slot> slots = new List<Slot>();

    private void Awake()
    {
        slots = GetComponentsInChildren<Slot>().ToList();
    }

    public void AddBundleToSlot(CardBundle cardBundle)
    {
        Slot targetSlot = GetSlot(cardBundle.ColorType);
        if (targetSlot != null)
        {
            targetSlot.AddBundle(cardBundle);
        }
        else
        {
            Debug.LogWarning($"No slot found for color type: {cardBundle.ColorType}");
        }
    }

    public Slot GetSlot(ColorType colorType)
    {
        return slots.FirstOrDefault(slot => slot.colorType == colorType);
    }
}
