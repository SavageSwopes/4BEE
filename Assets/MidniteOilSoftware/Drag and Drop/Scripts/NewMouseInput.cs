using UnityEngine;
using UnityEngine.InputSystem;

namespace MidniteOilSoftware.DragAndDrop
{
    public class NewMouseInput : MouseInputBase
    {
        public override Vector3 MousePosition => Mouse.current.position.ReadValue();

        public override bool GetMouseButtonDown(int button)
        {
            return button switch
            {
                0 => Mouse.current.leftButton.wasPressedThisFrame,
                1 => Mouse.current.rightButton.wasPressedThisFrame,
                _ => false
            };
        }

        public override bool GetMouseButtonUp(int button)
        {
            return button switch
            {
                0 => Mouse.current.leftButton.wasReleasedThisFrame,
                1 => Mouse.current.rightButton.wasReleasedThisFrame,
                _ => false
            };
        }
    }
}
