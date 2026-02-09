// Decompiled with JetBrains decompiler
// Type: CustomNetworkComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine.Networking;

#nullable disable
public class CustomNetworkComponent : NetworkBehaviour
{
  public bool is_locally_controlled = true;

  public void UNetVersion()
  {
  }

  public override bool OnSerialize(NetworkWriter writer, bool forceAll)
  {
    bool flag;
    return flag;
  }

  public override void OnDeserialize(NetworkReader reader, bool initialState)
  {
  }

  public override void PreStartClient()
  {
  }
}
