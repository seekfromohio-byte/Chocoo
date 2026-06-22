// Decompiled with JetBrains decompiler
// Type: X.RpcPlayAnimationPatch
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

[HarmonyPatch(typeof (PlayerControl), "RpcPlayAnimation")]
public static class RpcPlayAnimationPatch
{
  public static bool Prefix(PlayerControl __instance, byte animType)
  {
    if (!ChocooPlugin.BypassVisualTasksEnabled || !Object.op_Equality((Object) __instance, (Object) PlayerControl.LocalPlayer))
      return true;
    __instance.PlayAnimation(animType);
    MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) __instance).NetId, (byte) 0, (SendOption) 1, -1);
    messageWriter.Write(animType);
    ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
    return false;
  }
}
