using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(SimpleSongInfo.BuildInfo.Description)]
[assembly: AssemblyDescription(SimpleSongInfo.BuildInfo.Description)]
[assembly: AssemblyCompany(SimpleSongInfo.BuildInfo.Company)]
[assembly: AssemblyProduct(SimpleSongInfo.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + SimpleSongInfo.BuildInfo.Author)]
[assembly: AssemblyTrademark(SimpleSongInfo.BuildInfo.Company)]
[assembly: AssemblyVersion(SimpleSongInfo.BuildInfo.Version)]
[assembly: AssemblyFileVersion(SimpleSongInfo.BuildInfo.Version)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCulture("")]

[assembly: MelonInfo(typeof(SimpleSongInfo.Mod), SimpleSongInfo.BuildInfo.Name, SimpleSongInfo.BuildInfo.Version, SimpleSongInfo.BuildInfo.Author, SimpleSongInfo.BuildInfo.DownloadLink)]
[assembly: MelonGame("Odders", "OhShape")]

[assembly: ComVisible(false)]
[assembly: Guid("61370266-fdb5-4ae9-a8bf-355ac8dd45db")]

