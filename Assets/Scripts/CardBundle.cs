using Creative;
using System;
using System.Collections.Generic;
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
			if (card.TryGetComponent<Renderer>(out var renderer))
			{
				renderer.material = ColorData.Instance.GetMaterial(colorType);
			}
		}
	}

	internal void RepositionChildren()
	{
		int i = 0;
		foreach (var child in cards)
		{
			child.transform.SetPositionAndRotation(ColorData.Instance.GetStackedPosition(i++, transform.position, ColorData.Instance.SlotOffset), Quaternion.identity);
		}
	}

	internal void UnParentChildren()
	{
		foreach (Transform card in transform)
		{
			card.transform.parent = null;
		}
	}

	internal void DestroyChildren()
	{
		foreach (var child in cards)
		{
			child.SetActive(false);
		}
	}
}
