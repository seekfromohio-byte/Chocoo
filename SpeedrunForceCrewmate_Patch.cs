// Decompiled with JetBrains decompiler
// Type: X.SpeedrunForceCrewmate_Patch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using HarmonyLib;
using Hazel;
using InnerNet;

#nullable disable
namespace X;

[HarmonyPatch(typeof (RoleManager), "SelectRoles")]
public static class SpeedrunForceCrewmate_Patch
{
  public static bool Prefix()
  {
    if (!((InnerNetClient) AmongUsClient.Instance).AmHost || !ChocooPlugin.TaskSpeedrunEnabled)
      return true;
    foreach (PlayerControl allPlayerControl1 in PlayerControl.AllPlayerControls)
    {
      foreach (PlayerControl allPlayerControl2 in PlayerControl.AllPlayerControls)
      {
        MessageWriter messageWriter = ((InnerNetClient) AmongUsClient.Instance).StartRpcImmediately(((InnerNetObject) allPlayerControl1).NetId, (byte) 44, (SendOption) 1, ((InnerNetObject) allPlayerControl2).OwnerId);
        messageWriter.Write((ushort) 0);
        messageWriter.Write(false);
        ((InnerNetClient) AmongUsClient.Instance).FinishRpcImmediately(messageWriter);
      }
    }
    return false;
  }
}
