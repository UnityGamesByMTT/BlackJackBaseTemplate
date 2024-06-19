using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BJController : MonoBehaviour
{

    [Header("Transforms")]
    [SerializeField]
    private Transform ChipsParent_Transform;
    [SerializeField]
    private Transform PlayerContainer_Transform;
    [SerializeField]
    private Transform DealerContainer_Transform;
    [SerializeField]
    private Transform Deck_Transform;
    [SerializeField]
    private Transform FirstSplit_Transform;
    [SerializeField]
    private Transform SecondSplit_Transform;

    [Header("Lists and Arrays")]
    [SerializeField]
    private Transform[] CoinContainers_Transform;
    [SerializeField]
    private GameObject[] Coins_Prefab;
    [SerializeField]
    private GameObject[] activeCoins_Object;
    [SerializeField]
    private List<GameObject> instantiated_Coins;
    [SerializeField]
    private List<int> instantiated_Value;

    [Header("Integers")]
    [SerializeField]
    private int CoinCounter = 0;
    [SerializeField]
    private int[] amount_array;
    [SerializeField]
    private int totalBet = 0;
    [SerializeField]
    private int dealerTotal = 0;
    [SerializeField]
    private int playerTotal = 0;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject Cards_Prefab;

    [Header("Card Sprites")]
    [SerializeField]
    private Sprite[] clubs_Sprite;
    [SerializeField]
    private Sprite[] spades_Sprite;
    [SerializeField]
    private Sprite[] hearts_Sprite;
    [SerializeField]
    private Sprite[] diamonds_Sprite;

    [Header("Texts")]
    [SerializeField]
    private TMP_Text TotalBet_Text;
    [SerializeField]
    private TMP_Text Balance_Text;
    [SerializeField]
    private TMP_Text PlayerTotal_Text;
    [SerializeField]
    private TMP_Text DealerTotal_Text;
    [SerializeField]
    private TMP_Text FirstSplitTotal_Text;
    [SerializeField]
    private TMP_Text SecondSplitTotal_Text;

    [Header("Test Data")]
    [SerializeField]
    internal List<int> playerData;
    [SerializeField]
    internal List<int> firstplayerData;
    [SerializeField]
    internal List<int> secondplayerData;
    [SerializeField]
    internal List<int> dealerData;

    private int totalValue = 0;
    private int FirsttotalValue = 0;
    private int SecondtotalValue = 0;
    private int totaldealerValue = 0;
    private int playerCounter = 0;
    private int FirstSplitplayerCounter = 0;
    private int SecondSplitplayerCounter = 0;
    private int dealerCounter = 0;

    internal bool isFlippin = false;
    internal bool isSplit = false;
    internal bool isFirstSplit = false;

    private CardScript tempdealer = null;

    private void Start()
    {
        if (activeCoins_Object[CoinCounter]) activeCoins_Object[CoinCounter].SetActive(true);
        if (FirstSplit_Transform) FirstSplit_Transform.gameObject.SetActive(false);
        if (SecondSplit_Transform) SecondSplit_Transform.gameObject.SetActive(false);
    }

    internal void SelectCoin(int counter)
    {
        foreach (GameObject objs in activeCoins_Object)
        {
            objs.SetActive(false);
        }
        activeCoins_Object[counter].SetActive(true);
        CoinCounter = counter;
    }

    internal void DoubleBetButton()
    {
        totalBet *= 2;
        if (TotalBet_Text) TotalBet_Text.text = totalBet.ToString();
    }

    internal void BetOnButton()
    {
        GameObject coin = Instantiate(Coins_Prefab[CoinCounter], CoinContainers_Transform[CoinCounter]);
        coin.transform.localPosition = Vector2.zero;
        coin.transform.SetParent(ChipsParent_Transform);
        coin.transform.DOLocalMove(new Vector2(0, 0), 0.5f);
        instantiated_Coins.Add(coin);
        instantiated_Value.Add(amount_array[CoinCounter]);
        totalBet += amount_array[CoinCounter];
        if (TotalBet_Text) TotalBet_Text.text = totalBet.ToString();
        Canvas.ForceUpdateCanvases();
    }

    internal void ClearBetButton()
    {
        foreach(GameObject coin in instantiated_Coins)
        {
            Destroy(coin);
        }
        instantiated_Coins.Clear();
        instantiated_Coins.TrimExcess();
        instantiated_Value.Clear();
        instantiated_Value.TrimExcess();
        totalBet = 0;
        if (TotalBet_Text) TotalBet_Text.text = totalBet.ToString();
    }

    private void ClearCards()
    {
        for (int i = 1; i < PlayerContainer_Transform.childCount; i++)
        {
            Destroy(PlayerContainer_Transform.GetChild(i).gameObject);
        }

        for (int i = 1; i < DealerContainer_Transform.childCount; i++)
        {
            Destroy(DealerContainer_Transform.GetChild(i).gameObject);
        }

        for (int i = 1; i < FirstSplit_Transform.childCount; i++)
        {
            Destroy(FirstSplit_Transform.GetChild(i).gameObject);
        }

        for (int i = 1; i < SecondSplit_Transform.childCount; i++)
        {
            Destroy(SecondSplit_Transform.GetChild(i).gameObject);
        }
        playerCounter = 0;
        dealerCounter = 0;
        FirstSplitplayerCounter = 0;
        SecondSplitplayerCounter = 0;
        totalValue = 0;
        totaldealerValue = 0;
        FirsttotalValue = 0;
        SecondtotalValue = 0;
        if (PlayerTotal_Text) PlayerTotal_Text.text = "0";
        if (DealerTotal_Text) DealerTotal_Text.text = "0";
        if (FirstSplitTotal_Text) FirstSplitTotal_Text.text = "0";
        if (SecondSplitTotal_Text) SecondSplitTotal_Text.text = "0";
        if (FirstSplit_Transform) FirstSplit_Transform.gameObject.SetActive(false);
        if (SecondSplit_Transform) SecondSplit_Transform.gameObject.SetActive(false);
        if (PlayerContainer_Transform) PlayerContainer_Transform.gameObject.SetActive(false);
        isSplit = false;
        isFirstSplit = false;
    }

    internal void SplitButton()
    {
        if (FirstSplit_Transform) FirstSplit_Transform.gameObject.SetActive(true);
        if (SecondSplit_Transform) SecondSplit_Transform.gameObject.SetActive(true);
        GameObject tempCard = PlayerContainer_Transform.GetChild(1).gameObject;
        tempCard.transform.SetParent(FirstSplit_Transform);
        tempCard.transform.DOLocalMove(new Vector2(0, 0), 0.3f);
        tempCard = PlayerContainer_Transform.GetChild(1).gameObject;
        tempCard.transform.SetParent(SecondSplit_Transform);
        tempCard.transform.DOLocalMove(new Vector2(0, 0), 0.3f);
        if (FirstSplitTotal_Text) FirstSplitTotal_Text.text = SplitPlayerNumberValue(firstplayerData[FirstSplitplayerCounter], true);
        string temp1 = SplitPlayerNumberValue(FirstSplitplayerCounter, true);
        Debug.Log("my valus is " + temp1);
        if (SecondSplitTotal_Text) SecondSplitTotal_Text.text = SplitPlayerNumberValue(secondplayerData[SecondSplitplayerCounter], false);
        FirstSplitplayerCounter++;
        SecondSplitplayerCounter++;
        if (PlayerContainer_Transform) PlayerContainer_Transform.gameObject.SetActive(false);
        isSplit = true;
        isFirstSplit = true;
    }

    internal void RebetButton()
    {
        ClearBetButton();
        ClearCards();
    }

    internal void RebetDealButton()
    {
        ClearCards();
    }

    internal void UndoBetButton()
    {
        if (instantiated_Coins.Count > 0)
        {
            Destroy(instantiated_Coins[instantiated_Coins.Count - 1]);
            instantiated_Coins.RemoveAt(instantiated_Coins.Count - 1);
        }
        if (instantiated_Value.Count > 0)
        {
            totalBet -= instantiated_Value[instantiated_Value.Count - 1];
            instantiated_Value.RemoveAt(instantiated_Value.Count - 1);
        }
        if (TotalBet_Text) TotalBet_Text.text = totalBet.ToString();
    }

    internal void OnDealerButton()
    {
        GameObject card = Instantiate(Cards_Prefab, Deck_Transform);
        card.transform.localPosition = Vector2.zero;
        card.transform.SetParent(DealerContainer_Transform);
        Sprite tempArr = SelectRandomArray(dealerData[dealerCounter]);
        card.transform.DOLocalMove(new Vector2(0, 0), 0.3f).OnComplete(delegate
        {
            card.GetComponent<CardScript>().OnFlipMethod(tempArr, 2);
        });
    }

    internal void OnDealerButtonClosedCard()
    {
        GameObject card = Instantiate(Cards_Prefab, Deck_Transform);
        card.transform.localPosition = Vector2.zero;
        card.transform.SetParent(DealerContainer_Transform);
        Sprite tempArr = SelectRandomArray(dealerData[dealerCounter]);
        card.transform.DOLocalMove(new Vector2(0, 0), 0.3f).OnComplete(delegate
        {
            card.GetComponent<LayoutElement>().ignoreLayout = false;
            tempdealer = card.GetComponent<CardScript>();
        });
    }

    internal void OnDealerButtonOpen()
    {
        Sprite tempArr = SelectRandomArray(dealerData[dealerCounter]);
        tempdealer.OnFlipMethod(tempArr, 2);
    }

    internal void OnPlayerDealButton()
    {
        GameObject card = Instantiate(Cards_Prefab, Deck_Transform);
        card.transform.localPosition = Vector2.zero;
        card.transform.SetParent(PlayerContainer_Transform);
        Sprite tempArr = SelectRandomArray(playerData[playerCounter]);
        card.transform.DOLocalMove(new Vector2(0, 0), 0.3f).OnComplete(delegate
        {
            card.GetComponent<CardScript>().OnFlipMethod(tempArr, 1);
        });
    }

    internal void SplitStandButton()
    {
        isFirstSplit = false;
    }

    internal void OnSplitDealButton()
    {
        if (isFirstSplit)
        {
            GameObject card = Instantiate(Cards_Prefab, Deck_Transform);
            card.transform.localPosition = Vector2.zero;
            card.transform.SetParent(FirstSplit_Transform);
            Sprite tempArr = SelectRandomArray(firstplayerData[FirstSplitplayerCounter]);
            card.transform.DOLocalMove(new Vector2(0, 0), 0.3f).OnComplete(delegate
            {
                card.GetComponent<CardScript>().OnFlipMethod(tempArr, 3);
            });
        }
        else
        {
            GameObject card = Instantiate(Cards_Prefab, Deck_Transform);
            card.transform.localPosition = Vector2.zero;
            card.transform.SetParent(SecondSplit_Transform);
            Sprite tempArr = SelectRandomArray(secondplayerData[SecondSplitplayerCounter]);
            card.transform.DOLocalMove(new Vector2(0, 0), 0.3f).OnComplete(delegate
            {
                card.GetComponent<CardScript>().OnFlipMethod(tempArr, 4);
            });
        }
    }

    internal void AfterCardFlip(int value)
    {
        switch(value)
        {
            case 1:
                if (PlayerTotal_Text) PlayerTotal_Text.text = PlayerNumberValue(playerData[playerCounter]);
                playerCounter++;
                break;
            case 2:
                if (DealerTotal_Text) DealerTotal_Text.text = DealerNumberValue(dealerData[dealerCounter]);
                dealerCounter++;
                break;
            case 3:
                if (FirstSplitTotal_Text) FirstSplitTotal_Text.text = SplitPlayerNumberValue(firstplayerData[FirstSplitplayerCounter],true);
                FirstSplitplayerCounter++;
                break;
            case 4:
                if (SecondSplitTotal_Text) SecondSplitTotal_Text.text = SplitPlayerNumberValue(secondplayerData[SecondSplitplayerCounter],false);
                SecondSplitplayerCounter++;
                break;
        }
        isFlippin = false;
    }

    private string PlayerNumberValue(int value)
    {
        switch (value)
        {
            case 1:
                if (totalValue > 10)
                {
                    totalValue += value;
                    return totalValue.ToString();
                }
                else
                {
                    totalValue += value;
                    string specialval = totalValue.ToString() + "/" + (totalValue + 9).ToString();
                    return specialval.ToString();
                }
            default:
                totalValue += value;
                return totalValue.ToString();
        }
    }

    private string SplitPlayerNumberValue(int value, bool isFirst)
    {
        if (isFirst)
        {
            switch (value)
            {
                case 1:
                    if (FirsttotalValue > 10)
                    {
                        FirsttotalValue += value;
                        return FirsttotalValue.ToString();
                    }
                    else
                    {
                        FirsttotalValue += value;
                        string specialval = FirsttotalValue.ToString() + "/" + (FirsttotalValue + 9).ToString();
                        return specialval.ToString();
                    }
                default:
                    FirsttotalValue += value;
                    return FirsttotalValue.ToString();
            }
        }
        else
        {
            switch (value)
            {
                case 1:
                    if (SecondtotalValue > 10)
                    {
                        SecondtotalValue += value;
                        return SecondtotalValue.ToString();
                    }
                    else
                    {
                        SecondtotalValue += value;
                        string specialval = SecondtotalValue.ToString() + "/" + (SecondtotalValue + 9).ToString();
                        return specialval.ToString();
                    }
                default:
                    SecondtotalValue += value;
                    return SecondtotalValue.ToString();
            }
        }
    }

    private string DealerNumberValue(int value)
    {
        switch (value)
        {
            case 1:
                if (totaldealerValue > 10)
                {
                    totaldealerValue += value;
                    return totaldealerValue.ToString();
                }
                else
                {
                    totaldealerValue += value;
                    string specialval = totaldealerValue.ToString() + "/" + (totaldealerValue + 9).ToString();
                    return specialval.ToString();
                }
            default:
                totaldealerValue += value;
                return totaldealerValue.ToString();
        }
    }

    private Sprite SelectRandomArray(int value)
    {
        int randomIndex = Random.Range(0, 4); // Generates a random number between 0 and 3
        Sprite[] temparr = null;
        switch (randomIndex)
        {
            case 0:
                temparr = clubs_Sprite;
                break;
            case 1:
                temparr = spades_Sprite;
                break;
            case 2:
                temparr = hearts_Sprite;
                break;
            case 3:
                temparr = diamonds_Sprite;
                break;
        }

        int randomNumber = Random.Range(9, 12);
        switch (value)
        {
            case 10:
                return temparr[randomNumber];
            default:
                return temparr[value - 1];
        }
    }
}
