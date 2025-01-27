using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class {
    public class HintController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private GameObject hint;
        [SerializeField] private GameObject blackBoard;
        [SerializeField] private GameObject hintGuard;
        [Space]
        [Header("Hint Gaurd Factor")]
        [SerializeField] private float endTime = 3f;
        [SerializeField] float speed = 3f;
        [Space]
        [Header("Stage 1's Tv Screen Factor")]
        [SerializeField] private GameObject tvScreen;
        [SerializeField] private Material[] DesksImages;
        [Space]
        [Header("Stage 2's Lectern Factor")]
        [SerializeField] private GameObject Lectern;
        [SerializeField] private Material emissiveLecternMaterial;
        private int currentStage;
        

        private void Start()
        {
            currentStage = GameManagerEx.Instance.CurrentStage;
        }


        float elapsedTime = 0;
        private IEnumerator setHintGuardMove()
        {
            while (elapsedTime < endTime)
            {
                hintGuard.transform.position += Vector3.right * speed * Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0;
            hintGuard.SetActive(false);
        }

        public void SetHintAppear()
        {
            if(currentStage == 1)
            {
                int imageIndex = DeskManager.Instance.PresetIndex;
                tvScreen.GetComponent<MeshRenderer>().material = DesksImages[imageIndex];
            }
            else if (currentStage == 2)
            {
                Lectern.GetComponent<MeshRenderer>().material = emissiveLecternMaterial;
            }
            else
            {
                StartCoroutine(setHintGuardMove());
            }
            
        }

        [ContextMenu("stage 2 test")]
        public void testtest()
        {
            Lectern.GetComponent<MeshRenderer>().material = emissiveLecternMaterial;
        }

    }



}
