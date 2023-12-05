using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateReader : MonoBehaviour
{
    public BattleUnit Unit;
    public TMP_Text m_text;

    void Update()
    {
        m_text.text = Unit.GetState();
    }
}
