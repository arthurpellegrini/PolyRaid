using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform HeathUI;
    [SerializeField] private PlayerNetworkHealth playerNetworkHealth;

    void OnEnable()
    {
        playerNetworkHealth.GetHealthPoint().OnValueChanged += HealthChanged;
    }
    
    void OnDisable()
    {
        playerNetworkHealth.GetHealthPoint().OnValueChanged += HealthChanged;
    }

    private void HealthChanged(int previousValue, int newValue)
    {
        HeathUI.transform.localScale = new Vector3(newValue / 100.0f, 1.0f, 1.0f);
    }
}
