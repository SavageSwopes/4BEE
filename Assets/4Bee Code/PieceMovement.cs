using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    public class DragMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] Vector3 _snapOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] bool _strictSnap = true;

        private Draggable _draggable;
        private Vector3 _startPosition; // Remembers where we came from
        private SnapPoint _currentSnapPoint; // Remembers which square we just left

        private void Awake()
        {
            _draggable = GetComponent<Draggable>();
        }

        private void Start()
        {
            // On game start, scan underneath to "claim" the square we are sitting on.
            // This ensures pieces placed in the Editor block squares immediately.
            RegisterOnStart();
        }

        private void OnEnable()
        {
            _draggable.OnDragStart += HandleDragStart;
            _draggable.OnDragged += HandleDragged;
            _draggable.OnDragEnd += HandleDragEnd;
        }

        private void OnDisable()
        {
            _draggable.OnDragStart -= HandleDragStart;
            _draggable.OnDragged -= HandleDragged;
            _draggable.OnDragEnd -= HandleDragEnd;
        }

        // 1. Pick Up
        private void HandleDragStart(GameObject obj)
        {
            // Remember where we started in case we need to bounce back
            _startPosition = transform.position;

            // If we were sitting on a SnapPoint, tell it "I'm leaving, you are now empty."
            if (_currentSnapPoint != null)
            {
                _currentSnapPoint.CurrentPiece = null;
                _currentSnapPoint = null;
            }
        }

        // 2. Dragging (Visuals only)
        private void HandleDragged(GameObject obj, Vector3 hitPoint, GameObject dropTarget)
        {
            if (dropTarget && dropTarget.GetComponent<SnapPoint>())
            {
                // Visual snap while hovering
                transform.position = dropTarget.transform.position + _snapOffset;
            }
            else
            {
                // Follow mouse loosely if off-grid
                if (!_strictSnap && hitPoint != Vector3.positiveInfinity)
                {
                    transform.position = hitPoint + _snapOffset;
                }
            }
        }

        // 3. Drop (The Decision)
        private void HandleDragEnd(GameObject obj, GameObject dropTarget)
        {
            // Did we drop on a valid square?
            if (dropTarget != null)
            {
                SnapPoint targetSquare = dropTarget.GetComponent<SnapPoint>();

                // CHECK: Is the square valid AND Empty?
                if (targetSquare != null && !targetSquare.IsOccupied)
                {
                    // SUCCESS: Snap to new square
                    transform.position = dropTarget.transform.position + _snapOffset;

                    // Register ourselves on the new square
                    targetSquare.CurrentPiece = gameObject;
                    _currentSnapPoint = targetSquare;
                    return;
                }
            }

            // FAILURE: If we got here, the drop was invalid or occupied.
            // Return to start.
            transform.position = _startPosition;

            // Re-occupy our old square (if we had one)
            // We need to raycast down to find the old square again to be safe
            if (Physics.Raycast(_startPosition, Vector3.down, out RaycastHit hit, 2f))
            {
                var oldSquare = hit.collider.GetComponent<SnapPoint>();
                if (oldSquare)
                {
                    oldSquare.CurrentPiece = gameObject;
                    _currentSnapPoint = oldSquare;
                }
            }
        }

        private void RegisterOnStart()
        {
            // Shoot a ray down to find the square underneath us at the start of the game
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
            {
                var snapPoint = hit.collider.GetComponent<SnapPoint>();
                if (snapPoint)
                {
                    snapPoint.CurrentPiece = gameObject;
                    _currentSnapPoint = snapPoint;
                }
            }
        }
    }
}