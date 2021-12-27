using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _cardButton;
    [SerializeField] public Image _image;
    public Action onPressCard;
    public string Name;
    public Texture2D rubashkaTexture2D;
    public Texture2D faceTexture2D;
    private Sprite _rubashkaSprite;
    private Sprite _faceSprite;

    private void Awake()
    {
        _cardButton.onClick.AddListener(PressCard);
    }

    private void PressCard()
    {
        onPressCard.Invoke();
    }

    public void SetupSprite()
    {
        _rubashkaSprite = Sprite.Create(rubashkaTexture2D, new Rect(0,0, rubashkaTexture2D.width, rubashkaTexture2D.height), Vector2.zero);
        _faceSprite = Sprite.Create(faceTexture2D, new Rect(0,0, faceTexture2D.width, faceTexture2D.height), Vector2.zero);
        _image.sprite = _rubashkaSprite;
    }
    
    public void ShowCardFace()
    {
        Debug.Log($"Show Face {Name}");
        _image.sprite = _faceSprite;
    }

    public void HideFace()
    {
        Debug.Log($"Hide Face {Name}");
        _image.sprite = _rubashkaSprite;
    }
}