using System;
using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    [RequireComponent(typeof(MouseInputBase))]
    public class Clickable : MonoBehaviour
    {
        public Action<GameObject> OnMouseDown, OnMouseUp, OnMouseEntered, OnMouseExited;

        public void SetActive(bool active)
        {
            Active = active;
        }

        [SerializeField] protected LayerMask _raycastLayerMask;
        [SerializeField] protected float _raycastDistance = 100f;

        protected MouseInputBase MouseInput { get; private set; }

        bool Active { get; set; }
        protected Camera Camera { get; private set; }
        bool IsMouseOver { get; set; }

        protected virtual void Awake()
        {
            Camera = Camera.main;
            MouseInput = GetComponent<MouseInputBase>();
        }

        protected virtual void Update()
        {
            if (!Active) return;
            DetectMouseOver();
            DetectMouseDown();
            DetectMouseUp();
        }

        void DetectMouseOver()
        {
            var ray = Camera.ScreenPointToRay(MouseInput.MousePosition);
            if (!Physics.Raycast(ray, out var hitInfo, _raycastDistance, _raycastLayerMask))
            {
                if (!IsMouseOver) return;
                HandleMouseExit();
                return;
            }

            if (hitInfo.collider.gameObject != gameObject)
            {
                if (!IsMouseOver) return;
                HandleMouseExit();
                return;
            }

            if (IsMouseOver) return;
            HandleMouseEntered();
        }

        void HandleMouseEntered()
        {
            IsMouseOver = true;
            OnMouseEntered?.Invoke(gameObject);
        }

        void HandleMouseExit()
        {
            IsMouseOver = false;
            OnMouseExited?.Invoke(gameObject);
        }

        void DetectMouseDown()
        {
            if (!MouseInput.GetMouseButtonDown(0)) return;
            if (!IsMouseOver) return;
            OnMouseDown?.Invoke(gameObject);
        }

        void DetectMouseUp()
        {
            if (!MouseInput.GetMouseButtonUp(0)) return;
            if (!IsMouseOver) return;
            OnMouseUp?.Invoke(gameObject);
        }
    }
}