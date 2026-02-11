using System;
using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    public class DragAndDropManager : MonoBehaviour
    {
        public event Action<GameObject> OnDragStart;
        public event Action<GameObject, GameObject> OnDragEnd;
        public event Action<GameObject, Vector3, GameObject> OnDragged;

        [SerializeField] Draggable[] _draggables;
//    [SerializeField] Vector3 _hoverScale = new Vector3(1.1f, 1.1f, 1.1f);

        GameObject _draggedObject;
        bool IsDragging => _draggedObject;

        void Awake()
        {
            foreach (var draggable in _draggables)
            {
                draggable.OnDragStart += HandleDragStart;
                draggable.OnDragged += HandleDragged;
                draggable.OnDragEnd += HandleDragEnd;
                draggable.SetActive(true);
            }
        }

        void OnDisable()
        {
            foreach (var draggable in _draggables)
            {
                draggable.OnDragStart -= HandleDragStart;
                draggable.OnDragged -= HandleDragged;
                draggable.OnDragEnd -= HandleDragEnd;
                draggable.SetActive(false);
            }
        }

        void HandleDragStart(GameObject draggedObject)
        {
            if (IsDragging) return;
            foreach (var draggable in _draggables)
            {
                if (draggable.gameObject == draggedObject) continue;
                draggable.SetActive(false);
            }

            _draggedObject = draggedObject;
            OnDragStart?.Invoke(draggedObject);
        }

        void HandleDragged(GameObject go, Vector3 position, GameObject dropTarget)
        {
            if (!IsDragging) return;
            OnDragged?.Invoke(go, position, dropTarget);
        }

        void HandleDragEnd(GameObject go, GameObject dropTarget)
        {
            if (!IsDragging) return;
            foreach (var draggable in _draggables)
            {
                draggable.SetActive(true);
            }

            OnDragEnd?.Invoke(go, dropTarget);
        }
    }
}