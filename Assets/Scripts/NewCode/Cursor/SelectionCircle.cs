using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCircle : MonoBehaviour
{
    public float SelectRadius;
    public Transform SelectVisuals;

    private List<BattleUnit> m_selected = new();
    private CircleCollider2D m_collider;

    private void Start()
    {
        m_collider = GetComponent<CircleCollider2D>();
        m_collider.enabled = false;
        m_collider.radius = CursorManager.Instance.selectionStartRadius;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.isTrigger) return;
        BattleUnit unit = collider.GetComponent<BattleUnit>();
        if (unit != null && unit.Selectable && !m_selected.Contains(unit))
        {
            m_selected.Add(unit);
            unit.Select();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.isTrigger) return;

        BattleUnit unit = collider.GetComponent<BattleUnit>();
        if (unit != null && m_selected.Contains(unit))
        {
            m_selected.Remove(unit);
            unit.Deselect();
        }
    }

    public List<BattleUnit> GetSelected()
    {
        return m_selected;
    }

    public void StartSelection()
    {
        m_collider.enabled = true;
        m_collider.radius = SelectRadius;
        SelectVisuals.gameObject.SetActive(true);
        SelectVisuals.localScale = Vector3.one*SelectRadius;
    }

    public void StopSelection()
    {
        for(int n=m_selected.Count-1; n>=0; --n)
        {
            m_selected[n].Deselect();
            m_selected.RemoveAt(n);
        }
        m_collider.enabled = false;
        SelectVisuals.gameObject.SetActive(false);
    }

    public void Upgrade()
    {
        SelectRadius += CursorManager.Instance.selectionRadiusSizeIncrease;
    }
}
