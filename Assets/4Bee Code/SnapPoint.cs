using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    // This variable remembers which piece is currently sitting here.
    // If it is null, the sqare is empty.
    public GameObject CurrentPiece;

    // Helper to check if occupied
    public bool IsOccupied => CurrentPiece != null;
}