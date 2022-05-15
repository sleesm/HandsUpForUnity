using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public GameObject cardItem;

    private List<Card> cards;
    //private List<Card> customCards;

    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        cards = new List<Card>();
        //customCards = new List<Card>();
    }


    public void InitCards(int categoryId, bool isGame = false)
    {
        string path = "CardViewPage";

        if(cards.Count > 0)
        {
            if (!isGame)
                DestoryCards();
            cards.Clear();
            //customCards.Clear();
        }

        GetCardsFromServer(categoryId, isGame, path);

        if (playerManager.GetUserId() < 0)
            if(isGame)
                GameManager.isCustomCardLoaded = true;

    }

    public void DestoryCards()
    {
        GameObject[] content = GameObject.FindGameObjectsWithTag("cardItem");
        for (int i = 0; i < content.Length; i++)
        {
            Destroy(content[i]);
        }
    }

    public void GetCardsFromServer(int categoryId, bool isGame, string path)
    {
        CardData cardData = new CardData();
        cardData.category_id = categoryId;
        if(playerManager.GetUserId() >= 0)
            cardData.user_id = playerManager.GetUserId();

        var req = JsonConvert.SerializeObject(cardData);

        StartCoroutine(DataManager.sendDataToServer("category/card", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            foreach (JObject tmpCard in applyJObj["cards"])
            {
                Card tmp = new Card();
                tmp.SetCardId((int)tmpCard["card_id"]);
                tmp.SetName(tmpCard["card_name"].ToString());
                tmp.SetCategoryId(categoryId);
                tmp.SetImagePath(tmpCard["card_img_path"].ToString());
                if ((int)tmpCard["card_is_built_in"] == 1)
                    tmp.SetCardIsBuiltIn(true);
                else
                    tmp.SetCardIsBuiltIn(false);

                tmp.SetUserId(-1);
                cards.Add(tmp);
            }

            if(!isGame)
                CreateNewCardItems(cards, "CardViewPage");
            else
                GameManager.isCardLoaded = true;

        }));
    }

    public void CreateNewCardItems(List<Card> cards,  string path)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject newCardItem = Instantiate(cardItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCardItem.transform.SetParent(GameObject.Find("Canvas").transform.Find(path).transform.Find("CardsScrollView/Viewport/Content").transform);
            newCardItem.transform.localScale = new Vector3(1, 1, 1);
            newCardItem.GetComponent<Card>().SetCardId(cards[i].GetCardId());
            newCardItem.GetComponent<Card>().SetImagePath(cards[i].GetImagePath());
            newCardItem.GetComponent<Card>().SetName(cards[i].GetName());
            if (cards[i].GetCardIsBuiltIn())
                newCardItem.GetComponent<Card>().SetCardIsBuiltIn(true);
            else
                newCardItem.GetComponent<Card>().SetCardIsBuiltIn(false);

            newCardItem.GetComponentInChildren<Text>().text = cards[i].GetName();
            StartCoroutine(getImagesFromURL(cards[i].GetImagePath(), newCardItem));
        }
    }

    public List<Card> GetCards()
    {
        return cards;
    }

    public Card GetCardInfo(int id)
    {
        foreach (Card tmp in cards)
        {
            if (tmp.GetCardId().Equals(id))
                return tmp;
        }

        return null;
    }



/*    public void GetCustomCardsFromServer(int categoryId, int userId, bool isGame = false, string path = "")
    {
        CardData cardData = new CardData();
        cardData.category_id = categoryId;
        cardData.user_id = userId;

        var req = JsonConvert.SerializeObject(cardData);

        StartCoroutine(DataManager.sendDataToServer("category/card/custom", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            foreach (JObject tmpCard in applyJObj["cards"])
            {
                Card tmp = new Card();
                tmp.SetCardId((int)tmpCard["card_id"]);
                tmp.SetName(tmpCard["card_name"].ToString());
                tmp.SetCategoryId(categoryId);
                tmp.SetImagePath(tmpCard["card_img_path"].ToString());

                tmp.SetUserId(userId);

                customCards.Add(tmp);
            }

            if (!isGame)
                CreateNewCustomCardItems(customCards, path);
            else
                GameManager.isCustomCardLoaded = true;

        }));
    }

    private void CreateNewCustomCardItems(List<Card> customCards, string path)
    {
        for (int i = 0; i < customCards.Count; i++)
        {
            GameObject newCardItem = Instantiate(cardItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCardItem.transform.SetParent(GameObject.Find("Canvas").transform.Find(path).transform.Find("CardsScrollView/Viewport/Content").transform);
            newCardItem.transform.localScale = new Vector3(1, 1, 1);
            newCardItem.GetComponent<Card>().SetImagePath(customCards[i].GetImagePath());
            newCardItem.GetComponent<Card>().SetName(customCards[i].GetName());

            newCardItem.GetComponentInChildren<Text>().text = customCards[i].GetName();
            StartCoroutine(getImagesFromURL(customCards[i].GetImagePath(), newCardItem));
        }
    }

    public List<Card> GetCustomCards()
    {
        return customCards;
    }

    public Card GetCustomCardInfo(int id)
    {
        foreach (Card tmp in customCards)
        {
            if (tmp.GetCardId().Equals(id))
                return tmp;
        }

        return null;
    }


    public List<Card> GetAllCards()
    {
        List<Card> cardsTmp = new List<Card>();
        foreach (Card tmp in cards)
        {
            cardsTmp.Add(tmp);
        }
        if(customCards.Count > 0)
            foreach (Card tmp in customCards)
            {
                cardsTmp.Add(tmp);
            }

        return cardsTmp;
    }*/

    public IEnumerator getImagesFromURL(string imgurl, GameObject item, bool isGame = false)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imgurl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            if(isGame)
                GameManager.isImgLoaded = true;
            item.GetComponentInChildren<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
