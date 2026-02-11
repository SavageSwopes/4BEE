using UnityEngine;
public class PieceController : MonoBehaviour
{
    private bool isDragging = false;
    private float startYPosition;

    void Start()
    {
        startYPosition = transform.position.y;
    }

    void OnMouseDown()
    {
        Debug.Log("I WAS CLICKED!"); 
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToNearestPoint();
    }

    void Update()
    {
        if (isDragging)
        {
            MovePieceWithMouse();
        }
    }

    void MovePieceWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, startYPosition, 0));

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }

    void SnapToNearestPoint()
    {
        GameObject[] snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint");

        GameObject closestPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject point in snapPoints)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPoint = point;
            }
        }

        if (closestPoint != null && shortestDistance < 1.5f)
        {
            Vector3 newPos = closestPoint.transform.position;
            transform.position = new Vector3(newPos.x, startYPosition, newPos.z);
        }
    }
}

