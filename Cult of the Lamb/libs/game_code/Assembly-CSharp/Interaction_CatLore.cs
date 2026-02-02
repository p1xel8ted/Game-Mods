// Decompiled with JetBrains decompiler
// Type: Interaction_CatLore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using Unify;
using UnityEngine;

#nullable disable
public class Interaction_CatLore : MonoBehaviour
{
  [SerializeField]
  public Interaction_SimpleConversation loreConvo;
  [SerializeField]
  public Interaction_SimpleConversation returnFirstChildConvo;
  [SerializeField]
  public Interaction_SimpleConversation returnFirstChildConvoB;
  [SerializeField]
  public Interaction_SimpleConversation returnSecondChildConvo;
  [SerializeField]
  public Interaction_SimpleConversation returnSecondChildConvoB;
  [SerializeField]
  public Interaction_SimpleConversation returnedAfterChildren;
  [SerializeField]
  public GameObject bark;
  [SerializeField]
  public SkeletonAnimation catSpine;
  [SerializeField]
  public SkeletonAnimation baalSpine;
  [SerializeField]
  public SkeletonAnimation aymSpine;
  [SerializeField]
  public GameObject[] podiums;

  public void Start()
  {
    if (DataManager.Instance.ForneusLore || !DataManager.Instance.BeatenDungeon2)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.loreConvo.gameObject);
    if (DungeonSandboxManager.Active)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.returnFirstChildConvo.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.returnFirstChildConvoB.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.returnSecondChildConvo.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.returnSecondChildConvoB.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.returnedAfterChildren.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.baalSpine.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.aymSpine.gameObject);
    }
    else
    {
      this.returnedAfterChildren.gameObject.SetActive(false);
      if (DataManager.Instance.HasReturnedAym && DataManager.Instance.HasReturnedBaal)
      {
        if (!DataManager.Instance.HasReturnedBoth)
        {
          DataManager.Instance.HasReturnedBoth = true;
          this.returnedAfterChildren.gameObject.SetActive(true);
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) this.baalSpine.gameObject);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.aymSpine.gameObject);
      }
      bool flag1 = DataManager.Instance.Followers_Demons_Types.Contains(6);
      bool flag2 = DataManager.Instance.Followers_Demons_Types.Contains(7);
      if (DataManager.Instance.HasReturnedBaal || !flag1 || DataManager.Instance.HasReturnedAym || !flag2)
      {
        if (this.GetChildrenReturned() >= 1)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnFirstChildConvo.gameObject);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnFirstChildConvoB.gameObject);
          this.returnFirstChildConvo = (Interaction_SimpleConversation) null;
        }
        if (this.GetChildrenReturned() >= 2)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnSecondChildConvo.gameObject);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnSecondChildConvoB.gameObject);
          this.returnSecondChildConvo = (Interaction_SimpleConversation) null;
        }
      }
      if (!flag1 && !flag2)
      {
        if ((UnityEngine.Object) this.returnFirstChildConvo != (UnityEngine.Object) null)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnFirstChildConvo.gameObject);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnFirstChildConvoB.gameObject);
          this.returnFirstChildConvo = (Interaction_SimpleConversation) null;
        }
        if ((UnityEngine.Object) this.returnSecondChildConvo != (UnityEngine.Object) null)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnSecondChildConvo.gameObject);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.returnSecondChildConvoB.gameObject);
          this.returnSecondChildConvo = (Interaction_SimpleConversation) null;
        }
      }
      this.baalSpine.gameObject.SetActive(DataManager.Instance.HasReturnedBaal);
      this.baalSpine.AnimationState.AddAnimation(0, "Forneus/idle-baal", true, 0.0f);
      this.aymSpine.gameObject.SetActive(DataManager.Instance.HasReturnedAym);
      this.aymSpine.AnimationState.AddAnimation(0, "Forneus/idle-aym", true, 0.0f);
      foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData("Boss Baal").SlotAndColours[0].SlotAndColours)
      {
        Slot slot = this.baalSpine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData("Boss Aym").SlotAndColours[0].SlotAndColours)
      {
        Slot slot = this.aymSpine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      if ((UnityEngine.Object) this.returnFirstChildConvo != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.returnSecondChildConvo)
      {
        this.returnSecondChildConvo.gameObject.SetActive(false);
        this.returnSecondChildConvoB.gameObject.SetActive(false);
      }
      if ((UnityEngine.Object) this.returnFirstChildConvo != (UnityEngine.Object) null || (UnityEngine.Object) this.returnSecondChildConvo != (UnityEngine.Object) null)
      {
        foreach (GameObject podium in this.podiums)
          podium.gameObject.SetActive(false);
      }
      foreach (GameObject demon in Demon_Arrows.Demons)
      {
        Demon_Spirit component = demon.GetComponent<Demon_Spirit>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (component.FollowerInfo.ID == 99994 || component.FollowerInfo.ID == 99995))
          component.ForceMove = true;
      }
    }
  }

  public int GetChildrenReturned()
  {
    int childrenReturned = 0;
    if (DataManager.Instance.HasReturnedAym)
      ++childrenReturned;
    if (DataManager.Instance.HasReturnedBaal)
      ++childrenReturned;
    return childrenReturned;
  }

  public void GiveChild()
  {
    this.bark.gameObject.SetActive(false);
    if (DataManager.Instance.Followers_Demons_Types.Contains(6) && !DataManager.Instance.HasReturnedBaal)
    {
      DataManager.Instance.HasReturnedBaal = true;
      this.StartCoroutine((IEnumerator) this.GiveDemon(true));
    }
    else
    {
      if (!DataManager.Instance.Followers_Demons_Types.Contains(7) || DataManager.Instance.HasReturnedAym)
        return;
      DataManager.Instance.HasReturnedAym = true;
      this.StartCoroutine((IEnumerator) this.GiveDemon(false));
    }
  }

  public IEnumerator GiveDemon(bool isBaal)
  {
    yield return (object) new WaitForEndOfFrame();
    int num1 = isBaal ? 6 : 7;
    int num2 = isBaal ? 99994 : 99995;
    Demon_Spirit demon = (Demon_Spirit) null;
    foreach (GameObject demon1 in Demon_Arrows.Demons)
    {
      Demon_Spirit component = demon1.GetComponent<Demon_Spirit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.FollowerInfo.ID == num2)
      {
        demon = component;
        break;
      }
    }
    int index = 0;
    foreach (int followersDemonsType in DataManager.Instance.Followers_Demons_Types)
    {
      if (followersDemonsType == num1)
      {
        int followersDemonsId = DataManager.Instance.Followers_Demons_IDs[index];
        DataManager.Instance.Followers_Demons_Types.RemoveAt(index);
        DataManager.Instance.Followers_Demons_IDs.RemoveAt(index);
        FollowerManager.RemoveFollowerBrain(followersDemonsId);
        break;
      }
      ++index;
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.catSpine.gameObject);
    demon.enabled = false;
    demon.spine.transform.DOMove(isBaal ? this.baalSpine.transform.position : this.aymSpine.transform.position, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    AudioManager.Instance.PlayOneShot("event:/Stings/refuse_kneel_sting");
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/door/goop_door_unlock");
    demon.gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(isBaal ? this.baalSpine.transform.position : this.aymSpine.transform.position);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    if (isBaal)
    {
      this.baalSpine.gameObject.SetActive(true);
      yield return (object) new WaitForEndOfFrame();
      this.baalSpine.AnimationState.SetAnimation(0, "Forneus/greet-baal", false);
      this.baalSpine.AnimationState.AddAnimation(0, "Forneus/idle-baal", true, 0.0f);
    }
    else
    {
      this.aymSpine.gameObject.SetActive(true);
      yield return (object) new WaitForEndOfFrame();
      this.aymSpine.AnimationState.SetAnimation(0, "Forneus/greet-aym", false);
      this.aymSpine.AnimationState.AddAnimation(0, "Forneus/idle-aym", true, 0.0f);
    }
    this.catSpine.AnimationState.SetAnimation(0, "greet", false);
    this.catSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    GameManager.GetInstance().OnConversationNext(isBaal ? this.baalSpine.gameObject : this.aymSpine.gameObject, 5f);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/shop_cat_forneus/buy_forneus");
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/shop_cat_forneus/buy_forneus");
    yield return (object) new WaitForSeconds(2f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/shop_cat_forneus/buy_forneus");
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 7f);
    bool waiting = true;
    RelicType relicType = isBaal ? RelicType.HeartConversion_Blessed : RelicType.HeartConversion_Dammed;
    RelicCustomTarget.Create(this.catSpine.transform.position, PlayerFarming.Instance.transform.position, 1f, relicType, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    if (this.GetChildrenReturned() < 2 && (DataManager.Instance.Followers_Demons_Types.Contains(6) && !DataManager.Instance.HasReturnedBaal || DataManager.Instance.Followers_Demons_Types.Contains(7) && !DataManager.Instance.HasReturnedAym))
    {
      this.returnSecondChildConvo.gameObject.SetActive(true);
      this.returnSecondChildConvoB.gameObject.SetActive(true);
    }
    else
    {
      foreach (GameObject podium in this.podiums)
      {
        podium.gameObject.SetActive(true);
        podium.transform.localPosition = new Vector3(podium.transform.localPosition.x, podium.transform.localPosition.y, 2f);
        podium.transform.DOLocalMoveZ(0.0f, 1f);
      }
    }
    if (DataManager.Instance.HasReturnedAym && DataManager.Instance.HasReturnedBaal)
    {
      AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("RETURN_BAAL_AYM"));
      Debug.Log((object) "ACHIEVEMENT GOT : RETURN_BAAL_AYM");
    }
  }
}
