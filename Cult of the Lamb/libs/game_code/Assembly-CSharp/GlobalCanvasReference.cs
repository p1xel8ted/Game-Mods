// Decompiled with JetBrains decompiler
// Type: GlobalCanvasReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GlobalCanvasReference : MonoBehaviour
{
  public static Transform instance;
  public static Canvas canvasInstance;
  public static bool isInitialized;

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

  public void Awake()
  {
    GlobalCanvasReference.canvasInstance = this.transform.GetComponent<Canvas>();
    GlobalCanvasReference.instance = this.transform;
    GlobalCanvasReference.isInitialized = true;
  }

  public void OnDestroy()
  {
    GlobalCanvasReference.canvasInstance = (Canvas) null;
    GlobalCanvasReference.instance = (Transform) null;
    GlobalCanvasReference.isInitialized = false;
  }
}
