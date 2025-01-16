using UnityEngine;

namespace Class
{
	public class OmrCardProp : Usable
	{
		[SerializeField] private RectTransform omrUI;

		public override PropTypes PropType => throw new System.NotImplementedException();

		public override void Interact(PlayerController controller)
		{
			omrUI.gameObject.SetActive(true);
		}

		protected override void Init()
		{
			if (omrUI.gameObject.activeSelf)
				omrUI.gameObject.SetActive(false);
		}
	}
}