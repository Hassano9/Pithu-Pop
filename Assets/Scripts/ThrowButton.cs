using UnityEngine;
using UnityEngine.UI;

public class ThrowButtonClick : MonoBehaviour
{
    public Button throwButton;

    void Start()
    {
        // Add a listener to detect button clicks
        throwButton.onClick.AddListener(OnThrowClicked);
    }

    void OnThrowClicked()
    {
        Debug.Log("Throw button clicked!");
    }
}
