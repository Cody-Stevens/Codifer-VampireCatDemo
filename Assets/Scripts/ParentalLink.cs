using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentalLink : MonoBehaviour
{
    public Transform parents;
    public Transform kid;

    private LineRenderer lineRenderer;

	public Transform parentIcon;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		lineRenderer.SetPosition(0, parents.position);
		lineRenderer.SetPosition(1, kid.position);

		Vector2 midpoint = (parents.transform.position + kid.transform.position) / 2;
		parentIcon.position = midpoint;
	}
}
