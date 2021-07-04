using MelonLoader;
using System;
using System.Collections;
using System.Linq;
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
                MelonLogger.Error("Unable to get game info - quitting!");
                yield break;
            }

            SongManager songManager = FindObjectOfType<SongManager>();
            if (songManager == null)
            {
                int i = 0;
                while (songManager == null)
                {
                    yield return new WaitForSeconds(0.2f);
                    songManager = FindObjectOfType<SongManager>();
                    i++;

                    if (i > 50)
                    {
                        MelonLogger.Error("Unable to get song manager - quitting!");
                        yield break;
                    }
                }
            }

            AlbumSongs_SO songData = songManager.CurrentSong;
            if (songData == null)
            {
                MelonLogger.Error("Unable to get song data - quitting!");
                yield break;
            }

            // Prepare the data we can output
            string songAuthor = songData.Author;
            string songName = songData.Name;
            string songDifficulty = WallDanceUtils.GetLocalizedText(songData.Levels[songManager.CurrentSongLevel].Difficulty.ToString().ToUpper());
            string songLength = TimeSpan.FromSeconds(songData.AudioTime).ToString(@"%m\:ss");
            string songAccuracyMultiplier = (1 / WallDanceUtils.CurrentPrecisionMultiplier).ToString("F1");
            string songSpeedMultiplier = WallDanceUtils.CurrentSpeedMultiplier.ToString("F1");
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

            MelonLogger.Msg("Song info written to disk successfully for song name '" + songName + "' by '" + songAuthor + "'");

            yield return null;
        }
    }
}
