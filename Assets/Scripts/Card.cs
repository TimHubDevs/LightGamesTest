using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _cardButton;
    [SerializeField] public Image _image;
    
    public Action onPressCard;
    public string Name;
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

    public void SetupSprite(Texture2D face, Texture2D rubashka)
    {
        _rubashkaSprite = Sprite.Create(rubashka, new Rect(0,0, rubashka.width, rubashka.height), Vector2.zero);
        _faceSprite = Sprite.Create(face, new Rect(0,0, face.width, face.height), Vector2.zero);
        _image.sprite = _rubashkaSprite;
    }
    
    public void ShowCardFace()
    {
        _image.sprite = _faceSprite;
    }

    public void HideFace()
    {
        _image.sprite = _rubashkaSprite;
    }

    public void DestroySprite()
    {
        _image.sprite = null;
        _image.color = new Color(1,1,1,0);
    }

    public IEnumerator FirstHideFace()
    {
        _cardButton.enabled = false;
        _image.sprite = _faceSprite;
        yield return new WaitForSeconds(5);
        HideFace();
        _cardButton.enabled = true;
    }
}