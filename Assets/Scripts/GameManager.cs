using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int pairCount = 0;
    private int maxPairCount;
    private bool _isCanGame;
    private List<CardInfo> _cardsInfo = new List<CardInfo>();
    private List<Card> _openCards = new List<Card>();
    public List<Card> _cards = new List<Card>();
    private string _firstCardName;
    private string _secondCardName;
    private int _countOpenCard = 0;
    private CardSpawner _cardSpawner;
    CardDownloader cardDownloader = new CardDownloader();
    private int _entryCount = 0;


    private void Awake()
    {
        _cardSpawner = FindObjectOfType<CardSpawner>();
        InitGame();
    }

    private void SubscribeCard()
    {
        Debug.Log("SubscribeCard");
        foreach (var card in _cards)
        {
            card.onPressCard += () =>
            {
                CheckCountOpenCard();

                if (!_isCanGame)
                    return;

                card.ShowCardFace();

                _openCards.Add(card);

                FillOpenCardsName(card);

                CheckResult();
            };
        }
    }

    private void InitGame()
    {
        Debug.Log("InitGame");

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
        Debug.Log("InitCardSpawner");

        _cardSpawner.SpawnCard(_cardsInfo);
        maxPairCount = _cards.Count / 2;
        SubscribeCard();
    }

    public void RestartGame()
    {
        InitGame();
    }

    private IEnumerator DownloadCard()
    {
        Debug.Log("DownloadCard");
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
            Debug.Log($"pairCount {pairCount} +++++ maxPairCount {maxPairCount}");

            if (pairCount == maxPairCount)
            {
                //win
                Debug.Log("Win ==== Restart");
                pairCount = 0;
                RestartGame();
            }

            foreach (var openCard in _openCards)
            {
                Destroy(openCard.gameObject);
            }

            _openCards.Clear();
        }
        else if (_secondCardName == String.Empty)
        {
            Debug.Log("One card");
        }
        else
        {
            foreach (var openCard in _openCards)
            {
                Debug.Log("Hide card");

                openCard.HideFace();
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
            card.onPressCard -= () =>
            {
                CheckCountOpenCard();

                if (!_isCanGame)
                    return;

                card.ShowCardFace();

                _openCards.Add(card);

                FillOpenCardsName(card);

                CheckResult();
            };
        }
    }
}