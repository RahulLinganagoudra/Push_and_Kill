using System.Collections;
using UnityEngine;

namespace Creative
{
	public class Spawnner : MonoBehaviour
	{
		[SerializeField] GameObject prefab;
		[SerializeField] Transform spawnPosition;
		IEnumerator Start()
		{
			for (int i = 0; i < 100; i++)
			{
				var go = Instantiate(prefab, transform);
				go.transform.position = spawnPosition.position;
				yield return new WaitForSeconds(.05f);
			}
		}
	}
}