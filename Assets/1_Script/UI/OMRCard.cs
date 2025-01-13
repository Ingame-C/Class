using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Class.UI
{
    public class OMRCard : MonoBehaviour
    {
		[SerializeField] private int maxItemCount;
        [SerializeField] private GameObject itemPrefab;
		[SerializeField] private Transform itemParent;

		[SerializeField] private Button submitButton;

		private List<OMRItem> omrItems = new List<OMRItem>();

		private void Awake()
		{
			Init();

			submitButton.onClick.AddListener(() =>
			{
				OnSubmitButton();
			});
		}

		private void Init()
		{
			for (int i = 0; i < maxItemCount; i++)
			{
				omrItems.Add(Instantiate(itemPrefab, itemParent).GetComponent<OMRItem>());
				omrItems[i].SetProbNum(i + 1);
			}
		}
		
		// HACK - 임시로 만든 부분
		private void OnSubmitButton()
		{
			Debug.Log("Submit!");
			gameObject.SetActive(false);
		}

	}
}