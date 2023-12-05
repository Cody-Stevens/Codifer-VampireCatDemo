 using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    public static bool _dragging = false;

    bool canDrag = false;

    bool isOffMap = false;
    float returnBackTimer;
    float defaultTimerValue = .5f;

    Vector3 startingPosition;

    private void Start()
    {
        returnBackTimer = defaultTimerValue;
        this.transform.parent = null;
        startingPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging && !MouseOffMap.Instance.m_offMap)
        {
            if (UnitController.selectedUnits.Count != 0)
            {
                UnitController.Instance.ClearUnits();
            }

            _dragging = true;
            // Move object, taking into account original offset.
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            isOffMap = false;
        }
        else if (MouseOffMap.Instance.m_offMap)
        {
            _dragging = false;
        }

        // return object to spawn position if it's off the map
        if (isOffMap)
        {
            returnBackTimer -= Time.deltaTime;
            
            if (returnBackTimer <= 0)
            {
                isOffMap = false;
                this.transform.position = startingPosition;
                returnBackTimer = defaultTimerValue;
            }
        }
    }

    private void OnMouseDown()
    {        
        // Record the difference between the objects centre, and the clicked point on the camera plane.

        if (!MouseOffMap.Instance.m_offMap)
        {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }
    }

    private void OnMouseUp()
    {
        // Stop dragging.
        dragging = false;
        _dragging = false;
    }

    private void OnMouseOver()
    {
     
        CursorManager.Instance.UseCustomCursor("Grab");
    
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.UseCustomCursor("");
        _dragging = false;
    }
}