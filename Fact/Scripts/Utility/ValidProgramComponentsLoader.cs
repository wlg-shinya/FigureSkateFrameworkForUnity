using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;

namespace Wlg.FigureSkate.Fact
{
    public class ValidProgramComponentsLoader
    {
        // インスタンス禁止
        private ValidProgramComponentsLoader() { }

        public ValidProgramComponents Root { get; private set; }
        public Dictionary<ushort, string> IdToStringMap { get; private set; }

        public const string VALID_PROGRAM_COMPONENTS_FILENAME = "ValidProgramComponents.bin.gz";
        public const string ID_MAP_FILENAME = "IdMap.json";

        public static async Task<ValidProgramComponentsLoader> Load(string dirPath)
        {
            var loader = new ValidProgramComponentsLoader();
            var mapJsonPath = Path.Combine(dirPath, ID_MAP_FILENAME);
            var binGzPath = Path.Combine(dirPath, VALID_PROGRAM_COMPONENTS_FILENAME);

            if (!File.Exists(mapJsonPath) || !File.Exists(binGzPath))
            {
                throw new FileNotFoundException("TrieデータファイルまたはIDマップファイルが見つかりません。", dirPath);
            }

            // --- Step 1: IDマップ(JSON)を非同期で読み込む ---
            var json = await File.ReadAllTextAsync(mapJsonPath);
            var serializableMap = JsonUtility.FromJson<SerializableUshortStringMap>(json);
            loader.IdToStringMap = new Dictionary<ushort, string>();
            for (int i = 0; i < serializableMap.keys.Count; i++)
            {
                loader.IdToStringMap[serializableMap.keys[i]] = serializableMap.values[i];
            }

            // --- Step 2: GZip圧縮されたTrieデータを非同期で読み込み、伸張する ---
            // まず非同期でファイルからメモリへ読み込み、その後メモリ上で高速にパースする
            using (var memoryStream = new MemoryStream())
            {
                // FileStreamを非同期モードで開く
                using (var fileStream = new FileStream(binGzPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                using (var decompressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    // 非同期で伸張し、結果をメモリストリームにコピーする
                    await decompressionStream.CopyToAsync(memoryStream);
                }

                // メモリストリームの先頭から読み込むために位置をリセット
                memoryStream.Position = 0;
                using (var binReader = new BinaryReader(memoryStream))
                {
                    // ReadNodeはメモリ上のデータを読むだけなので、同期のままで高速
                    loader.Root = loader.ReadNode(binReader);
                }
            }

            return loader;
        }

        [Serializable]
        public class SerializableUshortStringMap
        {
            public List<ushort> keys = new();
            public List<string> values = new();

            public SerializableUshortStringMap(Dictionary<ushort, string> dict)
            {
                foreach (var pair in dict)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
        }

        private ValidProgramComponents ReadNode(BinaryReader reader)
        {
            var node = new ValidProgramComponents
            {
                IsEndOfValid = reader.ReadBoolean()
            };
            if (node.IsEndOfValid)
            {
                node.TotalBaseValue = reader.ReadSingle();
            }

            int childCount = reader.ReadInt32();
            node.Children = new Dictionary<ushort, ValidProgramComponents>(childCount);

            for (int i = 0; i < childCount; i++)
            {
                ushort elementId = reader.ReadUInt16();
                var childNode = ReadNode(reader);
                node.Children.Add(elementId, childNode);
            }
            return node;
        }
    }
}