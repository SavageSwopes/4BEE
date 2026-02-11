using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    // This variable remembers which piece is currently sitting here.
    // If it is null, the sqare is empty.

    [SerializeField] private float detectionRadius = 0.5f;
    //Which tags are allowed on this snap point
    [SerializeField] private List<string> allowedTags = new List<string>();
    private bool allowAnyPiece = true;
    [SerializeField] private GameObject snapPointPrefab;
    [SerializeField] private float xLocation;
    [SerializeField] private float yLocation;
    [SerializeField] private float snapPointOffsetZ = 0.0001062f;


    // Helper to check if occupied
    public bool IsOccupied => CurrentPiece != null;
    public GameObject CurrentPiece;
    public GameObject pieceToIgnore;

    //Create script to add new snap points about placed bee pieces.
    private void OnTriggerEnter(Collider other)
    {
        //if (IsOccupied) return;
        Debug.Log("SnapPoint triggered by: " + other.gameObject.name);
        if (allowAnyPiece || allowedTags.Contains(other.tag))
        {
            CurrentPiece = other.gameObject;
            Debug.Log("Object placed on snap point!");
            HandleStackLogic();
        }
    }

    private void HandleStackLogic()
    {
        if (CurrentPiece.CompareTag("Red Bee")) 
        {
            Debug.Log("Red Bee placed, creating snap point");
            CreateSnapPointAbove("Red Bee", "Blue Stinger");
        }
    }

    private void CreateSnapPointAbove(string beeTag, string stringerTag)
    {
        Vector3 spawnPosition = transform.position + new Vector3(xLocation, yLocation, snapPointOffsetZ);

        GameObject newSnapPoint = Instantiate(snapPointPrefab, spawnPosition, Quaternion.identity);

        //Make it follow the piece
        newSnapPoint.transform.parent = CurrentPiece.transform;

        SnapPoint snapScript = newSnapPoint.GetComponent<SnapPoint>();

        snapScript.allowedTags.Clear();
        snapScript.allowedTags.Add(beeTag);
        snapScript.allowedTags.Add(stringerTag);

    }
}