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
        [SerializeField] Color transparentColor;
        private void Awake()
        {
            controller = GetComponent<PlayerController>();
        }

        private void Update()
        {

            if (controller.UIisSet)
            {
                viewportCenter.color = transparentColor;
                return;
            }
            
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