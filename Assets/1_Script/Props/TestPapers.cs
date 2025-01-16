using System.Collections;
using UnityEngine;

namespace Class
{
	public class TestPapers : Usable
	{
		public override PropTypes PropType => throw new System.NotImplementedException();

		[SerializeField] private GameObject pagePrefab;
		[SerializeField] private float flipDuration;

		private TestPage leftPage = null;
		private TestPage rightPage = null;
		private TestPage flippingPage = null;

		private bool flipLock = false;

		protected override void Init()
		{
			leftPage = Instantiate(pagePrefab, transform.TransformPoint(Vector3.zero), 
				Quaternion.Euler(0, 0, 180f), transform).GetComponent<TestPage>();
			rightPage = Instantiate(pagePrefab, transform.TransformPoint(Vector3.zero),
				Quaternion.Euler(0, 0, 0), transform).GetComponent<TestPage>();
		}

		public override void Interact(PlayerController controller)
		{
			if (flipLock) return;
			flipLock = true;

			flippingPage = rightPage;
			rightPage = Instantiate(pagePrefab, transform.TransformPoint(Vector3.zero),
				Quaternion.Euler(0, 0, 0), transform).GetComponent<TestPage>();

			StartCoroutine(PageFlipCoroutine());
		}

		private IEnumerator PageFlipCoroutine()
		{
			float elapsedTime = 0f;
			while(elapsedTime < flipDuration)
			{
				flippingPage.transform.rotation = Quaternion.Euler(0, 0, 180 * elapsedTime / flipDuration);
				elapsedTime += Time.deltaTime;	
				yield return null;
			}

			Destroy(leftPage);
			leftPage = flippingPage;
			flippingPage = null;


			flipLock = false;
		}
	}
}