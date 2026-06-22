// Decompiled with JetBrains decompiler
// Type: X.CloneMgr
// Assembly: Area51files, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00A892C3-13C3-404B-9CB9-1FDCB50BD8FF
// Assembly location: C:\Users\Administrator\Desktop\Among us Mod menus\ChocooMenu v1.0.8_dev2.dll

using chocoomenu;
using Hazel;
using InnerNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

#nullable disable
namespace X;

public static class CloneMgr
{
  public static List<(PlayerControl clone, byte sourcePlayerId, string sourceName)> L = new List<(PlayerControl, byte, string)>();

  public static int Add() => CloneMgr.AddOf(PlayerControl.LocalPlayer);

  public static int AddOf(PlayerControl source)
  {
    try
    {
      if (!((InnerNetClient) AmongUsClient.Instance).AmHost || Object.op_Equality((Object) source, (Object) null))
        return -1;
      PlayerControl localPlayer = PlayerControl.LocalPlayer;
      if (Object.op_Equality((Object) localPlayer, (Object) null))
        return -1;
      Vector2 vector2 = Vector2.op_Implicit(((Component) localPlayer).transform.position);
      PlayerControl cl = Object.Instantiate<PlayerControl>(AmongUsClient.Instance.PlayerPrefab);
      cl.PlayerId = source.PlayerId;
      cl.isNew = false;
      cl.notRealPlayer = true;
      ((InnerNetClient) AmongUsClient.Instance).Spawn((InnerNetObject) cl, -2, (SpawnFlags) 0);
      ((Component) cl).transform.position = Vector2.op_Implicit(vector2);
      if (Object.op_Inequality((Object) cl.NetTransform, (Object) null))
        cl.NetTransform.SnapTo(vector2);
      string nm = source.Data?.PlayerName ?? "???";
      CloneMgr.L.Add((cl, source.PlayerId, nm));
      int count = CloneMgr.L.Count;
      CoroutineHelper.Start(CloneMgr.SetNameDelayed(cl, count, nm, 0.2f));
      CoroutineHelper.Start(CloneMgr.SetNameDelayed(cl, count, nm, 0.5f));
      CoroutineHelper.Start(CloneMgr.SetNameDelayed(cl, count, nm, 1f));
      return count;
    }
    catch (Exception ex)
    {
      ChocooPlugin.Logger.LogError((object) ("CloneMgr.AddOf: " + ex.ToString()));
      return -1;
    }
  }

  private static IEnumerator RemoveFromListDelayed(PlayerControl cl, float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    if (PlayerControl.AllPlayerControls.Contains(cl))
      PlayerControl.AllPlayerControls.Remove(cl);
  }

  private static IEnumerator SetNameDelayed(PlayerControl cl, int n, string nm, float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    CloneMgr.SetName(cl, n, nm);
  }

  private static void SetName(PlayerControl cl, int n, string nm)
  {
    if (!Object.op_Inequality((Object) cl?.cosmetics?.nameText, (Object) null))
      return;
    ((TMP_Text) cl.cosmetics.nameText).text = $"<color=#888>[{n}]</color> {nm}";
  }

  public static void Remove(int idx)
  {
    if (idx < 0 || idx >= CloneMgr.L.Count)
      return;
    (PlayerControl clone, byte sourcePlayerId, string sourceName) tuple = CloneMgr.L[idx];
    CloneMgr.L.RemoveAt(idx);
    if (Object.op_Inequality((Object) tuple.clone, (Object) null))
    {
      try
      {
        MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
        messageWriter.StartMessage((byte) 5);
        messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
        messageWriter.StartMessage((byte) 5);
        messageWriter.WritePacked(((InnerNetObject) tuple.clone).NetId);
        messageWriter.EndMessage();
        messageWriter.EndMessage();
        ((InnerNetClient) AmongUsClient.Instance).SendOrDisconnect(messageWriter);
        messageWriter.Recycle();
        ((InnerNetClient) AmongUsClient.Instance).RemoveNetObject((InnerNetObject) tuple.clone);
        Object.Destroy((Object) ((Component) tuple.clone).gameObject);
      }
      catch
      {
      }
    }
    CloneMgr.UpdNames();
  }

  public static void Clear()
  {
    foreach ((PlayerControl clone, byte sourcePlayerId, string sourceName) tuple in CloneMgr.L.ToArray())
    {
      if (Object.op_Inequality((Object) tuple.clone, (Object) null))
      {
        try
        {
          MessageWriter messageWriter = MessageWriter.Get((SendOption) 1);
          messageWriter.StartMessage((byte) 5);
          messageWriter.Write(((InnerNetClient) AmongUsClient.Instance).GameId);
          messageWriter.StartMessage((byte) 5);
          messageWriter.WritePacked(((InnerNetObject) tuple.clone).NetId);
          messageWriter.EndMessage();
          messageWriter.EndMessage();
          ((InnerNetClient) AmongUsClient.Instance).SendOrDisconnect(messageWriter);
          messageWriter.Recycle();
          ((InnerNetClient) AmongUsClient.Instance).RemoveNetObject((InnerNetObject) tuple.clone);
          Object.Destroy((Object) ((Component) tuple.clone).gameObject);
        }
        catch
        {
        }
      }
    }
    CloneMgr.L.Clear();
  }

  public static void ClearMine()
  {
    PlayerControl localPlayer = PlayerControl.LocalPlayer;
    CloneMgr.ClearOf(Object.op_Inequality((Object) localPlayer, (Object) null) ? localPlayer.PlayerId : byte.MaxValue);
  }

  public static void ClearOf(byte playerId)
  {
    List<int> intList = new List<int>();
    for (int index = CloneMgr.L.Count - 1; index >= 0; --index)
    {
      if ((int) CloneMgr.L[index].sourcePlayerId == (int) playerId)
        intList.Add(index);
    }
    foreach (int idx in intList)
      CloneMgr.Remove(idx);
  }

  public static void UpdNames()
  {
    for (int index = 0; index < CloneMgr.L.Count; ++index)
    {
      (PlayerControl clone, byte sourcePlayerId, string sourceName) tuple = CloneMgr.L[index];
      if (Object.op_Inequality((Object) tuple.clone?.cosmetics?.nameText, (Object) null))
        ((TMP_Text) tuple.clone.cosmetics.nameText).text = $"<color=#888>[{index + 1}]</color> {tuple.sourceName}";
    }
  }

  public static string GetCloneName(int idx)
  {
    return idx < 0 || idx >= CloneMgr.L.Count ? "???" : CloneMgr.L[idx].sourceName;
  }

  public static int GetCloneCount(byte playerId)
  {
    return CloneMgr.L.Count<(PlayerControl, byte, string)>((Func<(PlayerControl, byte, string), bool>) (e => (int) e.sourcePlayerId == (int) playerId));
  }

  public static List<int> GetCloneIndices(byte playerId)
  {
    List<int> cloneIndices = new List<int>();
    for (int index = 0; index < CloneMgr.L.Count; ++index)
    {
      if ((int) CloneMgr.L[index].sourcePlayerId == (int) playerId)
        cloneIndices.Add(index);
    }
    return cloneIndices;
  }

  public static List<int> GetMyCloneIndices()
  {
    PlayerControl localPlayer = PlayerControl.LocalPlayer;
    return CloneMgr.GetCloneIndices(Object.op_Inequality((Object) localPlayer, (Object) null) ? localPlayer.PlayerId : byte.MaxValue);
  }
}
