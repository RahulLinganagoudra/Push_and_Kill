using System.Collections;
using UnityEngine;

namespace Creative
{
	public class Spawnner : MonoBehaviour
	{
		[SerializeField] CardBundle prefab;
		[SerializeField] Transform spawnPosition;
		[SerializeField] float spawnDelay = 0.01f;
		[SerializeField] int spawnCount = 200;
		IEnumerator Start()
		{
			for (int i = 0; i < spawnCount; i++)
			{
				var go = Instantiate(prefab, transform);
				go.Init( ColorData.Instance.GetRandomColorType());
				go.transform.position = spawnPosition.position;
				yield return new WaitForSeconds(spawnDelay);
			}
		}
	}
}