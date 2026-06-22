// Decompiled with JetBrains decompiler
// Type: X.DisableVentingPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (PlayerControl), "FixedUpdate")]
public static class DisableVentingPatch
{
  private static int ventKickCooldown;

  public static void Postfix(PlayerControl __instance)
  {
    try
    {
      if (!ChocooPlugin.DisableVentingEnabled)
        return;
      if (__instance.inVent)
      {
        if (DisableVentingPatch.ventKickCooldown <= 0)
        {
          if (Object.op_Inequality((Object) ShipStatus.Instance, (Object) null) && ShipStatus.Instance.AllVents != null)
          {
            foreach (Vent allVent in (Il2CppArrayBase<Vent>) ShipStatus.Instance.AllVents)
              VentilationSystem.Update((VentilationSystem.Operation) 5, allVent.Id);
          }
          DisableVentingPatch.ventKickCooldown = 15;
        }
        else
          --DisableVentingPatch.ventKickCooldown;
      }
      else
      {
        if (__instance.inVent)
          return;
        bool flag = false;
        if (PlayerControl.AllPlayerControls != null)
        {
          foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
          {
            if (Object.op_Inequality((Object) allPlayerControl, (Object) null) && allPlayerControl.inVent)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          DisableVentingPatch.ventKickCooldown = 0;
      }
    }
    catch (Exception ex)
    {
    }
  }
}
