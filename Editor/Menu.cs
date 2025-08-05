#if UNITY_EDITOR && WLG_DEVELOP
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Wlg.FigureSkate.Editor
{
    public static class Menu
    {
        [UnityEditor.MenuItem("Wlg.FigureSkate/LocalizeHealthCheck (HEAVY)")]
        public static async Task LocalizeHealthCheck()
        {
            try
            {
                PackageInfo packageInfo = PackageInfo.FindForAssetPath("Packages/com.welovegamesinc.figureskate-framework") ??
                throw new Exception("Not found path 'com.welovegamesinc.figureskate-framework'");

                var stringTableNameList = new List<string>() {
                    "FigureSkateFrameworkCore",
                    "FigureSkateFrameworkFact",
                };
                var searchRootPath = packageInfo.resolvedPath;
                var searchFileParamList = new List<(string DirPath, string FileExt)>() {
                    (DirPath: "Core", FileExt: "cs"),
                    (DirPath: "Fact", FileExt: "cs"),
                    (DirPath: Path.Combine("Fact", "Objects"), FileExt: "asset"),
                    (DirPath: Path.Combine("Fact", "UI"), FileExt: "uxml"),
                };

                var healthChecker = new LocalizeHealthChecker(packageInfo.resolvedPath);
                await healthChecker.Run(stringTableNameList, searchFileParamList);
                healthChecker.Dump();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
#endif