using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CardSpawner _cardSpawner;
    [SerializeField] private GameObject _loadingPanel;

    [HideInInspector] public List<Card> _cards = new List<Card>();
    [HideInInspector] public int pairCount;
    [HideInInspector] public int maxPairCount;
    public Action onPairCountChange;

    private int _countOpenCard;
    private int _entryCount;
    private bool _isCanGame;
    private string _firstCardName;
    private string _secondCardName;
    private List<CardInfo> _cardsInfo = new List<CardInfo>();
    private List<Card> _openCards = new List<Card>();

    CardDownloader cardDownloader = new CardDownloader();

    private void Awake()
    {
        _loadingPanel.SetActive(true);
        InitGame();
    }

    private void SubscribeCard()
    {
        foreach (var card in _cards)
        {
            card._cardButton.enabled = false;

            StartCoroutine(card.FirstHideFace());

            card.onPressCard += () =>
            {
                CheckCountOpenCard();

                if (!_isCanGame)
                    return;

                card._cardButton.enabled = false;
                StartCoroutine(card.ShowCardFace(() =>
                {
                    _openCards.Add(card);

                    FillOpenCardsName(card);

                    CheckResult();
                }));
            };
        }
    }

    private void InitGame()
    {
        if (_entryCount != 1)
        {
            StartCoroutine(DownloadCard());
            _entryCount++;
        }
        else
        {
            InitCardSpawner();
        }
    }

    private void InitCardSpawner()
    {
        _cards.Clear();
        _cardSpawner.SpawnCard(_cardsInfo);
        maxPairCount = _cards.Count / 2;
        onPairCountChange.Invoke();
        SubscribeCard();
        _loadingPanel.SetActive(false);
    }

    public void RestartGame()
    {
        InitGame();
    }

    private IEnumerator DownloadCard()
    {
        yield return cardDownloader.GetData(data =>
        {
            foreach (var kvp in data)
            {
                var ci = new CardInfo();
                ci.Name = kvp.Key;
                ci.Texture = kvp.Value;
                if (!_cardsInfo.Contains(ci))
                {
                    _cardsInfo.Add(ci);
                }
            }
        });
        InitCardSpawner();
    }


    private void CheckCountOpenCard()
    {
        _isCanGame = false;
        if (_countOpenCard <= 1)
        {
            _countOpenCard++;
            _isCanGame = true;
        }
        else
        {
            _countOpenCard = 0;
            _isCanGame = false;
        }
    }

    private void FillOpenCardsName(Card card)
    {
        if (_firstCardName == String.Empty)
        {
            _firstCardName = card.Name;
        }
        else if (_firstCardName != String.Empty && _secondCardName == String.Empty)
        {
            _secondCardName = card.Name;
        }
        else if (_firstCardName != String.Empty && _secondCardName != String.Empty)
        {
            _firstCardName = card.Name;
            _secondCardName = String.Empty;
        }
    }

    private void CheckResult()
    {
        if (_firstCardName == _secondCardName)
        {
            //win condition
            pairCount++;
            onPairCountChange.Invoke();
            foreach (var openCard in _openCards)
            {
                StartCoroutine(openCard.DestroySprite(() =>
                {
                    if (pairCount == maxPairCount)
                    {
                        //win
                        pairCount = 0;
                        onPairCountChange.Invoke();

                        foreach (var card in _cards)
                        {
                            Destroy(card.gameObject);
                        }

                        RestartGame();
                    }

                    _openCards.Clear();
                }));
            }
        }
        else if (_secondCardName == String.Empty)
        {
            //nothing
        }
        else
        {
            foreach (var openCard in _openCards)
            {
                StartCoroutine(openCard.HideFace());
            }

            _openCards.Clear();
        }
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void UnSubscribe()
    {
        foreach (var card in _cards)
        {
            card.onPressCard -= () => { };
        }
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}