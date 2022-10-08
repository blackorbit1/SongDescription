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

        [UIComponent("button")] public readonly NoTransitionsButton buttonGo = null;
        [UIComponent("description")] public readonly CurvedTextMeshPro descriptionText = null;


        internal void SetLevel(IBeatmapLevel level)
        {
            lastLevel = level;
        }

        async void LoadDescription()
        {
            if(!lastLevel.levelID.StartsWith("custom_level_"))
            {
                descriptionText.text = "Not a custom level";
                return;
            }

            descriptionText.text = "Loading...";

            var hash = lastLevel.levelID.Substring(13, 40);

            try
            {
                var description = await Task.Run(() => {
                    var download = new WebClient().DownloadData($"https://api.beatsaver.com/maps/hash/{hash}");

                    using (var jsonReader = new JsonTextReader(new StreamReader(new MemoryStream(download))))
                    {
                        var ser = new JsonSerializer();

                        return ser.Deserialize<JObject>(jsonReader).GetValue("description").Value<string>();
                    }
                });

                descriptionText.text = description;
                descriptionText.gameObject.SetActive(false);
                descriptionText.gameObject.SetActive(true);
            } 
            catch
            {
                descriptionText.text = "Failed to load Description";
            }
        }
    }
}
