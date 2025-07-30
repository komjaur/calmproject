using UnityEngine;
using UnityEngine.UIElements;
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button _button;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        _button = uiDocument.rootVisualElement.Q<Button>("Button_Play");
        _button.clicked += OnStartButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked");
    }
}
