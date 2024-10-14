using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour
{

    [SerializeField] private Vector3 classCenter;
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        cameraTransform.position = classCenter;
        cameraTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

}
