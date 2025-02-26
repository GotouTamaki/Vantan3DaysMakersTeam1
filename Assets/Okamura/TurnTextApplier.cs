using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnTextApplier : MonoBehaviour
{
    TMP_Text _turnText;
    private void Start()
    {
        _turnText = GetComponent<TMP_Text>();
    }
    private void FixedUpdate()
    {
        _turnText.text = $"{GameManager.Instance.PlayerTurnCount}";
    }
}
