// Decompiled with JetBrains decompiler
// Type: X.SpeedrunLobbyResultChat_Patch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using BepInEx.Unity.IL2CPP.Utils;
using chocoomenu;
using HarmonyLib;
using InnerNet;
using System.Collections;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (GameStartManager), "Start")]
public static class SpeedrunLobbyResultChat_Patch
{
  public static void Postfix()
  {
    if (!((InnerNetClient) AmongUsClient.Instance).AmHost || string.IsNullOrEmpty(ChocooPlugin.SpeedrunLastResult) || !ChocooPlugin.TaskSpeedrunEnabled)
      return;
    string speedrunLastResult = ChocooPlugin.SpeedrunLastResult;
    ChocooPlugin.SpeedrunLastResult = "";
    MonoBehaviourExtensions.StartCoroutine((MonoBehaviour) AmongUsClient.Instance, SpeedrunLobbyResultChat_Patch.SendResultDelayed(speedrunLastResult));
  }

  private static IEnumerator SendResultDelayed(string msg)
  {
    yield return (object) new WaitForSeconds(3f);
    if (Object.op_Inequality((Object) PlayerControl.LocalPlayer, (Object) null))
      PlayerControl.LocalPlayer.RpcSendChat(msg);
  }
}
