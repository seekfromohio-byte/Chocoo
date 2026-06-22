// Decompiled with JetBrains decompiler
// Type: X.AccurateDisconnectSelfPatch
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using AmongUs.Data;
using chocoomenu;
using HarmonyLib;
using InnerNet;
using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace X;

[HarmonyPatch(typeof (DisconnectPopup), "DoShow")]
public static class AccurateDisconnectSelfPatch
{
  public static bool Prefix(DisconnectPopup __instance)
  {
    if (!ChocooPlugin.AccurateDisconnectReasonsEnabled)
      return true;
    DisconnectReasons disconnectReason = ((InnerNetClient) AmongUsClient.Instance).LastDisconnectReason;
    DisconnectReasons disconnectReasons = disconnectReason;
    string str1;
    string str2;
    if (disconnectReasons <= 116)
    {
      switch ((int) disconnectReasons)
      {
        case 0:
        case 16 /*0x10*/:
          break;
        case 1:
          str1 = "Lobby is full.";
          str2 = "You could not join because the room is full.";
          goto label_34;
        case 2:
          str1 = "Game already started.";
          str2 = "You cannot join a game in progress.";
          goto label_34;
        case 3:
          str1 = "Game not found.";
          str2 = "The lobby may have closed.";
          goto label_34;
        case 4:
        case 8:
        case 12:
        case 13:
        case 14:
        case 15:
        case 18:
          goto label_33;
        case 5:
        case 21:
          str1 = "Version mismatch.";
          str2 = "Your game version does not match the server.";
          goto label_34;
        case 6:
          str1 = "You were banned from the room.";
          str2 = "You can rejoin that room by changing IP.";
          goto label_34;
        case 7:
          str1 = "You were kicked from the room.";
          str2 = "You may rejoin if the host allows it.";
          goto label_34;
        case 9:
          str1 = "Your name was rejected.";
          str2 = "Please choose a different name.";
          goto label_34;
        case 10:
          str1 = "You were banned by the Anticheat for Hacking.";
          str2 = "Please stop.";
          goto label_34;
        case 11:
          str1 = "Not authorized.";
          str2 = "You are not authenticated to join this game.";
          goto label_34;
        case 17:
          str1 = "You were disconnected due to an error.";
          str2 = "Please try rejoining.";
          goto label_34;
        case 19:
label_15:
          str1 = "Internal server error.";
          str2 = $"Code: {(ValueType) (int) disconnectReason}";
          goto label_34;
        case 20:
          str1 = "Server is full.";
          str2 = "Please try again later.";
          goto label_34;
        default:
          switch (disconnectReasons - 100)
          {
            case 0:
            case 1:
            case 2:
              goto label_15;
            case 3:
            case 14:
              str1 = "Platform restriction.";
              str2 = "You cannot join due to crossplay or platform settings.";
              goto label_34;
            case 4:
              str1 = "Removed for inactivity.";
              str2 = "You were idle in the lobby for too long.";
              goto label_34;
            case 5:
              str1 = "Matchmaker inactivity.";
              str2 = "You were disconnected due to matchmaker timeout.";
              goto label_34;
            case 7:
              str1 = "No servers available.";
              str2 = "Please try again later.";
              goto label_34;
            case 10:
              str1 = "Chat mode mismatch.";
              str2 = "Your chat mode does not match this lobby's requirements.";
              goto label_34;
            case 12:
              str1 = "Your account has been sanctioned.";
              str2 = "You have been restricted from playing online.";
              goto label_34;
            case 15:
              str1 = "Duplicate connection detected.";
              str2 = "You are already connected from another device.";
              goto label_34;
            case 16 /*0x10*/:
              str1 = "Too many requests.";
              str2 = "You have been temporarily rate limited.";
              goto label_34;
            default:
              goto label_33;
          }
      }
    }
    else
    {
      switch (disconnectReasons - 208 /*0xD0*/)
      {
        case 0:
          str1 = "You left a game early.";
          str2 = $"You have {DataManager.Player.Ban.BanMinutesLeft} minute(s) remaining on your penalty.";
          goto label_34;
        case 1:
        case 2:
        case 5:
        case 6:
          goto label_33;
        case 3:
          str1 = "Blocked by parental controls.";
          str2 = "Your platform's parental settings prevent you from playing online.";
          goto label_34;
        case 4:
          str1 = "Blocked by a user.";
          str2 = "A player in this lobby has blocked you.";
          goto label_34;
        case 7:
          str1 = "Connection timed out.";
          str2 = "You lost connection to the server.";
          goto label_34;
        case 8:
          str1 = "Authentication failed.";
          str2 = "Please restart the game and try again.";
          goto label_34;
        default:
          if (disconnectReasons == (int) byte.MaxValue)
            break;
          goto label_33;
      }
    }
    return true;
label_33:
    str1 = "You were disconnected.";
    str2 = $"Reason: {disconnectReason} ({(ValueType) (int) disconnectReason})";
label_34:
    ((TMP_Text) __instance._textArea).text = $"{str1}\n\n{str2}";
    ((Component) __instance).gameObject.SetActive(true);
    return false;
  }
}
