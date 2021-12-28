using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _gameManager.onPairCountChange += UpdateText;
    }

    private void UpdateText()
    {
        _text.text = "Pair " + _gameManager.pairCount + " : " + _gameManager.maxPairCount;
    }

}
