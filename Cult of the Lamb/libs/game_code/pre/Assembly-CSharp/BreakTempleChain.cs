// Decompiled with JetBrains decompiler
// Type: BreakTempleChain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class BreakTempleChain : BaseMonoBehaviour
{
  public RoomSwapManager RoomSwapManager;
  public TempleChain TempleChain;
  public GameObject PlayerPosition;

  private void OnEnable() => this.StartCoroutine((IEnumerator) this.EnableRoutine());

  private IEnumerator EnableRoutine()
  {
    BreakTempleChain breakTempleChain = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(breakTempleChain.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    breakTempleChain.StartCoroutine((IEnumerator) breakTempleChain.FadeRoutine());
    float Progress = 0.0f;
    float Duration = 3f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetZoom((float) (6.0 - 4.0 * (double) Progress / (double) Duration));
      CameraManager.shakeCamera((float) (0.10000000149011612 + 0.60000002384185791 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
  }

  private IEnumerator FadeRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BreakTempleChain breakTempleChain = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.WhiteFade, MMTransition.NO_SCENE, 3f, "", new System.Action(breakTempleChain.ChangeRoom));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void ChangeRoom()
  {
    PlayerFarming.Instance.transform.position = this.PlayerPosition.transform.position;
    PlayerFarming.Instance.state.facingAngle = 90f;
    this.RoomSwapManager.ToggleChurch();
    CameraManager.shakeCamera(0.5f);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.transform.position);
    this.TempleChain.Play();
  }
}
