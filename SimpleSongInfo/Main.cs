using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SimpleSongInfo
{
    public static class BuildInfo
    {
        public const string Name = "Simple Song Info";          // Name of the Mod.  (MUST BE SET)
        public const string ShortName = "SimpleSongInfo";
        public const string Description = "Outputs current song data to files which you can use for a stream overlay."; // Description for the Mod.  (Set as null if none)
        public const string Author = "Shadnix";                 // Author of the Mod.  (Set as null if none)
        public const string Company = null;                     // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0";                  // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null;                // Download Link for the Mod.  (Set as null if none)
    }

    public class Mod : MelonMod
    {

        internal enum ModStates { Menu, Game };

        internal static string htmlEmptyInfo;       // html file template to write on game start and after exiting a map
        internal static string htmlSongInfo;        // html file template to write on song start, present during a song
        internal static string txtEmptyInfo;        // txt file template to write on game start and after exiting a map
        internal static string txtSongInfo;         // txt file template to write on song start, present during a song

        internal static ModStates modState = ModStates.Menu;

        // Initialize mod on startup
        public override void OnApplicationStart()
        {
            MelonLogger.Log("OnApplicationStart called. Initializing " + BuildInfo.Name + "...");
            Load();
        }


        // Gets called on scene change - check for current scene and updates files on disk if neccessary
        public override void OnLevelWasInitialized(int level)
        {

            // Check if game scene is active
            bool isGameScene = false;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                MelonLogger.Log("Scene number " + i.ToString() + " is called: " + scene.name);
                
                if (scene.name == "GameScene") { isGameScene = true; }
            }

            // If game scene is inactive, but mod is in game state, switch to menu state and write empty info templates to file
            if (!isGameScene && modState == ModStates.Game)
            {
                modState = ModStates.Menu;

                FileHandler.WriteSongInfo(htmlEmptyInfo, FileHandler.FileTemplateType.HTML);
                FileHandler.WriteSongInfo(txtEmptyInfo, FileHandler.FileTemplateType.TXT);
            }

            // If game scene is active, but mod is in menu state, switch to game state and also try to get and output game/song information
            if (isGameScene && modState == ModStates.Menu)
            {
                // MelonLogger.Log("GameScene is active. Trying to get song data...");
                modState = ModStates.Game;

                GameInfo gameInfo = Resources.FindObjectsOfTypeAll<GameInfo>().FirstOrDefault();
                if (gameInfo == null) {
                    MelonLogger.LogError("Unable to get game info - quitting!");
                    return; 
                }

                SongManager songManager = gameInfo._songManager;
                if (songManager == null) {
                    MelonLogger.LogError("Unable to get song manager - quitting!");
                    return; 
                }

                AlbumSongs_SO songData = songManager.CurrentSong;
                if (songData == null) { 
                    MelonLogger.LogError("Unable to get song data - quitting!");
                    return;
                }

                // Prepare the data we can output
                string songAuthor = songData.Author;
                string songName = songData.Name;
                string songDifficulty = songData.Levels[songManager.CurrentSongLevel].Difficulty.ToString();
                string songLength = TimeSpan.FromSeconds(songData.AudioTime).ToString(@"%m\:ss");
                string songAccuracyMultiplier = (1 / WallDanceUtils.CurrentPrecisionMultiplier).ToString("F1");
                string songSpeedMultiplier = WallDanceUtils.CurrentSpeedMultiplier.ToString("F1");
                string songObjectSpeed = songManager._objectsSpeed.ToString("F0");

                // Replace values in HTML template
                string htmlData = htmlSongInfo.Replace("%songauthor%", songAuthor);
                htmlData = htmlData.Replace("%songname%", songName);
                htmlData = htmlData.Replace("%difficulty%", songDifficulty);
                htmlData = htmlData.Replace("%accuracymultiplier%", songAccuracyMultiplier);
                htmlData = htmlData.Replace("%speedmultiplier%", songSpeedMultiplier);
                htmlData = htmlData.Replace("%objectspeed%", songObjectSpeed);

                // Replace values in TXT template
                string txtData = txtSongInfo.Replace("%songauthor%", songAuthor);
                txtData = txtData.Replace("%songname%", songName);
                txtData = txtData.Replace("%difficulty%", songDifficulty);
                txtData = txtData.Replace("%accuracymultiplier%", songAccuracyMultiplier);
                txtData = txtData.Replace("%speedmultiplier%", songSpeedMultiplier);
                txtData = txtData.Replace("%objectspeed%", songObjectSpeed);

                // Output data to disk
                FileHandler.WriteSongInfo(htmlData, FileHandler.FileTemplateType.HTML);
                FileHandler.WriteSongInfo(txtData, FileHandler.FileTemplateType.TXT);
            }
        }

        public override void OnApplicationQuit()
        {
            MelonLogger.Log("OnApplicationQuit called.");
        }

        private void Load()
        {
            modState = ModStates.Menu;
            FileHandler.InitializeFiles();
            FileHandler.LoadTemplates();
            FileHandler.WriteSongInfo(htmlEmptyInfo, FileHandler.FileTemplateType.HTML);
            FileHandler.WriteSongInfo(txtEmptyInfo, FileHandler.FileTemplateType.TXT);
        }
    }
}
