using System;
using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    [RequireComponent(typeof(MouseInputBase))]
    public class Draggable : Clickable
    {
        public event Action<GameObject> OnDragStart;
        public event Action<GameObject, GameObject> OnDragEnd;
        public event Action<GameObject, Vector3, GameObject> OnDragged;

        public bool IsDragging { get; private set; }
        public GameObject DropTarget { get; private set; }

        [SerializeField] LayerMask _dropTargetLayerMask;

        Vector3 _lastMousePosition;

        protected override void Awake()
        {
            base.Awake();
            OnMouseDown += HandleMouseDown;
            OnMouseUp += HandleMouseUp;
        }

        void OnDisable()
        {
            OnMouseDown -= HandleMouseDown;
            OnMouseUp -= HandleMouseUp;
        }

        protected override void Update()
        {
            base.Update();
            if (!IsDragging) return;
            if (MouseInput.MousePosition.Equals(_lastMousePosition)) return;
            _lastMousePosition = MouseInput.MousePosition;
            var (dropTarget, hitPoint) = DetectDropTarget();
            DropTarget = dropTarget;
            OnDragged?.Invoke(gameObject, hitPoint, DropTarget);
        }

        void HandleMouseDown(GameObject go)
        {
            if (IsDragging) return;
            IsDragging = true;
            OnDragStart?.Invoke(go);
        }

        void HandleMouseUp(GameObject go)
        {
            if (!IsDragging) return;
            IsDragging = false;
            var (dropTarget, _) = DetectDropTarget();
            OnDragEnd?.Invoke(go, dropTarget);
        }

        (GameObject targetObject, Vector3 hitPoint) DetectDropTarget()
        {
            var ray = Camera.ScreenPointToRay(MouseInput.MousePosition);
            if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, _dropTargetLayerMask))
                return (null, Vector3.positiveInfinity);
            if (DropTarget && hitInfo.collider.gameObject.GetInstanceID().Equals(DropTarget.GetInstanceID()))
                return (DropTarget, ray.GetPoint(hitInfo.distance));
            return (hitInfo.collider.gameObject, ray.GetPoint(hitInfo.distance));
        }
    }
}