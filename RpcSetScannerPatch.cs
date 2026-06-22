// Decompiled with JetBrains decompiler
// Type: X.RpcSetScannerPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using Hazel;
using InnerNet;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (PlayerControl), "RpcSetScanner")]
public static class RpcSetScannerPatch
{
  public static bool Prefix(PlayerControl __instance, bool value)
  {
    if (!ChocooPlugin.BypassVisualTasksEnabled || !Object.op_Equality((Object) __instance, (Object) PlayerControl.LocalPlayer))
      return true;
    __instance.SetScanner(value, (byte) ((uint) __instance.scannerCount + 1U));
    MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) __instance).NetId, (byte) 15, (SendOption) 1, -1);
    messageWriter.Write(value);
    messageWriter.Write((byte) ((uint) __instance.scannerCount + 1U));
    ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
    return false;
  }
}
