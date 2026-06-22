// Decompiled with JetBrains decompiler
// Type: X.PlayerSpeedPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (PlayerControl), "FixedUpdate")]
public static class PlayerSpeedPatch
{
  private static float baseSpeed = -1f;

  public static void Postfix(PlayerControl __instance)
  {
    if (!Object.op_Equality((Object) __instance, (Object) PlayerControl.LocalPlayer))
      return;
    if ((double) PlayerSpeedPatch.baseSpeed < 0.0 && Object.op_Inequality((Object) __instance.MyPhysics, (Object) null))
      PlayerSpeedPatch.baseSpeed = __instance.MyPhysics.Speed;
    if (ChocooPlugin.SpeedHackEnabled && (double) PlayerSpeedPatch.baseSpeed > 0.0)
      __instance.MyPhysics.Speed = ChocooPlugin.PlayerSpeed;
    else if (!ChocooPlugin.SpeedHackEnabled && (double) PlayerSpeedPatch.baseSpeed > 0.0)
      __instance.MyPhysics.Speed = PlayerSpeedPatch.baseSpeed;
  }
}
