// Decompiled with JetBrains decompiler
// Type: X.KillTimerDecrement_Patch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (PlayerControl), "FixedUpdate")]
public static class KillTimerDecrement_Patch
{
  public static void Postfix(PlayerControl __instance)
  {
    if (!ChocooPlugin.ShowKillCooldown || Object.op_Equality((Object) __instance, (Object) null))
      return;
    NetworkedPlayerInfo data = __instance.Data;
    if (Object.op_Equality((Object) data, (Object) null) || data.IsDead || Object.op_Equality((Object) data.Role, (Object) null) || !data.Role.CanUseKillButton || !Object.op_Implicit((Object) ShipStatus.Instance))
      return;
    if (Object.op_Inequality((Object) __instance, (Object) PlayerControl.LocalPlayer) && (__instance.ForceKillTimerContinue || __instance.IsKillTimerEnabled))
      __instance.killTimer = Mathf.Max(__instance.killTimer - Time.fixedDeltaTime, 0.0f);
    ChocooPlugin.PlayerNametagsPatch.ApplyNametag(__instance);
  }
}
