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
        [Header("Factor")]
        [SerializeField] private float endTime = 3f;
        [SerializeField] float speed = 3f;


        Vector3 size = new Vector3();

        private void Start()
        {
            size.Set(8.22f, 1.81f, 0.16f);

            hintGuard.transform.localScale = size;
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
            StartCoroutine(setHintGuardMove());
        }


    }



}
