using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CamController : MonoBehaviour
{
	public static CamController Instance;

	private Camera cam;
	private Transform player;
	public float groundZ = 0;
	
	private void Start()
	{
        cam = Camera.main;
		player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	void Update()
	{
		cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);

		if (Input.GetAxis("Mouse ScrollWheel") > 0f && cam.orthographicSize > 7f) // forward
		{
			cam.orthographicSize--;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cam.orthographicSize < 30) // backwards
		{
			cam.orthographicSize++;
		}
	}
	private Vector3 GetWorldPosition(float z)
	{
        //Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        //Plane ground = new Plane(Vector3.up, new Vector3(0, z, 0));
        //float distance;
        //ground.Raycast(mousePos, out distance);
        //return mousePos.GetPoint(distance);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = cam.nearClipPlane;
        var pos = cam.ScreenToWorldPoint(mousePosition);
		pos.z = z;
		return pos;
    }

    public Vector3 GetCursorWorldPosition()
	{
		return GetWorldPosition(groundZ);
	}
}
