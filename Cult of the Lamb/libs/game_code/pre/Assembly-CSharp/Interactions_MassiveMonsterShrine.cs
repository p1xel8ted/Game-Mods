// Decompiled with JetBrains decompiler
// Type: Interactions_MassiveMonsterShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interactions_MassiveMonsterShrine : Interaction
{
  public SpriteRenderer Shrine;
  public GameObject ShrineNormal;
  public GameObject ShrineNight;
  public bool usedShrine;

  private bool CheckAvailibity() => DataManager.GetFollowerSkinUnlocked("MassiveMonster");

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.CheckAvailibity())
    {
      this.usedShrine = true;
      this.Interactable = false;
      this.Label = "";
    }
    else if (TimeManager.IsNight)
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.Interactions.Pray;
    }
    else
    {
      this.Interactable = false;
      this.Label = "";
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if (this.CheckAvailibity())
    {
      this.usedShrine = true;
      this.Interactable = false;
    }
    else
    {
      this.PhaseChanged();
      TimeManager.OnNewPhaseStarted += new System.Action(this.PhaseChanged);
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    TimeManager.OnNewPhaseStarted -= new System.Action(this.PhaseChanged);
  }

  private new void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.PhaseChanged);

  private void PhaseChanged()
  {
    this.ShrineNormal.SetActive(false);
    this.ShrineNight.SetActive(false);
    if (TimeManager.IsNight)
      this.ShrineNight.SetActive(true);
    else
      this.ShrineNormal.SetActive(true);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.PrayRoutine());
  }

  private IEnumerator PrayRoutine()
  {
    Interactions_MassiveMonsterShrine massiveMonsterShrine = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", PlayerFarming.Instance.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 12f);
    PlayerFarming.Instance.CustomAnimation("pray", false);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i < 10; ++i)
    {
      SoulCustomTarget.Create(massiveMonsterShrine.gameObject, PlayerFarming.Instance.transform.position, Color.red, (System.Action) null);
      yield return (object) new WaitForSeconds(0.05f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    massiveMonsterShrine.gameObject.transform.DOShakePosition(1f, new Vector3(0.25f, 0.0f, 0.0f));
    massiveMonsterShrine.Shrine.DOColor(Color.red, 1f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 1f);
    AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win", massiveMonsterShrine.transform.position);
    yield return (object) new WaitForSeconds(1f);
    massiveMonsterShrine.Shrine.DOColor(Color.white, 0.2f);
    FollowerSkinCustomTarget.Create(massiveMonsterShrine.transform.position, PlayerFarming.Instance.transform.position, 1f, "MassiveMonster", new System.Action(massiveMonsterShrine.PickedUp));
    massiveMonsterShrine.ShrineNormal.SetActive(true);
    massiveMonsterShrine.ShrineNight.SetActive(false);
  }

  private void PickedUp()
  {
    DataManager.Instance.PrayedAtMassiveMonsterShrine = true;
    GameManager.GetInstance().OnConversationEnd();
    this.Interactable = false;
  }
}
