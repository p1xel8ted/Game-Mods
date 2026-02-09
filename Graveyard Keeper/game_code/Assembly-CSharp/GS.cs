// Decompiled with JetBrains decompiler
// Type: GS
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
public class GS
{
  public static GameObject _globals_scripts;

  public static WorldGameObject Spawn(string obj_id, Transform pos = null, string custom_tag = "")
  {
    WorldGameObject context = WorldMap.SpawnWGO(MainGame.me.world_root, obj_id, new Vector3?((Object) pos == (Object) null ? Vector3.zero : pos.position));
    context.custom_tag = custom_tag;
    context.OnJustSpawned();
    foreach (GraphOwner componentsInChild in context.GetComponentsInChildren<GraphOwner>())
    {
      componentsInChild.StartBehaviour();
      Debug.Log((object) componentsInChild.name, (Object) componentsInChild);
    }
    Debug.Log((object) $"Spawned {obj_id} ({context.obj_id}) at: {(context.transform.position / 96f).ToString()}", (Object) context);
    return context;
  }

  public void GoTo(Vector2 dest)
  {
  }

  public static GameObject global_scripts
  {
    get
    {
      if ((Object) GS._globals_scripts != (Object) null)
        return GS._globals_scripts;
      GS._globals_scripts = GameObject.Find("* Global Scripts");
      if ((Object) GS._globals_scripts != (Object) null)
        return GS._globals_scripts;
      GS._globals_scripts = new GameObject("* Global Scripts");
      return GS._globals_scripts;
    }
  }

  public static CustomFlowScript RunFlowScript(
    string uscript_name,
    CustomFlowScript.OnFinishedDelegate on_finished = null)
  {
    return CustomFlowScript.Create(GS.global_scripts, uscript_name, true, on_finished);
  }

  public static void SetPlayerEnable(bool player_enabled, bool affect_cinematic)
  {
    Debug.Log((object) $"Set player {(player_enabled ? "enabled" : "disabled")}, affect_cinematic = {affect_cinematic.ToString()}");
    MainGame.me.player_char.control_enabled = player_enabled;
    GUIElements.ChangeHUDAlpha(player_enabled, affect_cinematic);
    GUIElements.ChangeBubblesVisibility(player_enabled);
    GUIElements.me.overhead_panel.gameObject.SetActive(player_enabled);
    MainGame.me.player.components.character.body.bodyType = player_enabled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    if (affect_cinematic)
      CameraTools.TweenLetterbox(!player_enabled);
    if (affect_cinematic | player_enabled)
      GUIElements.me.relation.ChangeHUDAlpha(player_enabled, false);
    GUIElements.me.relation.Update();
  }

  public static void AffectCinematic(bool show)
  {
    bool controlEnabled = MainGame.me.player_char.control_enabled;
    GUIElements.ChangeHUDAlpha(controlEnabled, show);
    GUIElements.ChangeBubblesVisibility(controlEnabled);
    GUIElements.me.overhead_panel.gameObject.SetActive(controlEnabled);
    CameraTools.TweenLetterbox(show);
    if (show | controlEnabled)
      GUIElements.me.relation.ChangeHUDAlpha(controlEnabled, false);
    GUIElements.me.relation.Update();
  }

  public static bool IsPlayerEnable() => MainGame.me.player_char.control_enabled;

  public static void AddCameraTarget(Transform transform)
  {
    CameraTools.AddToCameraTargets(transform);
  }

  public static void RemoveCameraTarget(Transform transform)
  {
    CameraTools.RemoveFromCameraTargets(transform);
  }
}
