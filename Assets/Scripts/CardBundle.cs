using Creative;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBundle : MonoBehaviour
{
    private ColorType colorType;
    public List<GameObject> cards = new List<GameObject>();

    public ColorType ColorType { get => colorType; set => colorType = value; }

    private void Awake()
    {
        cards = new();

        foreach (Transform child in transform)
        {
            cards.Add(child.gameObject);
        }
    }

    public void Init(ColorType colorType)
    {
        this.ColorType = colorType;

        foreach (var card in cards)
        {
            var renderer = card.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = ColorData.Instance.GetMaterial(colorType);
            }
        }
    }
}
