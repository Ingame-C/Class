using UnityEngine;
using UnityEngine.UI;


namespace Class
{

    [RequireComponent(typeof(PlayerController))]
    public class DisplayUI : MonoBehaviour
    {
        private PlayerController controller;

        [Header("Viewport Center")]
        [SerializeReference] private Image viewportCenter;
        [SerializeField] Color originalColor;
        [SerializeField] Color interactableColor;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (controller.IsDetectInteractable)
            { 
                viewportCenter.color = interactableColor;
            }
            else
            {
                viewportCenter.color = originalColor;
            }
        }
    }

}