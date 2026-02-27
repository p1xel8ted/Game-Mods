// Decompiled with JetBrains decompiler
// Type: Interaction_LighthouseBurner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_LighthouseBurner : Interaction
{
  public Structure structure;
  [SerializeField]
  private UnityEvent CallBack;
  public HubShoreManager hubShoreManager;
  private int NumCooked;
  private string sAddFuel;
  private string sAddIngredients;
  private string sRecipe;
  private string sCancel;
  private int Fuel;
  private int RequiredFuel = 15;
  public bool LitLighthouse;
  private string _Label;
  public Interaction_LighthouseBurner.State CurrentState;
  public GameObject BurnerOn;
  public GameObject BurnerOff;
  public GameObject Light;
  public GameObject litLighthouseConvo;
  public DOTweenAnimation burnerAnimation;
  public GameObject Demons;
  public SkeletonAnimation[] worshippers;
  public SkeletonAnimation leader;
  private bool Activating;
  private bool TurnedOn;

  public override void OnEnableInteraction()
  {
    this.HasSecondaryInteraction = true;
    base.OnEnableInteraction();
    this.UpdateLocalisation();
    this.SetImages();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sAddFuel = ScriptLocalization.Interactions.AddFuel;
    this.sAddIngredients = ScriptLocalization.Interactions.AddIngredients;
  }

  private string GetAffordColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) > 0 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    switch (this.CurrentState)
    {
      case Interaction_LighthouseBurner.State.Off:
        if (DataManager.Instance.LighthouseFuel < this.RequiredFuel)
        {
          this.Label = string.Join(" ", this.sAddFuel, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.LOG, this.RequiredFuel));
          break;
        }
        this.Interactable = false;
        this.Label = "";
        break;
      case Interaction_LighthouseBurner.State.AddIngredients:
        this.Label = this.sAddIngredients;
        break;
      case Interaction_LighthouseBurner.State.Disabled:
        this.Label = "";
        break;
    }
  }

  public void SetImages()
  {
    this.BurnerOn.SetActive(false);
    this.BurnerOff.SetActive(false);
    this.Light.SetActive(false);
    this.GetLabel();
    if (DataManager.Instance.Lighthouse_Lit)
    {
      this.Light.SetActive(true);
      this.BurnerOn.SetActive(true);
      this.Interactable = false;
    }
    else
    {
      this.BurnerOff.SetActive(true);
      this.Interactable = true;
    }
  }

  private void TurnOn()
  {
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIX_LIGHTHOUSE"));
    AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.gameObject);
    for (int index = 0; index < this.worshippers.Length; ++index)
      this.worshippers[index].AnimationState.SetAnimation(0, "prayer_idle", true);
    this.leader.AnimationState.SetAnimation(0, "animation", true);
    DataManager.Instance.Lighthouse_Lit = true;
    this.Demons.SetActive(false);
    this.burnerAnimation.DOPlay();
    this.litLighthouseConvo.SetActive(true);
    this.TurnedOn = true;
    this.hubShoreManager.LighthouseLit = true;
    this.Activating = false;
    this.Interactable = false;
    this.CurrentState = Interaction_LighthouseBurner.State.Full;
    this.SetImages();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.CurrentState == Interaction_LighthouseBurner.State.Full || this.Activating)
      return;
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) >= this.RequiredFuel)
    {
      this.StartCoroutine((IEnumerator) this.AddFuel());
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
      MonoSingleton<Indicator>.Instance.PlayShake();
    }
  }

  public void ShakeBurner()
  {
    this.gameObject.transform.DOShakeScale(1f, new Vector3(1.5f, 1.5f, 1.5f));
    AudioManager.Instance.PlayOneShot("event:/locations/light_house/fireplace_shake", this.gameObject);
  }

  private IEnumerator AddFuel()
  {
    Interaction_LighthouseBurner lighthouseBurner = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(lighthouseBurner.gameObject, 7f);
    lighthouseBurner.Activating = true;
    int count = 0;
    InventoryItem.ITEM_TYPE ItemToDeposit = InventoryItem.ITEM_TYPE.LOG;
    MMVibrate.RumbleContinuous(0.25f, 0.75f);
    while (count < lighthouseBurner.RequiredFuel)
    {
      Inventory.GetItemByType((int) ItemToDeposit);
      AudioManager.Instance.PlayOneShot("event:/cooking/add_food_ingredient", PlayerFarming.Instance.gameObject);
      ResourceCustomTarget.Create(lighthouseBurner.gameObject, PlayerFarming.Instance.transform.position, ItemToDeposit, (System.Action) null);
      Inventory.ChangeItemQuantity((int) ItemToDeposit, -1);
      ++count;
      yield return (object) new WaitForSeconds(0.1f);
    }
    CameraManager.shakeCamera(10f);
    DataManager.Instance.LighthouseFuel = lighthouseBurner.RequiredFuel;
    DataManager.Instance.Lighthouse_Lit = true;
    lighthouseBurner.TurnOn();
    yield return (object) new WaitForSeconds(0.1f);
    MMVibrate.StopRumble();
    lighthouseBurner.Activating = false;
    lighthouseBurner.BurnerOff.SetActive(false);
    lighthouseBurner.BurnerOn.SetActive(true);
    lighthouseBurner.Light.SetActive(true);
    lighthouseBurner.enabled = false;
    GameManager.GetInstance().OnConversationEnd();
  }

  public enum State
  {
    Off,
    AddIngredients,
    Disabled,
    Full,
  }
}
