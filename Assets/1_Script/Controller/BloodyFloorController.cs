using System.Collections;
using UnityEngine;

public class BloodyFloorController : MonoBehaviour
{

    public void ShowBlood(float duration)
    {
        StartCoroutine(ShowBloodCoroutine(duration));
    }

    private IEnumerator ShowBloodCoroutine(float duration)
    {

        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = Color.clear;
            child.gameObject.SetActive(true);
        }

        float elapsedTime = 0f;
        Color tmpColor = Color.white;

        while (elapsedTime < duration)
        {
            tmpColor.a = elapsedTime / duration;
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material.color = tmpColor;
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
    
}
