#if UNITY_EDITOR && WLG_DEVELOP
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Wlg.FigureSkate.Fact.Editor
{
    public static class Menu
    {
        [MenuItem("Wlg.FigureSkate/LocalizeHealthCheck")]
        public static async void LocalizeHealthCheck()
        {
            try
            {
                Debug.Log($"LocalizeHealthCheck start. After a while, the log will be output.");

                var packageInfo = GetPackageInfo();
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
                Debug.Log($"LocalizeHealthCheck finished.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [MenuItem("Wlg.FigureSkate/ValidProgramComponentsBuilder/Full build")]
        public static async void ValidProgramComponentsBuilderFullBuild()
        {
            try
            {
                Debug.Log($"ValidProgramComponentsBuilder start. After a while, the log will be output.");

                var packageInfo = GetPackageInfo();
                _validProgramComponentsBuilder ??= new ValidProgramComponentsBuilder(Path.Combine(packageInfo.resolvedPath, "Fact", "ValidProgramComponents"));
                await _validProgramComponentsBuilder.FullBuild();
                // await builder.BuildOneProgram("2022-23", "SeniorMenShortProgram");
                Debug.Log($"ValidProgramComponentsBuilder finished.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [MenuItem("Wlg.FigureSkate/ValidProgramComponentsBuilder/2022-23 SeniorMenShortProgram")]
        public static async void ValidProgramComponentsBuilder2022_23SeniorMenShortProgram()
        {
            try
            {
                Debug.Log($"ValidProgramComponentsBuilder start. After a while, the log will be output.");

                var packageInfo = GetPackageInfo();
                _validProgramComponentsBuilder ??= new ValidProgramComponentsBuilder(Path.Combine(packageInfo.resolvedPath, "Fact", "ValidProgramComponents"));
                await _validProgramComponentsBuilder.BuildOneProgram("2022-23", "SeniorMenShortProgram");
                Debug.Log($"ValidProgramComponentsBuilder finished.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static UnityEditor.PackageManager.PackageInfo GetPackageInfo()
        {
            return UnityEditor.PackageManager.PackageInfo.FindForAssetPath("Packages/com.welovegamesinc.figureskate-framework") ??
                throw new Exception("Not found path 'com.welovegamesinc.figureskate-framework'");
        }

        private static ValidProgramComponentsBuilder _validProgramComponentsBuilder = null;
    }
}
#endif