using UnityEngine;
using UnityEngine.UIElements;
using SurvivalChaos;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _doc;
    private VisualTreeAsset _mainMenu;
    private VisualTreeAsset _queueMenu;
    private VisualTreeAsset _settingsMenu;
    private VisualTreeAsset _profileMenu;

    private User _player;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _mainMenu = Resources.Load<VisualTreeAsset>("UI/MainMenu");
        _queueMenu = Resources.Load<VisualTreeAsset>("UI/QueueMenu");
        _settingsMenu = Resources.Load<VisualTreeAsset>("UI/SettingsMenu");
        _profileMenu = Resources.Load<VisualTreeAsset>("UI/ProfileMenu");

        _player = new User("Player") { Rank = 1 };
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        _doc.visualTreeAsset = _mainMenu;
        _doc.rootVisualElement.Q<Button>("Button_Play").clicked += ShowQueueMenu;
        _doc.rootVisualElement.Q<Button>("Button_Settings").clicked += ShowSettingsMenu;
        _doc.rootVisualElement.Q<Button>("Button_Profile").clicked += ShowProfileMenu;
    }

    private void ShowQueueMenu()
    {
        _doc.visualTreeAsset = _queueMenu;
        _doc.rootVisualElement.Q<Button>("Button_QueuePlayers").clicked += QueueWithPlayers;
        _doc.rootVisualElement.Q<Button>("Button_QueueBots").clicked += QueueWithBots;
        _doc.rootVisualElement.Q<Button>("Button_Back").clicked += ShowMainMenu;
    }

    private void ShowSettingsMenu()
    {
        _doc.visualTreeAsset = _settingsMenu;
        _doc.rootVisualElement.Q<Button>("Button_Back").clicked += ShowMainMenu;
    }

    private void ShowProfileMenu()
    {
        _doc.visualTreeAsset = _profileMenu;
        _doc.rootVisualElement.Q<Label>("Label_Rank").text = $"Rank: {_player.Rank}";
        _doc.rootVisualElement.Q<Button>("Button_Back").clicked += ShowMainMenu;
    }

    private void QueueWithPlayers()
    {
        Debug.Log("Queueing with players");
    }

    private void QueueWithBots()
    {
        Debug.Log("Queueing with bots");
    }
}
