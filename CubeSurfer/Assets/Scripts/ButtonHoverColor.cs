using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _buttonText;
    private Color _baseColor;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.Instance;

        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        _baseColor = _buttonText.color;
    }

    void OnDisable() => SetBaseColor();

    public void OnPointerEnter(PointerEventData eventData) => SetHoverColor();

    public void OnPointerExit(PointerEventData eventData) => SetBaseColor();

    private void SetBaseColor() => _buttonText.color = _baseColor;

    private void SetHoverColor() => _buttonText.color = _gameManager.Theme.backgroundColor;
}
