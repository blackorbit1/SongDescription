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

namespace SongDescriptionButItWorks.HarmonyPatches
{
    [HarmonyPatch(
        typeof(StandardLevelDetailView), 
        nameof(StandardLevelDetailView.SetContent)
    )]
    static class SelectedSongChanged
    {
        

        static void Postfix(StandardLevelDetailView __instance, IBeatmapLevel level)
        {
            Plugin.detail.SetLevel(level);

            if (Plugin.detail.buttonGo != null)
                return;

            // Create the UI if it isnt yet (Button and Modal)
            BSMLParser.instance.Parse(
                Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "SongDescriptionButItWorks.UI.detailui.bsml"),
                __instance.gameObject,
                Plugin.detail
            );
        }
    }
}
