using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonHandle : MonoBehaviour
{
    [SerializeField] private Button CreatePlayerButton;

    private DefaultPlayer _player;

    void Awake()
    {
        if (CreatePlayerButton == null)
            return;
    }

    void FixedUpdate()
    {
        _player = FindObjectOfType<DefaultPlayer>();
        if (CreatePlayerButton != null)
            CreatePlayerButton.interactable = (_player == null);
    }
}
