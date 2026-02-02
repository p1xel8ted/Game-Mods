// Decompiled with JetBrains decompiler
// Type: StaticGameObjectReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StaticGameObjectReference : BaseMonoBehaviour
{
  public BaseMonoBehaviour BaseMonoBehaviour;
  public Collider2D collider2D;
  public static StaticGameObjectReference Instance;

  public void OnEnable() => StaticGameObjectReference.Instance = this;

  public static void EnableCollider2D()
  {
    StaticGameObjectReference.Instance.collider2D.enabled = true;
  }

  public static void DisableCollider2D()
  {
    StaticGameObjectReference.Instance.collider2D.enabled = false;
  }

  public static void EnableBaseMonoBehaviour()
  {
    StaticGameObjectReference.Instance.BaseMonoBehaviour.enabled = true;
  }

  public static void DisableBaseMonoBehaviour()
  {
    StaticGameObjectReference.Instance.BaseMonoBehaviour.enabled = false;
  }
}
