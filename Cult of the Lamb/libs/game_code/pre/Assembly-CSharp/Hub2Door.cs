// Decompiled with JetBrains decompiler
// Type: Hub2Door
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Hub2Door : BaseMonoBehaviour
{
  public BoxCollider2D Collider;
  public GameObject ShakeObject;

  private void Start()
  {
    Debug.Log((object) ("DataManager.Instance.BaseDoorEast " + DataManager.Instance.BaseDoorEast.ToString()));
    Debug.Log((object) ("DataManager.Instance.Chain1 " + DataManager.Instance.Chain1.ToString()));
    if (!DataManager.Instance.BaseDoorEast && DataManager.Instance.Chain1)
    {
      this.StartCoroutine((IEnumerator) this.PlayRoutine());
    }
    else
    {
      if (!DataManager.Instance.BaseDoorEast)
        return;
      Object.Destroy((Object) this.gameObject);
    }
  }

  private IEnumerator PlayRoutine()
  {
    Hub2Door hub2Door = this;
    Debug.Log((object) "PLAY ROUTINE!");
    yield return (object) new WaitForSeconds(3f);
    SimpleSetCamera.DisableAll();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(hub2Door.gameObject);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.shakeCamera(0.5f);
    hub2Door.Collider.enabled = false;
    hub2Door.ShakeObject.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    SimpleSetCamera.EnableAll();
    DataManager.Instance.BaseDoorEast = true;
    Object.Destroy((Object) hub2Door.gameObject);
  }
}
