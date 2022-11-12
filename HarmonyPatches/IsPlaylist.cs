using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using HarmonyLib;
using HMUI;
using SongDescriptionButItWorks.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlaylistManager.UI;

namespace SongDescriptionButItWorks.HarmonyPatches
{
    [HarmonyPatch(
        typeof(LevelDetailButtonsViewController),
        nameof(LevelDetailButtonsViewController.LevelCollectionUpdated)
    )]
    static class IsPlaylist
    {


        static void Postfix(
            LevelDetailButtonsViewController __instance,
            Boolean ____isPlaylistSong
            // IPlaylist selectedPlaylist
         )
        {
            Plugin.Log.Info($"____isPlaylistSong {____isPlaylistSong}");
            Plugin.detail.SetIsPlaylist(____isPlaylistSong);
        }
    }
}
