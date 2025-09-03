using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Wlg.FigureSkate.Fact
{
    public static class LoaderUtility
    {
        // 指定ディレクトリ以下のアセットファイルを非同期全読み込み
        public static async Task<List<T>> LoadAssetsAsync<T>(string path) where T : UnityEngine.Object
        {
            // MEMO:"filelist.txt" は ScriptableObjectFactory.OnProjectLoadedInEditor のものと一致させる必要がある
            // MEMO:ここはファイルパスではなくAddressablesのキーを指すのでPath.Combineは使わない
            var filelistKey = path + "/filelist.txt";

            // ファイル一覧情報からアセットのパスを取得
            // なければ何も読み込めないまま終了
            var filelistObj = await Addressables.LoadAssetAsync<TextAsset>(filelistKey).Task;
            if (filelistObj == null) return null;
            var assets = filelistObj.text
                .Split("\n") // 一行単位でリスト化
                .Where(x => !string.IsNullOrEmpty(x)); // 空白行を無視
            if (assets.Count() == 0) return null;

            // パス情報からアセット読み込み
            var taskList = new List<Task<T>>(assets.Count());
            foreach (var asset in assets)
            {
                taskList.Add(Addressables.LoadAssetAsync<T>(asset).Task);
            }
            await Task.WhenAll(taskList);
            return taskList.Select(x => x.Result).ToList();
        }
    }
}
