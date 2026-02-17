// Decompiled with JetBrains decompiler
// Type: Interaction_ConsumeFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_ConsumeFollower : Interaction
{
  public SkeletonAnimation followerSpine;
  public FollowerInfo followerInfo;
  public FollowerOutfit outfit;
  public System.Action<int, int, int, int, int, int> Callback;
  public Follower follower;
  public string sString;
  public int HP;
  public int SPIRITHP;
  public int BlueHeart;
  public int BlackHeart;
  public int IceHeart;
  public int FireHeart;
  public float TotalHealth;
  public string HeartDispayString;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = $"{ScriptLocalization.Interactions.SacrificeFollower}: <color=#FFD201>{this.followerInfo.Name}{(this.followerInfo.MarriedToLeader ? " <sprite name=\"icon_Married\">" : "")}</color>";
  }

  public void Play(FollowerInfo followerInfo, System.Action<int, int, int, int, int, int> Callback)
  {
    this.ActivateDistance = 2f;
    this.followerInfo = followerInfo;
    this.outfit = new FollowerOutfit(followerInfo);
    this.Callback = Callback;
    this.outfit.SetOutfit(this.followerSpine, false);
    this.follower = this.GetComponent<Follower>();
    this.SetHearts();
  }

  public void SetHearts()
  {
    this.TotalHealth = this.playerFarming.health.PLAYER_TOTAL_HEALTH + this.playerFarming.health.PLAYER_SPIRIT_TOTAL_HEARTS;
    if (this.followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
      this.FireHeart = 4;
    switch (this.followerInfo.CursedState)
    {
      case Thought.Dissenter:
        this.BlackHeart = 2;
        break;
      case Thought.OldAge:
        this.BlueHeart = 2;
        break;
      case Thought.Freezing:
        this.IceHeart = 2;
        break;
      default:
        int num = this.followerInfo.XPLevel * 2;
        this.HP = 0;
        this.SPIRITHP = 0;
        while (num > 0)
        {
          if ((double) this.HP < (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH)
          {
            ++this.HP;
            --num;
            if ((double) this.HP == (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH && this.HP % 2 == 1)
              --num;
          }
          else if ((double) this.SPIRITHP < (double) this.playerFarming.health.PLAYER_SPIRIT_TOTAL_HEARTS)
          {
            ++this.SPIRITHP;
            --num;
          }
          else
            break;
        }
        break;
    }
    if (DataManager.Instance.PlayerFleece == 1000)
      this.BlackHeart += 4;
    if (DataManager.Instance.PlayerFleece != 5)
    {
      this.HP = (int) Mathf.Clamp((float) this.HP, 1f, this.TotalHealth);
    }
    else
    {
      this.BlueHeart += this.followerInfo.XPLevel * 2;
      this.HP = this.SPIRITHP = 0;
    }
    if (TrinketManager.HasTrinket(TarotCards.Card.MutatedResurrectFullHealth, this.playerFarming) && this.followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      this.HP = (int) this.playerFarming.health.PLAYER_TOTAL_HEALTH;
      this.SPIRITHP = (int) this.playerFarming.health.PLAYER_SPIRIT_TOTAL_HEARTS;
    }
    this.SetHeartDisplayString();
  }

  public void SetHeartDisplayString()
  {
    this.HeartDispayString = "";
    switch (this.followerInfo.CursedState)
    {
      case Thought.Dissenter:
        this.HeartDispayString += FollowerThoughts.GetLocalisedName(this.followerInfo.CursedState, this.followerInfo.ID);
        break;
      case Thought.OldAge:
        this.HeartDispayString += FollowerThoughts.GetLocalisedName(this.followerInfo.CursedState, this.followerInfo.ID);
        break;
      default:
        this.HeartDispayString = $"{this.HeartDispayString}{ScriptLocalization.Interactions.Level} {this.followerInfo.XPLevel.ToNumeral()}";
        break;
    }
    if (this.followerInfo.MarriedToLeader)
      this.BlueHeart += 6;
    this.HeartDispayString += ": ";
    int num1 = -1;
    int num2 = (int) this.playerFarming.health.PLAYER_TOTAL_HEALTH + (int) this.playerFarming.health.PLAYER_SPIRIT_TOTAL_HEARTS;
    int hp = this.HP;
    int spirithp = this.SPIRITHP;
    int num3;
    for (; (num3 = num1 + 1) < num2; num1 = num3 + 1)
    {
      if (hp >= 2)
      {
        this.HeartDispayString += "<sprite name=\"icon_UIHeart\">";
        hp -= 2;
      }
      else if (hp == 1)
      {
        this.HeartDispayString = (double) num3 != (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH - 1.0 || (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH % 2.0 == 0.0 ? this.HeartDispayString + "<sprite name=\"icon_UIHeartHalf\">" : this.HeartDispayString + "<sprite name=\"icon_UIHeartHalfEmpty\">";
        hp -= 2;
      }
      else if (spirithp >= 2)
      {
        this.HeartDispayString += "<sprite name=\"icon_UIHeartSpirit\">";
        spirithp -= 2;
      }
      else if (spirithp == 1)
      {
        this.HeartDispayString += "<sprite name=\"icon_UIHeartHalfSpirit\">";
        spirithp -= 2;
      }
      else
        this.HeartDispayString = (double) num3 < (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH ? ((double) num3 != (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH - 1.0 || (double) this.playerFarming.health.PLAYER_TOTAL_HEALTH % 2.0 == 0.0 ? this.HeartDispayString + "<sprite name=\"icon_UIHeartEmpty\">" : this.HeartDispayString + "<sprite name=\"icon_UIHeartHalfFull\">") : this.HeartDispayString + "<sprite name=\"icon_UIHeartSpiritEmpty\">";
    }
    if (this.BlueHeart > 0)
    {
      int num4 = -1;
      while (++num4 < this.BlueHeart / 2)
        this.HeartDispayString += "<sprite name=\"icon_UIHeartBlue\">";
    }
    int num5 = -1;
    while (++num5 < this.BlackHeart / 2)
      this.HeartDispayString += "<sprite name=\"icon_UIHeartBlack\">";
    if (this.IceHeart > 0)
    {
      int num6 = -1;
      while (++num6 < this.IceHeart / 2)
        this.HeartDispayString += "<sprite name=\"icon_UIHeartIce\">";
    }
    if (this.FireHeart <= 0)
      return;
    int num7 = -1;
    while (++num7 < this.FireHeart / 2)
      this.HeartDispayString += "<sprite name=\"icon_UIHeartFire\">";
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    playerFarming.indicator.ShowTopInfo($"<sprite name=\"img_SwirleyLeft\"> {this.sString} <sprite name=\"img_SwirleyRight\">");
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    FollowerManager.ConsumeFollower(this.followerInfo.ID);
    state.CURRENT_STATE = StateMachine.State.InActive;
    System.Action<int, int, int, int, int, int> callback = this.Callback;
    if (callback != null)
      callback(this.HP, this.SPIRITHP, this.BlueHeart, this.BlackHeart, this.IceHeart, this.FireHeart);
    this.playerFarming.indicator.HideTopInfo();
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
    double num = (double) this.follower.SetBodyAnimation("worship-eyesopen", true);
    this.StartCoroutine((IEnumerator) this.ShowIcons());
  }

  public IEnumerator ShowIcons()
  {
    yield return (object) new WaitForEndOfFrame();
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    double num = (double) this.follower.SetBodyAnimation("pray", true);
  }

  public override void GetLabel() => this.Label = this.HeartDispayString;
}
