using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    public class DragIndicator : MonoBehaviour
    {
        [SerializeField] Vector3 _dragOffset = new Vector3(0, 0.1f, 0);

        Draggable _draggable;

        void Start()
        {
            _draggable = GetComponent<Draggable>();
            if (!_draggable) return;
            _draggable.OnDragStart += OnDragStart;
        }

        private void OnDisable()
        {
            if (!_draggable) return;
            _draggable.OnDragStart -= OnDragStart;
        }

        private void OnDragStart(GameObject _)
        {
            transform.position += _dragOffset;
        }
    }
}