using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
public class UnitController : MonoBehaviour
{
    public static Vector3 target;
    public static bool targetControlled = false;
    public static List<Pig> pigs;
    public static HashSet<Pig> selectedUnits;
    public GameObject targetParticle;
    public static bool overUnit = false;
    public float groundZ = 0;

    [SerializeField] AudioSource mousePressSFX;

    public static UnitController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        selectedUnits = new HashSet<Pig>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) &&
            !Drag._dragging &&
             !EventSystem.current.IsPointerOverGameObject() &&
             !overUnit)
        {
            MouseTargetPosition();
               
            MovePigs();
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseTargetPosition();
            mousePressSFX.Play();
            targetParticle.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    void MouseTargetPosition()
    {
        target = GetWorldPosition(groundZ);

        targetParticle.transform.position = target;
    }

    public static void GetPigs()
    {
        if (pigs == null)
            pigs = new List<Pig>();
        
        pigs = FindObjectsOfType<Pig>().ToList() ;
    }

    public void MovePigs()
    {
        if (selectedUnits == null)
        {
            return;
            //Debug.Log("No Units Selected");
        }

        //  GetPigs();
        //Debug.Log("Selected Units" + selectedUnits.Count);
        foreach (Pig pig in selectedUnits)
        {

            Patrol patrol = pig.gameObject.GetComponent<Patrol>();
            patrol.GoToSpecificPoint(target);

        }
    }

    public void ClearUnits()
    {
        foreach (Pig pig in selectedUnits)
        {
            pig.Deselect();
            //pig.selected.SetActive(false);
        }

        selectedUnits.Clear();
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
