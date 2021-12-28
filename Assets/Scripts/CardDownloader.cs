using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardDownloader
{
    private string jsonURL = "https://drive.google.com/uc?export=download&id=17-0tdOtGYKTwjltv4985p6Eed9jwxhnG";
    private Dictionary<string, Texture> _dataDictionary = new Dictionary<string, Texture>();

    public IEnumerator GetData(Action<Dictionary<string, Texture>> dic)
    {
        UnityWebRequest request = UnityWebRequest.Get(jsonURL);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            //if you want to see json text uncomment this -->
            // Debug.Log(request.downloadHandler.text);

            DataCards dataCards = JsonUtility.FromJson<DataCards>(request.downloadHandler.text);
            for (int i = 0; i < dataCards.Cards.Length; i++)
            {
                yield return GetImage(dataCards.Cards[i].ImageURL,
                    texture => { _dataDictionary.Add(dataCards.Cards[i].Name, texture); });
            }

            dic.Invoke(_dataDictionary);
        }
        else
            Debug.LogError(request.error);

        request.Dispose();
    }

    private IEnumerator GetImage(string url, Action<Texture> tex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            tex.Invoke(texture);
        }
        else
            Debug.LogError(request.error);

        request.Dispose();
    }
}