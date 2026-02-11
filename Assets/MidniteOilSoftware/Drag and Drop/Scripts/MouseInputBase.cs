using UnityEngine;

namespace MidniteOilSoftware.DragAndDrop
{
    public class MouseInputBase : MonoBehaviour
    {
        public virtual Vector3 MousePosition => Input.mousePosition;

        public virtual bool GetMouseButtonDown(int button) => Input.GetMouseButtonDown(button);
        public virtual bool GetMouseButtonUp(int button) => Input.GetMouseButtonUp(button);
    }
}