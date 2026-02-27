// Decompiled with JetBrains decompiler
// Type: CrownStatueController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CrownStatueController : MonoBehaviour
{
  public static CrownStatueController Instance;
  public Interaction EnterEndlessMode;
  public GameObject Haro;
  public GameObject Container;
  public GameObject CameraPosition;

  private void OnEnable()
  {
    CrownStatueController.Instance = this;
    this.Container.SetActive(true);
    this.Haro.SetActive(false);
    this.EnterEndlessMode.enabled = false;
  }

  private void OnDisable() => CrownStatueController.Instance = (CrownStatueController) null;

  public void EndlessModeOnboarded()
  {
    this.StartCoroutine((IEnumerator) this.EndlessModeOnboardRoutine());
  }

  private IEnumerator EndlessModeOnboardRoutine()
  {
    CrownStatueController statueController = this;
    DataManager.Instance.OnboardedEndlessMode = true;
    yield return (object) null;
    PlayerFarming.Instance.GoToAndStop(statueController.transform.position + new Vector3(0.0f, -1.5f), statueController.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(statueController.CameraPosition);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(statueController.CameraPosition, 5f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", statueController.transform.position);
    statueController.Container.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(statueController.transform.position, new Vector3(5f, 5f, 2f));
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    statueController.EnterEndlessMode.enabled = true;
  }
}
