// Decompiled with JetBrains decompiler
// Type: StaticGameObjectReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StaticGameObjectReference : BaseMonoBehaviour
{
  public BaseMonoBehaviour BaseMonoBehaviour;
  public Collider2D collider2D;
  public static StaticGameObjectReference Instance;

  private void OnEnable() => StaticGameObjectReference.Instance = this;

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
