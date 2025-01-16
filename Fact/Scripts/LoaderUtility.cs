using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Wlg.FigureSkate.Fact
{
    public static class LoaderUtility
    {
        // 指定ディレクトリ以下のアセットファイルを非同期全読み込み
        public static async Task<List<T>> LoadAssetsAsync<T>(string path)
        {
            var files = Directory.GetFiles(path);
            var assets = files
                .Where(x => Path.GetExtension(x) == ".asset")
                .Select(x => x.Replace("\\", "/")); // GetFiles で得たパスには \ が混在するので / に置き換え
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
