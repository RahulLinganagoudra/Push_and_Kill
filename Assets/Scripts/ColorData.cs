using System.Collections.Generic;
using UnityEngine;

namespace Creative
{
	public enum ColorType
	{
		Red,
		Green,
		Blue,
		Yellow,
		Purple,
		Cyan,
		Magenta
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
					instance = Resources.Load<ColorData>("HexaBundleData");
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
		[SerializeField] List<VisualData> materials;
		[SerializeField] float slotOffset = 0.5f;


		public Vector3 GetStackedPosition(int index, Vector3 startPosition, float slotOffset)
		{
			Vector3 offset = index * slotOffset * Vector3.up;

			return startPosition + offset; // Apply rotation to the offset
		}
		public Material GetMaterial(ColorType colorType)
		{
			foreach (var data in materials)
			{
				if (data.colorType == colorType)
				{
					return data.material;
				}
			}
			return null; // or throw an exception if not found
		}
	}
}