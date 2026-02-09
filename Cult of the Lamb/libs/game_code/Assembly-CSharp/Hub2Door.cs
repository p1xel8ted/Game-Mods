// Decompiled with JetBrains decompiler
// Type: Hub2Door
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Hub2Door : BaseMonoBehaviour
{
  public BoxCollider2D Collider;
  public GameObject ShakeObject;

  public void Start()
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

  public IEnumerator PlayRoutine()
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
