using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GameObject))] 
public class DisplayUI : MonoBehaviour
{

    [SerializeReference]
    private GameObject UIElement;   // it is UI displayed

    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    void Update()
    {
        UIElement.SetActive(controller.IsDetectInteractable && !controller.IsInteracting);
    }
}
