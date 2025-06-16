using UnityEngine;

namespace Creative
{
	public class Raycaster : MonoBehaviour
	{
		[SerializeField] LayerMask tileMask;
		RaycastHit hit;
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, float.PositiveInfinity, tileMask) && hit.collider.TryGetComponent(out CardBundle cardBundle))
				{
					SlotManager.Instance.AddBundleToSlot(cardBundle);
                    print($"Hit {hit.collider.name} && with the color type {cardBundle.ColorType}");
				}
			}
		}
	}
}