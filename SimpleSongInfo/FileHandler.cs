using MelonLoader;
using System;
using System.IO;

namespace SimpleSongInfo
{
    internal static class FileHandler
    {
        internal static readonly string dataPath = Path.Combine(MelonUtils.UserDataDirectory, BuildInfo.ShortName);

        internal static readonly string htmlFilePath = Path.Combine(dataPath, "simplesonginfo.html");
        internal static readonly string txtFilePath = Path.Combine(dataPath, "simplesonginfo.txt");

        internal static readonly string tplHtmlEmptyFilePath = Path.Combine(dataPath, "simplesonginfo-template-empty.html");
        internal static readonly string tplHtmlInfoFilePath = Path.Combine(dataPath, "simplesonginfo-template-song.html");
        internal static readonly string tplTxtEmptyFilePath = Path.Combine(dataPath, "simplesonginfo-template-empty.txt");
        internal static readonly string tplTxtInfoFilePath = Path.Combine(dataPath, "simplesonginfo-template-song.txt");

        public enum FileTemplateType { HTML, TXT };


        // Checks for existing template files - if those don't exist, create sample templates shipped with this mod
        internal static void InitializeFiles()
        {
            // Check for mod data path, try to create it doesn't exist
            if (!Directory.Exists(dataPath))
            {
                try
                {
                    MelonLogger.Msg("Unable to find mod data directory, trying to create path '" + dataPath + "' ...");
                    Directory.CreateDirectory(dataPath);
                    MelonLogger.Msg("successful!");
                }
                catch (Exception ex)
                {
                    MelonLogger.Error(ex.ToString());
                    MelonLogger.Error("Unable to create mod data patch! Skipping file initialization.");
                    return;
                }
            }

            // Check if template files already exist, try to create them if they don't exist
            if (!File.Exists(tplHtmlEmptyFilePath))
            {
                CopyFileFromResources(Properties.Resources.simplesonginfo_template_empty_html, tplHtmlEmptyFilePath);
            }
            if (!File.Exists(tplHtmlInfoFilePath))
            {
                CopyFileFromResources(Properties.Resources.simplesonginfo_template_song_html, tplHtmlInfoFilePath);
            }
            if (!File.Exists(tplTxtEmptyFilePath))
            {
                CopyFileFromResources(Properties.Resources.simplesonginfo_template_empty_txt, tplTxtEmptyFilePath);
            }
            if (!File.Exists(tplTxtInfoFilePath))
            {
                CopyFileFromResources(Properties.Resources.simplesonginfo_template_song_txt, tplTxtInfoFilePath);
            }
        }


        // Try to load template files from disk - if this fails, load internal templates which are shipped with this mod
        internal static void LoadTemplates()
        {
            try
            {
                Mod.htmlEmptyInfo = File.ReadAllText(tplHtmlEmptyFilePath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
                MelonLogger.Error("Unable to load empty HTML template file from disk. Using internal default instead.");
                Mod.htmlEmptyInfo = Properties.Resources.simplesonginfo_template_empty_html;
            }

            try
            {
                Mod.htmlSongInfo = File.ReadAllText(tplHtmlInfoFilePath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
                MelonLogger.Error("Unable to load song info HTML template file from disk. Using internal default instead.");
                Mod.htmlSongInfo = Properties.Resources.simplesonginfo_template_song_html;
            }

            try
            {
                Mod.txtEmptyInfo = File.ReadAllText(tplTxtEmptyFilePath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
                MelonLogger.Error("Unable to load empty TXT template file from disk. Using internal default instead.");
                Mod.txtEmptyInfo = Properties.Resources.simplesonginfo_template_empty_txt;
            }

            try
            {
                Mod.txtSongInfo = File.ReadAllText(tplTxtInfoFilePath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
                MelonLogger.Error("Unable to load song info TXT template file from disk. Using internal default instead.");
                Mod.txtSongInfo = Properties.Resources.simplesonginfo_template_song_txt;
            }
        }


        // Writes song info to file
        internal static void WriteSongInfo(string data, FileTemplateType fileTemplateType)
        {
            try
            {
                if (fileTemplateType == FileTemplateType.HTML)
                {
                    File.WriteAllText(htmlFilePath, data);
                }
                else if (fileTemplateType == FileTemplateType.TXT)
                {
                    File.WriteAllText(txtFilePath, data);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());

                if (fileTemplateType == FileTemplateType.HTML)
                {
                    MelonLogger.Error("Unable to write to status file '" + htmlFilePath + "'");
                }
                else if (fileTemplateType == FileTemplateType.TXT)
                {
                    MelonLogger.Error("Unable to write to status file '" + txtFilePath + "'");
                }
            }
        }


        // Copy resource file to disk - with logging
        private static void CopyFileFromResources(string resource, string target)
        {
            try
            {
                MelonLogger.Msg("Trying to copy file from resources to '" + target + "' ...");
                File.WriteAllText(target, resource);
                MelonLogger.Msg("successful!");
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
                MelonLogger.Error("Unable to write file! Skipping.");
                return;
            }
        }
    }
}
