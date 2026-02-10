// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeyWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class KeyWidget : BaseMonoBehaviour
{
  [SerializeField]
  public DLCKeyType keyType;

  public DLCKeyType KeyType => this.keyType;
}
