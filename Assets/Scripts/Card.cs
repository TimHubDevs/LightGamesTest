using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] public Button _cardButton;
    [SerializeField] public Image _image;
    [SerializeField] public Animator _animator;
    
    public Action onPressCard;
    public string Name;
    public readonly string RotateCard = "RotateCard";
    public readonly string ShakeCard = "ShakeCard";
    public readonly string DestroyCard = "DestroyCard";
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
    
    public IEnumerator ShowCardFace(Action callback)
    {
        _animator.Play(RotateCard,-1, 0f);
        yield return new WaitForSeconds(0.5f);
        _image.sprite = _faceSprite;
        yield return new WaitForSeconds(0.5f);
        callback.Invoke();
    }

    public IEnumerator HideFace()
    {
        _animator.Play(ShakeCard);
        yield return new WaitForSeconds(1.2f);
        _animator.Play(RotateCard);
        yield return new WaitForSeconds(0.5f);
        _image.sprite = _rubashkaSprite;
        yield return new WaitForSeconds(0.5f);
        _cardButton.enabled = true;
    }

    public IEnumerator DestroySprite(Action callback)
    {
        _animator.Play(DestroyCard);
        yield return new WaitForSeconds(1f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        callback.Invoke();
    }

    public IEnumerator FirstHideFace()
    {
        _animator.Play(RotateCard);
        _image.sprite = _faceSprite;
        yield return new WaitForSeconds(5);
        StartCoroutine(HideFace());
    }
}