using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Initial Buttons")]
    [SerializeField]
    private Button UndoBet_Button;
    [SerializeField]
    private Button Deal_Button;
    [SerializeField]
    private Button ClearBet_Button;
    [SerializeField]
    private Button DoubleBet_Button;
    [SerializeField]
    private Button MainBet_Button;
    [SerializeField]
    private Button Quit_Button;

    [Header("Middle Buttons")]
    [SerializeField]
    private Button Hit_Button;
    [SerializeField]
    private Button Stand_button;
    [SerializeField]
    private Button DoubleShow_Button;
    [SerializeField]
    private Button Split_Button;

    [Header("Rebet Buttons")]
    [SerializeField]
    private Button Rebet_Button;
    [SerializeField]
    private Button RebetDeal_Button;
    [SerializeField]
    private Button RebetDouble_Button;

    [Header("List & Arrays")]
    [SerializeField]
    private Button[] Chips_Button;

    [Header("GameObjets")]
    [SerializeField]
    private GameObject InitialButtons_object;
    [SerializeField]
    private GameObject MiddleButtons_object;
    [SerializeField]
    private GameObject RebetButtons_object;
    [SerializeField]
    private GameObject MainBet_object;
    [SerializeField]
    private GameObject Player_Object;
    [SerializeField]
    private GameObject Dealer_Object;
    [SerializeField]
    private GameObject ChipBets_Object;
    [SerializeField]
    private GameObject Split_object;
    [SerializeField]
    private GameObject MiddleDouble_object;

    [Header("Managers")]
    [SerializeField]
    private BJController BJmanager;

    private void Start()
    {
        if (MainBet_object) MainBet_object.SetActive(true);
        if (ChipBets_Object) ChipBets_Object.SetActive(true);
        if (Player_Object) Player_Object.SetActive(false);
        if (Dealer_Object) Dealer_Object.SetActive(false);
        for (int i = 0; i < Chips_Button.Length; i++)
        {
            if (Chips_Button[i]) Chips_Button[i].onClick.RemoveAllListeners();
            int j = i;
            if (Chips_Button[i]) Chips_Button[i].onClick.AddListener(delegate { OnSelectBet(j); });
        }

        if (MainBet_Button) MainBet_Button.onClick.RemoveAllListeners();
        if (MainBet_Button) MainBet_Button.onClick.AddListener(OnBet);

        if (Deal_Button) Deal_Button.onClick.RemoveAllListeners();
        if (Deal_Button) Deal_Button.onClick.AddListener(OnDeal);

        if (Hit_Button) Hit_Button.onClick.RemoveAllListeners();
        if (Hit_Button) Hit_Button.onClick.AddListener(delegate { OnHit(); });

        if (Stand_button) Stand_button.onClick.RemoveAllListeners();
        if (Stand_button) Stand_button.onClick.AddListener(OnStand);

        if (ClearBet_Button) ClearBet_Button.onClick.RemoveAllListeners();
        if (ClearBet_Button) ClearBet_Button.onClick.AddListener(OnClear);

        if (UndoBet_Button) UndoBet_Button.onClick.RemoveAllListeners();
        if (UndoBet_Button) UndoBet_Button.onClick.AddListener(OnUndo);

        if (Rebet_Button) Rebet_Button.onClick.RemoveAllListeners();
        if (Rebet_Button) Rebet_Button.onClick.AddListener(OnRebet);

        if (DoubleBet_Button) DoubleBet_Button.onClick.RemoveAllListeners();
        if (DoubleBet_Button) DoubleBet_Button.onClick.AddListener(OnInitialDouble);

        if (DoubleShow_Button) DoubleShow_Button.onClick.RemoveAllListeners();
        if (DoubleShow_Button) DoubleShow_Button.onClick.AddListener(OnDoubleAndStand);

        if (RebetDeal_Button) RebetDeal_Button.onClick.RemoveAllListeners();
        if (RebetDeal_Button) RebetDeal_Button.onClick.AddListener(OnRebetDeal);

        if (RebetDouble_Button) RebetDouble_Button.onClick.RemoveAllListeners();
        if (RebetDouble_Button) RebetDouble_Button.onClick.AddListener(OnRebetDouble);

        if (Split_Button) Split_Button.onClick.RemoveAllListeners();
        if (Split_Button) Split_Button.onClick.AddListener(OnSplit);

        if (Quit_Button) Quit_Button.onClick.RemoveAllListeners();
        if (Quit_Button) Quit_Button.onClick.AddListener(CallOnExitFunction);

        if (Split_object) Split_object.SetActive(false);
        if (MiddleDouble_object) MiddleDouble_object.SetActive(true);
    }

    private void CallOnExitFunction()
    {
        Application.ExternalCall("window.parent.postMessage", "onExit", "*");
    }

    private void OnBet()
    {
        BJmanager.BetOnButton();
    }

    private void OnSelectBet(int betCounter)
    {
        BJmanager.SelectCoin(betCounter);
    }

    private void OnDeal()
    {
        StartCoroutine(OnStartDeal());
    }

    private void OnClear()
    {
        BJmanager.ClearBetButton();
    }

    private void OnUndo()
    {
        BJmanager.UndoBetButton();
    }

    private void OnInitialDouble()
    {
        BJmanager.DoubleBetButton();
    }

    private void OnHit(bool isDouble = false)
    {
        if (Split_object) Split_object.SetActive(false);
        StartCoroutine(HitDealButton(isDouble));
    }

    private void OnStand()
    {
        StartCoroutine(DealerFinalButton());
    }

    private void OnDoubleAndStand()
    {
        BJmanager.DoubleBetButton();
        OnHit(true);
    }

    private void OnRebet()
    {
        if (MiddleDouble_object) MiddleDouble_object.SetActive(true);
        if (RebetButtons_object) RebetButtons_object.SetActive(false);
        BJmanager.RebetButton();
        if (MainBet_object) MainBet_object.SetActive(true);
        if (ChipBets_Object) ChipBets_Object.SetActive(true);
        if (Player_Object) Player_Object.SetActive(false);
        if (Dealer_Object) Dealer_Object.SetActive(false);
        if (InitialButtons_object) InitialButtons_object.SetActive(true);
    }

    private void OnRebetDeal()
    {
        if (MiddleDouble_object) MiddleDouble_object.SetActive(true);
        if (RebetButtons_object) RebetButtons_object.SetActive(false);
        BJmanager.RebetDealButton();
        OnDeal();
    }

    private void OnRebetDouble()
    {
        if (MiddleDouble_object) MiddleDouble_object.SetActive(true);
        BJmanager.DoubleBetButton();
        if (RebetButtons_object) RebetButtons_object.SetActive(false);
        BJmanager.RebetDealButton();
        OnDeal();
    }

    private void OnSplit()
    {
        if (MiddleButtons_object) MiddleButtons_object.SetActive(false);
        StartCoroutine(OnSplitDeal());
    }

    private IEnumerator OnSplitDeal()
    {
        if (MiddleDouble_object) MiddleDouble_object.SetActive(false);
        if (Split_object) Split_object.SetActive(false);
        BJmanager.SplitButton();
        yield return new WaitUntil(() => BJmanager.isSplit);
        if (MiddleButtons_object) MiddleButtons_object.SetActive(true);
    }

    private IEnumerator OnStartDeal()
    {
        if (MainBet_object) MainBet_object.SetActive(false);
        if (ChipBets_Object) ChipBets_Object.SetActive(false);
        if (Player_Object) Player_Object.SetActive(true);
        if (Dealer_Object) Dealer_Object.SetActive(true);
        if (InitialButtons_object) InitialButtons_object.SetActive(false);
        BJmanager.isFlippin = true;
        BJmanager.OnPlayerDealButton();
        yield return new WaitUntil(() => !BJmanager.isFlippin);
        BJmanager.isFlippin = true;
        BJmanager.OnDealerButton();
        yield return new WaitUntil(() => !BJmanager.isFlippin);
        BJmanager.isFlippin = true;
        BJmanager.OnPlayerDealButton();
        yield return new WaitUntil(() => !BJmanager.isFlippin);
        BJmanager.isFlippin = true;
        BJmanager.OnDealerButtonClosedCard();
        if (MiddleButtons_object) MiddleButtons_object.SetActive(true);
        if(BJmanager.playerData[0] == BJmanager.playerData[1])
        {
            if (Split_object) Split_object.SetActive(true);
        }
    }

    private IEnumerator HitDealButton(bool isDouble)
    {
        if (!BJmanager.isSplit)
        {
            if (MiddleButtons_object) MiddleButtons_object.SetActive(false);
            BJmanager.isFlippin = true;
            BJmanager.OnPlayerDealButton();
            yield return new WaitUntil(() => !BJmanager.isFlippin);
            if (isDouble)
            {
                OnStand();
            }
            else
            {
                if (MiddleButtons_object) MiddleButtons_object.SetActive(true);
            }
        }
        else
        {
            if (MiddleButtons_object) MiddleButtons_object.SetActive(false);
            BJmanager.isFlippin = true;
            BJmanager.OnSplitDealButton();
            yield return new WaitUntil(() => !BJmanager.isFlippin);
            if (MiddleButtons_object) MiddleButtons_object.SetActive(true);
        }
    }

    private IEnumerator DealerFinalButton()
    {
        if (!BJmanager.isSplit)
        {
            if (MiddleButtons_object) MiddleButtons_object.SetActive(false);
            BJmanager.isFlippin = true;
            BJmanager.OnDealerButtonOpen();
            yield return new WaitUntil(() => !BJmanager.isFlippin);
            for (int i = 0; i < BJmanager.dealerData.Count - 2; i++)
            {
                BJmanager.isFlippin = true;
                BJmanager.OnDealerButton();
                yield return new WaitUntil(() => !BJmanager.isFlippin);
            }
            if (RebetButtons_object) RebetButtons_object.SetActive(true);
        }
        else if(BJmanager.isFirstSplit)
        {
            if (MiddleButtons_object) MiddleButtons_object.SetActive(false);
            BJmanager.SplitStandButton();
            yield return new WaitUntil(() => !BJmanager.isFirstSplit);
            if (MiddleButtons_object) MiddleButtons_object.SetActive(true);
        }
        else
        {
            if (MiddleButtons_object) MiddleButtons_object.SetActive(false);
            BJmanager.isFlippin = true;
            BJmanager.OnDealerButtonOpen();
            yield return new WaitUntil(() => !BJmanager.isFlippin);
            for (int i = 0; i < BJmanager.dealerData.Count - 2; i++)
            {
                BJmanager.isFlippin = true;
                BJmanager.OnDealerButton();
                yield return new WaitUntil(() => !BJmanager.isFlippin);
            }
            if (RebetButtons_object) RebetButtons_object.SetActive(true);
        }
    }
}
