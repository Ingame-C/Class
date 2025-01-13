using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Class.UI
{
    public class OMRItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI probNumText;
        [SerializeField] private List<Button> buttons;

        private int curSelectedIdx = -1;
        public int SelectedIdx { get => curSelectedIdx; }

		private void Awake()
		{
			for(int i=0; i<buttons.Count; i++)
            {
                int t = i;
                buttons[i].onClick.AddListener(() =>
                {
                    OnOMRClick(t);
                });
            }
		}

        private void OnOMRClick(int idx)
        {
            Clear();
            buttons[idx].GetComponent<Image>().color = Color.black;
        }

		public void SetProbNum(int num)
        {
            probNumText.text = num.ToString();
        }

        public void Clear()
        {
            foreach(Button button in buttons)
            {
                button.GetComponent<Image>().color = Color.white;
            }
        }

    }
}