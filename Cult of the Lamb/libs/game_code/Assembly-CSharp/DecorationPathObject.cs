// Decompiled with JetBrains decompiler
// Type: DecorationPathObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DecorationPathObject : BaseMonoBehaviour
{
  [SerializeField]
  public Structure structure;

  public void Awake() => this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  public void OnBrainAssigned()
  {
    PathTileManager.Instance.SetTile(this.structure.Type, this.transform.position);
    this.structure.OnProgressCompleted.RemoveListener(new UnityAction(this.OnBrainAssigned));
    this.Invoke("RemoveStruct", 0.01f);
  }

  public void RemoveStruct() => this.structure.RemoveStructure();
}
