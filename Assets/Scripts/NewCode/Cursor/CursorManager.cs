using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public GameObject mouseClickParticle;
    public AudioSource mouseClickSource;
    public static CursorManager Instance;
    public Texture2D attackCursor, grab, heal,normal;
    public float selectionStartRadius = 6f;
    public float selectionRadiusSizeIncrease = 2f;

    // Actions
    public Action<Vector3> OnMouseLeftClick;
    public Action<Vector3> OnMouseRightClick;


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

    private void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        if (leftClick)
        {
            LeftClick();
        }
        bool rightClick = Input.GetMouseButtonDown(1);
        if (rightClick)
        {
            RightClick();
        }
    }

    private void RightClick()
    {
        try
        {
            PlayMouseClick();
            ShowMouseClickCloud();
            var mousePosition = CamController.Instance.GetCursorWorldPosition();
            OnMouseRightClick?.Invoke(mousePosition);
        }
        catch (Exception e)
        {
            Debug.LogError($"No OnMouseLeftClick action found: {e}");
        }
    }

    private void LeftClick()
    {
        try
        {
            PlayMouseClick();
            ShowMouseClickCloud();
            var mousePosition = CamController.Instance.GetCursorWorldPosition();
            OnMouseLeftClick?.Invoke(mousePosition);
        }
        catch (Exception e )
        {
            Debug.LogError($"No OnMouseLeftClick action found: {e}");
        }
        
    }

    public void UseCustomCursor(string cursor)
    {
        Texture2D crsr = null;

        switch (cursor)
        {
            case "Attack":
                crsr = attackCursor;
                break;
            case "Grab":
                crsr = grab;
                break;
            default:
                crsr = normal;
                break;
        }

        Cursor.SetCursor(crsr, new Vector2(0.1f, 0.1f), CursorMode.ForceSoftware);

        Cursor.visible = true;
    }

    private void PlayMouseClick()
    {
        mouseClickSource.Play();
    }
    private void ShowMouseClickCloud()
    {
        mouseClickParticle.transform.position = CamController.Instance.GetCursorWorldPosition();
        mouseClickParticle.GetComponentInChildren<ParticleSystem>().Play();
    }
}
