// Decompiled with JetBrains decompiler
// Type: X.AutoKillPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (KillButton), "SetTarget")]
public static class AutoKillPatch
{
  public static void Postfix(KillButton __instance)
  {
    try
    {
      if (!ChocooPlugin.AutoKillEnabled || Object.op_Equality((Object) PlayerControl.LocalPlayer, (Object) null) || Object.op_Equality((Object) PlayerControl.LocalPlayer.Data, (Object) null) || !PlayerControl.LocalPlayer.Data.Role.IsImpostor)
        return;
      PlayerControl currentTarget = __instance.currentTarget;
      if (Object.op_Equality((Object) currentTarget, (Object) null) || Object.op_Inequality((Object) currentTarget.Data, (Object) null) && Object.op_Inequality((Object) currentTarget.Data.Role, (Object) null) && currentTarget.Data.Role.IsImpostor && !ChocooPlugin.KillOtherImpostersEnabled || !PlayerControl.LocalPlayer.CanMove || (double) PlayerControl.LocalPlayer.killTimer > 0.0)
        return;
      PlayerControl.LocalPlayer.CmdCheckMurder(currentTarget);
    }
    catch
    {
    }
  }
}
