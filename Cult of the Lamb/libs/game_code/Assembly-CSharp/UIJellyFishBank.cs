// Decompiled with JetBrains decompiler
// Type: UIJellyFishBank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class UIJellyFishBank : BaseMonoBehaviour
{
  public CanvasGroup Menu;
  public CanvasGroup MenuMoney;
  public CanvasGroup MenuMoneyInvestmentAmount;
  public CanvasGroup MenuConfirm;
  public PauseInventoryItem resourceIndicator;
  public TextMeshProUGUI NewInvestmentTxt;
  public TextMeshProUGUI InvestmentProfitTxt;
  public TextMeshProUGUI ConfirmationText;
  public TextMeshProUGUI InitialInvestmentTxt;
  public Animator animator;
  [SerializeField]
  public AnimationCurve inputCurve;
  public List<GameObject> GraphPoints = new List<GameObject>();
  public GameObject LowPoint;
  public GameObject HighPoint;
  public int InvestmentAdd;
  public int InitialInvestment;
  public JellyFishInvestment localInvestment;
  public System.Action _Callback;
  public float InputDelay;
  public bool DoingSomething;
  public bool inMenu;
  public bool withdrawl;
  public bool confirmWithdrawl;

  public void UpdateLocalInvestment() => this.localInvestment = DataManager.Instance.Investment;

  public void Play(System.Action Callback)
  {
    this._Callback = Callback;
    this.Menu.alpha = 1f;
    this.MenuMoney.alpha = 0.0f;
    this.MenuMoney.interactable = false;
    this.MenuConfirm.interactable = false;
    this.resourceIndicator.GetComponent<CanvasGroup>().alpha = 0.0f;
    this.CheckInvestment();
    this.InputDelay = 0.3f;
  }

  public void Update()
  {
    this.resourceIndicator.Init(InventoryItem.ITEM_TYPE.BLACK_GOLD, Inventory.GetItemQuantity(20));
    if (!InputManager.UI.GetCancelButtonDown() && !InputManager.Gameplay.GetPauseButtonDown() || this.inMenu)
      return;
    System.Action callback = this._Callback;
    if (callback != null)
      callback();
    this.animator.Play("Out");
  }

  public void UpdateInvestmentAmount(int investmentAmount)
  {
    this.InitialInvestmentTxt.text = "<sprite name=icon_blackgold>";
    int num = 4 - investmentAmount.ToString().Length;
    string str = "<color=#6e6d69>0</color>";
    for (int index = 0; index < num; ++index)
      this.InitialInvestmentTxt.text = (this.InitialInvestmentTxt.text += str);
    this.InitialInvestmentTxt.text += investmentAmount.ToString();
  }

  public void SetGraph()
  {
    if (DataManager.Instance.CheckInvestmentExist())
    {
      int index = 0;
      foreach (GameObject graphPoint in this.GraphPoints)
      {
        graphPoint.SetActive(false);
        if (DataManager.Instance.Investment.InvestmentDays.Count > index)
        {
          Debug.Log((object) $"Checking index of investment day: {index.ToString()}Investment Count = {DataManager.Instance.Investment.InvestmentDays.Count.ToString()}");
          if (DataManager.Instance.Investment.InvestmentDays != null && DataManager.Instance.Investment.InvestmentDays[index] != null)
          {
            graphPoint.SetActive(true);
            graphPoint.GetComponent<TextMeshProUGUI>().text = DataManager.Instance.Investment.InvestmentDays[index].InterestRate.ToString();
            graphPoint.transform.position = Vector3.Lerp(new Vector3(graphPoint.transform.position.x, this.LowPoint.gameObject.transform.position.y, graphPoint.transform.position.z), new Vector3(graphPoint.transform.position.x, this.HighPoint.gameObject.transform.position.y, graphPoint.transform.position.z), (float) (((double) Mathf.Abs(DataManager.Instance.Investment.InvestmentDays[index].InterestRate) + 0.30000001192092896) / 0.60000002384185791));
            if ((double) Mathf.Sign(DataManager.Instance.Investment.InvestmentDays[index].InterestRate) == 1.0)
              graphPoint.GetComponent<TextMeshProUGUI>().color = StaticColors.GreenColor;
            else
              graphPoint.GetComponent<TextMeshProUGUI>().color = StaticColors.RedColor;
          }
        }
        ++index;
      }
    }
    else
    {
      foreach (GameObject graphPoint in this.GraphPoints)
        graphPoint.SetActive(false);
    }
  }

  public void Withdrawl()
  {
    this.Menu.alpha = 0.0f;
    this.Menu.interactable = true;
    this.MenuMoney.alpha = 1f;
    this.MenuMoney.interactable = true;
    this.InvestmentAdd = 0;
    this.NewInvestmentTxt.text = this.InvestmentAdd.ToString();
    this.resourceIndicator.GetComponent<CanvasGroup>().alpha = 1f;
    this.resourceIndicator.Init(InventoryItem.ITEM_TYPE.BLACK_GOLD, Inventory.GetItemQuantity(20));
    this.StartCoroutine((IEnumerator) this.WithdrawlRoutine(true));
  }

  public IEnumerator WithdrawlRoutine(bool Withdrawl)
  {
    UIJellyFishBank uiJellyFishBank = this;
    uiJellyFishBank.withdrawl = Withdrawl;
    while (InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    Debug.Log((object) "Withdrawl/Deposit Routine");
    uiJellyFishBank.inMenu = true;
    uiJellyFishBank.DoingSomething = false;
    float maxDelay = 0.25f;
    float minDelay = 1E-05f;
    float holdTimeToReachMin = 2f;
    float progress = 0.0f;
    int movingDirection = 0;
    int amountPerRound = 1;
    while (!InputManager.UI.GetCancelButtonDown() && (!InputManager.Gameplay.GetPauseButtonDown() || uiJellyFishBank.DoingSomething))
    {
      if ((double) InputManager.UI.GetHorizontalAxis() > -0.30000001192092896 && movingDirection == -1 || (double) InputManager.UI.GetHorizontalAxis() < 0.30000001192092896 && movingDirection == 1)
      {
        uiJellyFishBank.InputDelay = 0.0f;
        progress = 0.0f;
        movingDirection = 0;
      }
      amountPerRound = 1 + Mathf.CeilToInt(progress / 2f);
      int q;
      if ((double) (uiJellyFishBank.InputDelay -= Time.unscaledDeltaTime) < 0.0 && !uiJellyFishBank.DoingSomething)
      {
        for (q = 0; q < amountPerRound; ++q)
        {
          if ((double) InputManager.UI.GetHorizontalAxis() < -0.30000001192092896)
          {
            if (uiJellyFishBank.InvestmentAdd > 0)
            {
              --uiJellyFishBank.InvestmentAdd;
              uiJellyFishBank.NewInvestmentTxt.text = uiJellyFishBank.InvestmentAdd.ToString();
              AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", uiJellyFishBank.gameObject);
            }
            else
            {
              AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", uiJellyFishBank.gameObject);
              uiJellyFishBank.NewInvestmentTxt.gameObject.transform.DOPunchPosition(new Vector3(1f, 0.0f, 0.0f), 0.5f);
              yield return (object) new WaitForSeconds(0.5f);
            }
            uiJellyFishBank.InputDelay = Mathf.Lerp(maxDelay, minDelay, uiJellyFishBank.inputCurve.Evaluate(Mathf.Clamp01(progress / holdTimeToReachMin)));
            movingDirection = -1;
          }
          else if ((double) InputManager.UI.GetHorizontalAxis() > 0.30000001192092896)
          {
            if (!Withdrawl)
            {
              if (uiJellyFishBank.InvestmentAdd <= Inventory.GetItemQuantity(20) - 1)
              {
                AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", uiJellyFishBank.gameObject);
                ++uiJellyFishBank.InvestmentAdd;
                uiJellyFishBank.NewInvestmentTxt.text = uiJellyFishBank.InvestmentAdd.ToString();
              }
              else
              {
                AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", uiJellyFishBank.gameObject);
                uiJellyFishBank.NewInvestmentTxt.gameObject.transform.DOPunchPosition(new Vector3(1f, 0.0f, 0.0f), 0.5f);
                uiJellyFishBank.InputDelay = maxDelay;
                yield return (object) new WaitForSeconds(0.5f);
              }
            }
            else if (DataManager.Instance.CheckInvestmentExist())
            {
              if (uiJellyFishBank.InvestmentAdd < DataManager.Instance.Investment.InitialInvestment)
              {
                AudioManager.Instance.PlayOneShot("event:/ui/arrow_change_selection", uiJellyFishBank.gameObject);
                ++uiJellyFishBank.InvestmentAdd;
                uiJellyFishBank.NewInvestmentTxt.text = uiJellyFishBank.InvestmentAdd.ToString();
              }
              else
              {
                AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", uiJellyFishBank.gameObject);
                uiJellyFishBank.NewInvestmentTxt.gameObject.transform.DOPunchPosition(new Vector3(1f, 0.0f, 0.0f), 0.5f);
                uiJellyFishBank.InputDelay = maxDelay;
                yield return (object) new WaitForSeconds(0.5f);
              }
            }
            else
            {
              AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", uiJellyFishBank.gameObject);
              uiJellyFishBank.NewInvestmentTxt.gameObject.transform.DOPunchPosition(new Vector3(1f, 0.0f, 0.0f), 0.5f);
              uiJellyFishBank.InputDelay = maxDelay;
              yield return (object) new WaitForSeconds(0.5f);
            }
            uiJellyFishBank.InputDelay = Mathf.Lerp(maxDelay, minDelay, uiJellyFishBank.inputCurve.Evaluate(Mathf.Clamp01(progress / holdTimeToReachMin)));
            movingDirection = 1;
          }
        }
      }
      if ((double) InputManager.UI.GetHorizontalAxis() > 0.30000001192092896 || (double) InputManager.UI.GetHorizontalAxis() < -0.30000001192092896)
      {
        progress += Time.deltaTime;
      }
      else
      {
        uiJellyFishBank.InputDelay = 0.0f;
        progress = 0.0f;
        movingDirection = 0;
      }
      if ((double) uiJellyFishBank.InputDelay == 0.0 && InputManager.UI.GetAcceptButtonDown())
      {
        if (uiJellyFishBank.InvestmentAdd > 0)
        {
          Debug.Log((object) "Do Withdrawl / Deposit");
          uiJellyFishBank.DoingSomething = true;
          AudioManager.Instance.PlayOneShot("event:/ui/confirm_selection", uiJellyFishBank.gameObject);
          uiJellyFishBank.MenuConfirm.alpha = 1f;
          uiJellyFishBank.MenuMoney.alpha = 0.0f;
          uiJellyFishBank.MenuConfirm.interactable = true;
          uiJellyFishBank.MenuMoney.interactable = false;
          uiJellyFishBank.SetCofirmationText(Withdrawl);
          while (!uiJellyFishBank.confirmWithdrawl)
            yield return (object) null;
          uiJellyFishBank.MenuMoneyInvestmentAmount.alpha = 0.0f;
          float increment;
          if (!Withdrawl)
          {
            uiJellyFishBank.SetInvestment(uiJellyFishBank.InvestmentAdd);
            increment = 2f / (float) uiJellyFishBank.InvestmentAdd;
            amountPerRound = Mathf.Clamp(uiJellyFishBank.InvestmentAdd / 250, 1, int.MaxValue);
            q = 0;
            while (q < uiJellyFishBank.InvestmentAdd && q < uiJellyFishBank.InvestmentAdd)
            {
              for (int index = 0; index < amountPerRound && q < uiJellyFishBank.InvestmentAdd; ++index)
              {
                Inventory.ChangeItemQuantity(20, -1);
                uiJellyFishBank.NewInvestmentTxt.text = uiJellyFishBank.InvestmentAdd.ToString();
                ++uiJellyFishBank.InitialInvestment;
                uiJellyFishBank.UpdateInvestmentAmount(uiJellyFishBank.InitialInvestment);
                ++q;
              }
              yield return (object) new WaitForSeconds(increment);
            }
          }
          else
          {
            uiJellyFishBank.SetInvestment(uiJellyFishBank.InvestmentAdd * -1);
            increment = 2f / (float) uiJellyFishBank.InvestmentAdd;
            amountPerRound = Mathf.Clamp(uiJellyFishBank.InvestmentAdd / 250, 1, int.MaxValue);
            q = 0;
            while (q < uiJellyFishBank.InvestmentAdd && q < uiJellyFishBank.InvestmentAdd)
            {
              for (int index = 0; index < amountPerRound && q < uiJellyFishBank.InvestmentAdd; ++index)
              {
                Inventory.AddItem(20, 1);
                uiJellyFishBank.NewInvestmentTxt.text = uiJellyFishBank.InvestmentAdd.ToString();
                --uiJellyFishBank.InitialInvestment;
                uiJellyFishBank.UpdateInvestmentAmount(uiJellyFishBank.InitialInvestment);
                ++q;
              }
              yield return (object) new WaitForSeconds(increment);
            }
          }
          yield return (object) new WaitForSeconds(0.3f);
          uiJellyFishBank.confirmWithdrawl = false;
          uiJellyFishBank.MenuMoneyInvestmentAmount.alpha = 1f;
          uiJellyFishBank.StartCoroutine((IEnumerator) uiJellyFishBank.ExitMenu());
          yield break;
        }
        AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", uiJellyFishBank.gameObject);
        uiJellyFishBank.NewInvestmentTxt.gameObject.transform.DOPunchPosition(new Vector3(1f, 0.0f, 0.0f), 0.5f);
        yield return (object) null;
      }
      yield return (object) null;
    }
    uiJellyFishBank.StartCoroutine((IEnumerator) uiJellyFishBank.ExitMenu());
  }

  public void Deposit()
  {
    this.Menu.alpha = 0.0f;
    this.MenuMoney.alpha = 1f;
    this.Menu.interactable = false;
    this.MenuMoney.interactable = true;
    this.InvestmentAdd = 0;
    this.NewInvestmentTxt.text = this.InvestmentAdd.ToString();
    this.resourceIndicator.GetComponent<CanvasGroup>().alpha = 1f;
    this.resourceIndicator.Init(InventoryItem.ITEM_TYPE.BLACK_GOLD, Inventory.GetItemQuantity(20));
    this.StartCoroutine((IEnumerator) this.WithdrawlRoutine(false));
  }

  public IEnumerator ExitMenu()
  {
    this.resourceIndicator.GetComponent<CanvasGroup>().alpha = 0.0f;
    while (InputManager.UI.GetCancelButtonDown())
      yield return (object) null;
    this.inMenu = false;
    this.MenuMoney.alpha = 0.0f;
    this.MenuMoney.interactable = false;
    this.MenuConfirm.interactable = false;
    this.Menu.interactable = true;
    this.Menu.alpha = 1f;
    this.SetGraph();
  }

  public void OnEnable()
  {
  }

  public void GetInvestmentTotal()
  {
  }

  public void ConfirmDepositWithdrawl()
  {
    this.MenuConfirm.alpha = 0.0f;
    this.MenuMoney.alpha = 1f;
    this.MenuMoney.interactable = true;
    this.MenuConfirm.interactable = false;
    this.confirmWithdrawl = true;
  }

  public void DeclineDepositWithdrawl()
  {
    this.MenuConfirm.alpha = 0.0f;
    this.MenuMoney.alpha = 1f;
    this.MenuMoney.interactable = true;
    this.MenuConfirm.interactable = false;
    this.StopCoroutine((IEnumerator) this.WithdrawlRoutine(false));
    this.StartCoroutine((IEnumerator) this.WithdrawlRoutine(this.withdrawl));
  }

  public void SetCofirmationText(bool _Withdrawl)
  {
    if (_Withdrawl)
      this.ConfirmationText.text = "Withdrawl: " + this.InvestmentAdd.ToString();
    else
      this.ConfirmationText.text = "Deposit: " + this.InvestmentAdd.ToString();
  }

  public void CheckInvestment()
  {
    if (DataManager.Instance.CheckInvestmentExist())
    {
      int num = TimeManager.CurrentDay - DataManager.Instance.Investment.LastDayCheckedInvestment;
      if (num > 0)
      {
        for (int index = 0; index < num; ++index)
        {
          JellyFishInvestmentDay fishInvestmentDay = new JellyFishInvestmentDay();
          Vector2 vector2 = this.RunInvestment(DataManager.Instance.Investment.InvestmentDays[DataManager.Instance.Investment.InvestmentDays.Count - 1].InvestmentAmount);
          fishInvestmentDay.Day = (DataManager.Instance.Investment.LastDayCheckedInvestment += index);
          fishInvestmentDay.InvestmentAmount = (int) vector2.x;
          fishInvestmentDay.InterestRate = vector2.y;
          DataManager.Instance.Investment.InitialInvestment += fishInvestmentDay.InvestmentAmount;
          DataManager.Instance.Investment.InvestmentDays.Add(fishInvestmentDay);
          Debug.Log((object) $"Run Investments, day : {fishInvestmentDay.Day.ToString()} Interest Earned: {fishInvestmentDay.InvestmentAmount.ToString()}");
        }
        DataManager.Instance.Investment.LastDayCheckedInvestment = TimeManager.CurrentDay;
        this.SetGraph();
      }
      else
      {
        this.SetGraph();
        Debug.Log((object) $"Don't run investments, last invest: {DataManager.Instance.Investment.LastDayCheckedInvestment.ToString()}Current Day: {TimeManager.CurrentDay.ToString()}");
      }
      this.InitialInvestment = DataManager.Instance.Investment.InitialInvestment;
      this.UpdateInvestmentAmount(this.InitialInvestment);
      float f = (float) (DataManager.Instance.Investment.InitialInvestment - DataManager.Instance.Investment.ActualInvestedAmount);
      this.InvestmentProfitTxt.text = "Profit: " + f.ToString();
      if ((double) Mathf.Sign(f) == 1.0)
        this.InvestmentProfitTxt.color = StaticColors.GreenColor;
      else
        this.InvestmentProfitTxt.color = StaticColors.RedColor;
    }
    else
    {
      this.SetGraph();
      this.UpdateInvestmentAmount(0);
      this.InvestmentProfitTxt.text = "";
      Debug.Log((object) "No Investments");
    }
  }

  public Vector2 RunInvestment(int InvestmentAmount)
  {
    Debug.Log((object) ("Calculating Investment of " + InvestmentAmount.ToString()));
    int num = UnityEngine.Random.Range(0, 100);
    float y = num > 50 ? (num > 80 /*0x50*/ ? (num > 88 ? (num > 90 ? (num > 98 ? 0.3f : 0.2f) : -0.3f) : -0.2f) : -0.1f) : 0.1f;
    float x = (float) InvestmentAmount * y;
    if ((int) x == 0)
    {
      if ((double) y >= 0.10000000149011612)
        x = 1f;
      else if ((double) y <= 0.10000000149011612)
        x = -1f;
    }
    Debug.Log((object) $"Investment Ran: {x.ToString()}Return of: {((float) InvestmentAmount * y).ToString()}");
    return new Vector2((float) (int) x, y);
  }

  public bool checkInvestmentDay()
  {
    foreach (JellyFishInvestmentDay investmentDay in DataManager.Instance.Investment.InvestmentDays)
    {
      if (investmentDay.Day == TimeManager.CurrentDay)
        return true;
    }
    return false;
  }

  public int returnInvestmentDay()
  {
    int num = 0;
    foreach (JellyFishInvestmentDay investmentDay in DataManager.Instance.Investment.InvestmentDays)
    {
      if (investmentDay.Day == TimeManager.CurrentDay)
        return num;
      ++num;
    }
    return -1;
  }

  public void SetInvestment(int InvestmentAdd)
  {
    JellyFishInvestmentDay fishInvestmentDay = new JellyFishInvestmentDay();
    this.localInvestment = new JellyFishInvestment();
    Debug.Log((object) "Set Investment");
    if (DataManager.Instance.CheckInvestmentExist())
    {
      Debug.Log((object) "Investment Exists");
      DataManager.Instance.Investment.ActualInvestedAmount += InvestmentAdd;
      DataManager.Instance.Investment.InitialInvestment += InvestmentAdd;
      this.localInvestment = DataManager.Instance.Investment;
      if (!this.checkInvestmentDay())
      {
        Debug.Log((object) $"Create Investment Day, Day: {TimeManager.CurrentDay.ToString()} Total Investment On Day: {DataManager.Instance.Investment.InitialInvestment.ToString()}");
        fishInvestmentDay.Day = TimeManager.CurrentDay;
        fishInvestmentDay.InvestmentAmount = (InvestmentAdd += DataManager.Instance.Investment.InitialInvestment);
        this.localInvestment.InvestmentDays.Add(fishInvestmentDay);
      }
      else
      {
        Debug.Log((object) "Add to existing investment");
        DataManager.Instance.Investment.InvestmentDays[this.returnInvestmentDay()].InvestmentAmount = (InvestmentAdd += DataManager.Instance.Investment.InitialInvestment);
      }
    }
    else
    {
      Debug.Log((object) "Set Investment, create new one");
      this.localInvestment.InitialInvestment += InvestmentAdd;
      this.localInvestment.InvestmentDay = TimeManager.CurrentDay;
      fishInvestmentDay.Day = TimeManager.CurrentDay;
      fishInvestmentDay.InvestmentAmount = InvestmentAdd;
      this.localInvestment.LastDayCheckedInvestment = TimeManager.CurrentDay;
      this.localInvestment.ActualInvestedAmount = InvestmentAdd;
      this.localInvestment.InvestmentDays.Add(fishInvestmentDay);
      DataManager.Instance.CreateInvestment(this.localInvestment);
    }
    this.UpdateInvestments();
  }

  public void UpdateInvestments()
  {
    if (DataManager.Instance.CheckInvestmentExist())
      Debug.Log((object) "Investment does exist");
    else
      Debug.Log((object) "Investment doesn't exist");
  }
}
