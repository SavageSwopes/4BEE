using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    // [RequireComponent(typeof(Clickable))] // Better requirement
    public class HoverIndicator : MonoBehaviour
    {
        [SerializeField] Vector3 _hoverScaleMultiplier = new Vector3(1.1f, 1.1f, 1.1f);

        Clickable _clickable;
        Vector3 _originalScale; // Variable to remember starting size

        private void Start()
        {
            _clickable = GetComponent<Clickable>();

            // REMEMBER the starting size so we can go back to it later
            _originalScale = transform.localScale;

            if (!_clickable) return;
            _clickable.OnMouseEntered += OnMouseEntered;
            _clickable.OnMouseExited += OnMouseExited;
        }

        private void OnDisable()
        {
            if (!_clickable) return;
            _clickable.OnMouseEntered -= OnMouseEntered;
            _clickable.OnMouseExited -= OnMouseExited;
        }

        void OnMouseEntered(GameObject _)
        {
            // Multiply the original size by 1.1 instead of hardcoding it
            transform.localScale = Vector3.Scale(_originalScale, _hoverScaleMultiplier);
        }

        void OnMouseExited(GameObject _)
        {
            // Return to the size we remembered at the start
            transform.localScale = _originalScale;
        }
    }
}