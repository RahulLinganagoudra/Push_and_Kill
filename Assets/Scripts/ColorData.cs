using System.Collections.Generic;
using UnityEngine;

namespace Creative
{
	public enum ColorType
	{
		Red,
		Blue,
		Yellow,
	}

	[CreateAssetMenu]
	public class ColorData : ScriptableObject
	{
		static ColorData instance;

		public static ColorData Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<ColorData>(nameof(ColorData));
				}
				return instance;
			}
		}

		[System.Serializable]
		public class VisualData
		{
			public ColorType colorType;
			public Material material;
		}
		[field: SerializeField] public int MatchCount { get; internal set; } = 3; // Number of bundles to match before clearing the slot
		[SerializeField] List<VisualData> materials;
		[SerializeField] float slotOffset = 0.5f;
		[SerializeField] float bundleOffset = 0.25f;

		[SerializeField] float tileInterval = 1;
		[SerializeField] float tileJumpDuration = 0.5f;
		[field: SerializeField] public float TileRotationDuration { get; internal set; }



		public float SlotOffset { get => slotOffset; set => slotOffset = value; }
		public float BundleOffset { get => bundleOffset; set => bundleOffset = value; }
		public float TileInterval { get => tileInterval; set => tileInterval = value; }
		public float TileJumpDuration { get => tileJumpDuration; set => tileJumpDuration = value; }
		public List<VisualData> Materials { get => materials; set => materials = value; }

		public Vector3 GetStackedPosition(int index, Vector3 startPosition, float slotOffset)
		{
			Vector3 offset = index * slotOffset * Vector3.up;

			return startPosition + offset; // Apply rotation to the offset
		}
		public Material GetMaterial(ColorType colorType)
		{
			foreach (var data in Materials)
			{
				if (data.colorType == colorType)
				{
					return data.material;
				}
			}
			return null; // or throw an exception if not found
		}
		public ColorType GetRandomColorType()
		{
			return (ColorType)Random.Range(0, Materials.Count);
		}
	}
}