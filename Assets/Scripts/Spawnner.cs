using System.Collections;
using UnityEngine;

namespace Creative
{
	public class Spawnner : MonoBehaviour
	{
		[SerializeField] CardBundle prefab;
		[SerializeField] Transform spawnPosition;
		[SerializeField] int spawnCount = 200;
		IEnumerator Start()
		{
			for (int i = 0; i < spawnCount; i++)
			{
				var go = Instantiate(prefab, transform);
				go.Init((ColorType)Random.Range(0, System.Enum.GetNames(typeof(ColorType)).Length));
				go.transform.position = spawnPosition.position;
				yield return new WaitForSeconds(.05f);
			}
		}
	}
}