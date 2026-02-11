using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    public class DropTargetIndicator : MonoBehaviour
    {
        [SerializeField] DragAndDropManager _dragAndDropManager;
        [SerializeField] GameObject _dropIndicatorPrefab;

        GameObject _dropIndicator;

        void Start()
        {
            _dragAndDropManager.OnDragged += HandleDragged;
            _dragAndDropManager.OnDragEnd += HandleDragEnd;
        }

        void HandleDragged(GameObject draggedObject, Vector3 position, GameObject dropTarget)
        {
            if (!dropTarget && _dropIndicator)
            {
                ObjectPoolManager.DespawnGameObject(_dropIndicator);
                _dropIndicator = null;
                return;
            }

            if (!_dropIndicator)
            {
                SpawnDropTargetIndicator(dropTarget);
                return;
            }

            MoveDropTargetIndicator(dropTarget);
        }

        void SpawnDropTargetIndicator(GameObject dropTarget)
        {
            _dropIndicator = ObjectPoolManager.SpawnGameObject(_dropIndicatorPrefab, false);
            _dropIndicator.transform.SetParent(dropTarget.transform);
            _dropIndicator.gameObject.SetActive(true);
            _dropIndicator.transform.localPosition = Vector3.zero;
        }

        void MoveDropTargetIndicator(GameObject dropTarget)
        {
            _dropIndicator.transform.SetParent(dropTarget.transform);
            _dropIndicator.transform.localPosition = Vector3.zero;
        }

        void HandleDragEnd(GameObject arg1, GameObject arg2)
        {
            if (!_dropIndicator) return;
            ObjectPoolManager.DespawnGameObject(_dropIndicator);
            _dropIndicator = null;
        }
    }
}