using System.Collections;
using System.Collections.Generic;
using Class;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class DisplayUI : MonoBehaviour
{
    [SerializeReference]
    private TextMeshProUGUI InteractableNameUI;
    [SerializeReference]
    private GameObject key_F;

    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    void Update()
    {
        InteractableNameUI.text = controller.RecentlyDetectedProp?.ToString();

        key_F.SetActive(controller.IsDetectInteractable);
        InteractableNameUI.enabled = controller.IsDetectInteractable;
    }
}
