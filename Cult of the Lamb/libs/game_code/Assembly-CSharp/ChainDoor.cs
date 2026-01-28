// Decompiled with JetBrains decompiler
// Type: ChainDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class ChainDoor : BaseMonoBehaviour
{
  public static ChainDoor Instance;
  public SkeletonAnimation Spine;
  public float Zoom;
  public float ZoomToAdd;

  public void OnEnable() => ChainDoor.Instance = this;

  public void Start()
  {
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location) && !DungeonSandboxManager.Active && !DataManager.Instance.SurvivalModeActive)
      return;
    this.ShowOpenDoor();
  }

  public void ShowOpenDoor() => this.Spine.AnimationState.SetAnimation(0, "open", true);

  public void Play(System.Action Callback)
  {
    this.StartCoroutine((IEnumerator) this.PlayRoutine(Callback));
  }

  public IEnumerator PlayRoutine(System.Action Callback)
  {
    ChainDoor chainDoor = this;
    chainDoor.Zoom = 5f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(chainDoor.gameObject, chainDoor.Zoom += 3f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/door/chain_door_sequence");
    chainDoor.Spine.AnimationState.SetAnimation(0, "activate", false);
    chainDoor.Spine.AnimationState.AddAnimation(0, "open", true, 0.0f);
    chainDoor.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(chainDoor.AnimationState_Event);
    GameManager.GetInstance().OnConversationNext(chainDoor.gameObject, chainDoor.Zoom += 5f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 4.2f);
    yield return (object) new WaitForSeconds(5.2f);
    yield return (object) new WaitForSeconds(2.5f);
    GameManager.GetInstance().OnConversationEnd();
    chainDoor.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(chainDoor.AnimationState_Event);
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "break":
        CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
        GameManager.GetInstance().OnConversationNext(this.gameObject, ++this.Zoom);
        break;
      case "chainbreak":
        CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.5f);
        GameManager.GetInstance().OnConversationNext(this.gameObject, this.Zoom += 3f);
        break;
      case "bigchainbreak":
        CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.7f);
        GameManager.GetInstance().OnConversationNext(this.gameObject, this.Zoom += 5f + (this.ZoomToAdd -= 2f));
        break;
    }
  }
}
