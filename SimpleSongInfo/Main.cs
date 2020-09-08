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
        public const string Version = "1.1.0";                  // Version of the Mod.  (MUST BE SET)
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
        internal static GameSceneController _gscInstance;

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
                if (_gscInstance == null)
                {
                    _gscInstance = new GameObject(BuildInfo.ShortName).AddComponent<GameSceneController>();
                }
                
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
