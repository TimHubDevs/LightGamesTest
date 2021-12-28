using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _root;
    [SerializeField] private GameManager _gameManager;
    
    private readonly string NAME_RUBASHKA = "Rubashka";
    private Texture2D _rubashkaTexture2D;

    public void SpawnCard(List<CardInfo> dataCards)
    {
        for (var i = 0; i < dataCards.Count; i++)
        {
            if (dataCards[i].Name == NAME_RUBASHKA)
            {
                _rubashkaTexture2D = (Texture2D) dataCards[i].Texture;
            }
            else
            {
                //twice our instance for game ^^
                InstanceCard(dataCards, i);
                InstanceCard(dataCards, i);
            }
        }
    }

    private void InstanceCard(List<CardInfo> dataCards, int i)
    {
        var cardObject = Instantiate(_cardPrefab, _root);
        var card = cardObject.GetComponent<Card>();
        card.Name = dataCards[i].Name;
        Texture2D faceTexture2D = (Texture2D) dataCards[i].Texture;
        card.SetupSprite(faceTexture2D, _rubashkaTexture2D);
        int random = Random.Range(0, 6);
        cardObject.transform.SetSiblingIndex(random);
        _gameManager._cards.Add(card);
    }
}