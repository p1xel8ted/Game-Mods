// Decompiled with JetBrains decompiler
// Type: GlobalCanvasReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GlobalCanvasReference : MonoBehaviour
{
  private static Transform instance;
  private static Canvas canvasInstance;
  private static bool isInitialized;

  public static Canvas CanvasInstance => GlobalCanvasReference.canvasInstance;

  public static Transform Instance
  {
    get
    {
      if (GlobalCanvasReference.isInitialized)
        return GlobalCanvasReference.instance;
      GameObject withTag = GameObject.FindWithTag("Canvas");
      return (bool) (Object) withTag ? withTag.transform : GameObject.Find("Canvas").transform;
    }
  }

  private void Awake()
  {
    GlobalCanvasReference.canvasInstance = this.transform.GetComponent<Canvas>();
    GlobalCanvasReference.instance = this.transform;
    GlobalCanvasReference.isInitialized = true;
  }

  private void OnDestroy()
  {
    GlobalCanvasReference.canvasInstance = (Canvas) null;
    GlobalCanvasReference.instance = (Transform) null;
    GlobalCanvasReference.isInitialized = false;
  }
}
