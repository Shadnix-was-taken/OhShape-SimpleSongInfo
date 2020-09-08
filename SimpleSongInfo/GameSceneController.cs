using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleSongInfo
{
    public class GameSceneController : MonoBehaviour
    {
        private void Start() => Load();

        private void Load()
        {
            StartCoroutine(GetSongInformation());
        }

        private IEnumerator GetSongInformation()
        {
            GameInfo gameInfo = Resources.FindObjectsOfTypeAll<GameInfo>().FirstOrDefault();
            if (gameInfo == null)
            {
                MelonLogger.LogError("Unable to get game info - quitting!");
                yield return null;
            }

            //SongManager songManager = gameInfo._songManager;
            //SongManager songManager = Util.ReflectionUtil.GetPrivateField<SongManager>(gameInfo, "_songManager");
            yield return new WaitForSeconds(0.2f);
            SongManager songManager = UnityEngine.Object.FindObjectOfType<SongManager>();
            if (songManager == null)
            {
                MelonLogger.LogError("Unable to get song manager - quitting!");
                yield return null;
            }

            AlbumSongs_SO songData = songManager.CurrentSong;
            if (songData == null)
            {
                MelonLogger.LogError("Unable to get song data - quitting!");
                yield return null;
            }

            // Prepare the data we can output
            string songAuthor = songData.Author;
            string songName = songData.Name;
            string songDifficulty = songData.Levels[songManager.CurrentSongLevel].Difficulty.ToString();
            string songLength = TimeSpan.FromSeconds(songData.AudioTime).ToString(@"%m\:ss");
            string songAccuracyMultiplier = (1 / WallDanceUtils.CurrentPrecisionMultiplier).ToString("F1");
            string songSpeedMultiplier = WallDanceUtils.CurrentSpeedMultiplier.ToString("F1");
            //string songObjectSpeed = songManager._objectsSpeed.ToString("F0");
            string songObjectSpeed = Util.ReflectionUtil.GetPrivateField<float>(songManager, "_objectsSpeed").ToString("F0");

            // Replace values in HTML template
            string htmlData = Mod.htmlSongInfo.Replace("%songauthor%", songAuthor);
            htmlData = htmlData.Replace("%songname%", songName);
            htmlData = htmlData.Replace("%difficulty%", songDifficulty);
            htmlData = htmlData.Replace("%accuracymultiplier%", songAccuracyMultiplier);
            htmlData = htmlData.Replace("%speedmultiplier%", songSpeedMultiplier);
            htmlData = htmlData.Replace("%objectspeed%", songObjectSpeed);

            // Replace values in TXT template
            string txtData = Mod.txtSongInfo.Replace("%songauthor%", songAuthor);
            txtData = txtData.Replace("%songname%", songName);
            txtData = txtData.Replace("%difficulty%", songDifficulty);
            txtData = txtData.Replace("%accuracymultiplier%", songAccuracyMultiplier);
            txtData = txtData.Replace("%speedmultiplier%", songSpeedMultiplier);
            txtData = txtData.Replace("%objectspeed%", songObjectSpeed);

            // Output data to disk
            FileHandler.WriteSongInfo(htmlData, FileHandler.FileTemplateType.HTML);
            FileHandler.WriteSongInfo(txtData, FileHandler.FileTemplateType.TXT);

            yield return null;
        }
    }
}
