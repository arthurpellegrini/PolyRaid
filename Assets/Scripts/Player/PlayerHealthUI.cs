using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform HeathUI;
    [SerializeField] private PlayerNetworkHealth playerNetworkHealth;

    // void OnEnable()
    // {
    //     playerNetworkHealth.GetHealthPoint().OnValueChanged += HealthChanged;
    // }
    //
    // void OnDisable()
    // {
    //     playerNetworkHealth.GetHealthPoint().OnValueChanged += HealthChanged;
    // }
    //
    // private void HealthChanged(int previousValue, int newValue)
    // {
    //     
    // }
    private void Update()
    {
        HeathUI.transform.localScale = new Vector3(playerNetworkHealth.GetHealthPoint() / 100.0f, 1.0f, 1.0f);
    }
}
