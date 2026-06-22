// Decompiled with JetBrains decompiler
// Type: X.DisableKillAnimationPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using System;

#nullable disable
namespace X;

[HarmonyPatch(typeof (KillOverlay))]
public static class DisableKillAnimationPatch
{
  [HarmonyPatch("ShowKillAnimation")]
  [HarmonyPatch(new Type[] {typeof (NetworkedPlayerInfo), typeof (NetworkedPlayerInfo)})]
  [HarmonyPrefix]
  public static bool Prefix(NetworkedPlayerInfo killer, NetworkedPlayerInfo victim)
  {
    if (!ChocooPlugin.DisableKillAnimationEnabled)
      return true;
    ChocooPlugin.Logger.LogInfo((object) "Blocking kill animation");
    return false;
  }
}
