using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private PlayerManager playerManager;

    public GameObject cardItem;

    private List<Card> cards;
    private List<CustomCard> customCards;

    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        cards = new List<Card>();
        customCards = new List<CustomCard>();
    }


    public void InitCards(int categoryId)
    {
        GameObject.Find("Canvas").transform.Find("CategoriesScrollView").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("CardsScrollView").gameObject.SetActive(true);

        if(cards.Count > 0)
        {
            GameObject[] content = GameObject.FindGameObjectsWithTag("cardItem");
            for (int i = 0; i< content.Length; i++)
            {
                Destroy(content[i]);
            }
            cards.Clear();
            customCards.Clear();
        }

        GetBuitInCardsFromServer(categoryId);

        if (playerManager.GetUserId() >= 0)
        {
            GetCustomCardsFromServer(categoryId, playerManager.GetUserId());
        }

    }


    public void GetBuitInCardsFromServer(int categoryId)
    {
        CardData cardData = new CardData();
        cardData.category_id = categoryId;

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
                cards.Add(tmp);
            }

            CreateNewCardItems(cards);

        }));
    }

    private void CreateNewCardItems(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject newCardItem = Instantiate(cardItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCardItem.transform.SetParent(GameObject.Find("Canvas").transform.Find("CardsScrollView/Viewport/Content").transform);
            newCardItem.transform.localScale = new Vector3(1, 1, 1);
            newCardItem.GetComponent<Card>().SetCardId(cards[i].GetCardId());
            newCardItem.GetComponent<Card>().SetImagePath(cards[i].GetImagePath());
            newCardItem.GetComponent<Card>().SetName(cards[i].GetName());

            newCardItem.GetComponentInChildren<Text>().text = cards[i].GetName();
            StartCoroutine(getImagesFromURL(cards[i].GetImagePath(), newCardItem));
        }
    }

    public List<Card> GetBuitInCards()
    {
        return cards;
    }

    public Card GetBuiltInCardInfo(int id)
    {
        foreach (Card tmp in cards)
        {
            if (tmp.GetCardId().Equals(id))
                return tmp;
        }

        return null;
    }



    public void GetCustomCardsFromServer(int categoryId, int userId)
    {
        CardData cardData = new CardData();
        cardData.category_id = categoryId;
        cardData.user_id = userId;

        var req = JsonConvert.SerializeObject(cardData);

        StartCoroutine(DataManager.sendDataToServer("/category/custom", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            foreach (JObject tmpCard in applyJObj["cards"])
            {
                CustomCard tmp = new CustomCard();
                tmp.SetCardId((int)tmpCard["card_id"]);
                tmp.SetName(tmpCard["card_name"].ToString());
                tmp.SetUserId(userId);
                tmp.SetCategoryId(categoryId);
                tmp.SetImagePath(tmpCard["card_img_path"].ToString());

                customCards.Add(tmp);
            }

            CreateNewCustomCardItems(customCards);

        }));
    }

    private void CreateNewCustomCardItems(List<CustomCard> customCards)
    {
        for (int i = 0; i < customCards.Count; i++)
        {
            GameObject newCardItem = Instantiate(cardItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCardItem.transform.SetParent(GameObject.Find("Content").transform);
            newCardItem.transform.localScale = new Vector3(1, 1, 1);
            newCardItem.GetComponent<CustomCard>().SetCustomCardIdd(customCards[i].GetCustomCardId());
            newCardItem.GetComponent<CustomCard>().SetImagePath(customCards[i].GetImagePath());
            newCardItem.GetComponent<CustomCard>().SetName(customCards[i].GetName());

            newCardItem.GetComponentInChildren<Text>().text = customCards[i].GetName();
            StartCoroutine(getImagesFromURL(customCards[i].GetImagePath(), newCardItem));
        }
    }

    public List<CustomCard> GetCustomCards()
    {
        return customCards;
    }

    public CustomCard GetCustomCardInfo(int id)
    {
        foreach (CustomCard tmp in customCards)
        {
            if (tmp.GetCustomCardId().Equals(id))
                return tmp;
        }

        return null;
    }

    private IEnumerator getImagesFromURL(string imgurl, GameObject item)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imgurl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            item.GetComponentInChildren<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
