using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SongDescriptionButItWorks.UI
{
    class DetailViewController
    {
        static IPreviewBeatmapLevel lastLevel;
        static Boolean isPlaylist;

        [UIComponent("button")] public readonly NoTransitionsButton buttonGo = null;
        [UIComponent("description")] public readonly CurvedTextMeshPro description = null;
        [UIComponent("bsr")] public readonly CurvedTextMeshPro bsr = null;
        [UIComponent("mapperName")] public readonly CurvedTextMeshPro mapperName = null;
        [UIComponent("date")] public readonly CurvedTextMeshPro date = null;


        internal void SetLevel(IBeatmapLevel level)
        {
            lastLevel = level;
        }
        
        internal void SetIsPlaylist(Boolean newIsPlaylist)
        {
            isPlaylist = newIsPlaylist;
        }

        async void LoadDescription()
        {
            if(!lastLevel.levelID.StartsWith("custom_level_"))
            {
                description.text = "Not a custom level";
                return;
            }

            description.text = "Loading...";

            var hash = lastLevel.levelID.Substring(13, 40);

            try
            {
                var i = 1;
                Plugin.Log.Info($"{i++}");
                var mapInfo = await Task.Run(() => {
                    Plugin.Log.Info($"{i++}");
                    var download = new WebClient().DownloadData($"https://api.beatsaver.com/maps/hash/{hash}");
                    Plugin.Log.Info($"{i++}");

                    using (var jsonReader = new JsonTextReader(new StreamReader(new MemoryStream(download))))
                    {
                        Plugin.Log.Info($"{i++}");
                        var ser = new JsonSerializer();
                        Plugin.Log.Info($"{i++}");

                        return ser.Deserialize<JObject>(jsonReader);
                        // return isPlaylist ? "yes playlist" : "not playlist";
                    }
                });
                Plugin.Log.Info($"{i++}");

                Plugin.Log.Info($"description {mapInfo.GetValue("description").Value<string>()}");
                Plugin.Log.Info($"id {mapInfo.GetValue("id").Value<string>()}");
                Plugin.Log.Info($"updatedAt {mapInfo.GetValue("updatedAt").Value<string>()}");
                // Plugin.Log.Info($"uploader.name {mapInfo.GetValue("uploader.name").Value<string>()}");


                try{
                    description.text = mapInfo.GetValue("description").Value<string>();
                } catch { Plugin.Log.Error("Error while setting description"); }

                try
                {
                    bsr.text = mapInfo.GetValue("id").Value<string>();
                } catch { Plugin.Log.Error("Error while setting id"); }

                try
                {
                    date.text = mapInfo.GetValue("updatedAt").Value<string>();
                } catch { Plugin.Log.Error("Error while setting updatedAt"); }

                
                try
                {
                    // mapperName.text = mapInfo.GetValue("uploader.name").Value<string>();
                    mapperName.text = "blackorbit";
                } catch { Plugin.Log.Error("Error while setting uploader.name"); }
                
                
                
                

                Plugin.Log.Info($"description2 {description.text}");
                Plugin.Log.Info($"id2 {bsr.text}");
                Plugin.Log.Info($"updatedAt2 {date.text}");
                // Plugin.Log.Info($"uploader.name2 {mapperName.text}");

                description.gameObject.SetActive(false);
                description.gameObject.SetActive(true);
                bsr.gameObject.SetActive(false);
                bsr.gameObject.SetActive(true);
                date.gameObject.SetActive(false);
                date.gameObject.SetActive(true);
                mapperName.gameObject.SetActive(false);
                mapperName.gameObject.SetActive(true);
            } 
            catch
            {
                description.text = "Failed to load Description";
            }
        }
    }
}
