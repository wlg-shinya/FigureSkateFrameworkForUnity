using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Wlg.FigureSkate.Fact
{
    public class ValidProgramComponentsLoader
    {
        public ValidProgramComponents Root { get; private set; }
        public Dictionary<ushort, string> IdToStringMap { get; private set; }

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

        public ValidProgramComponentsLoader(string dirPath)
        {
            var mapJsonPath = Path.Combine(dirPath, "IdMap.json");
            var binGzPath = Path.Combine(dirPath, "ValidProgramComponents.bin.gz");

            if (!File.Exists(mapJsonPath) || !File.Exists(binGzPath))
            {
                throw new FileNotFoundException("TrieデータファイルまたはIDマップファイルが見つかりません。", dirPath);
            }

            // --- Step 1: IDマップ(JSON)を読み込む ---
            var json = File.ReadAllText(mapJsonPath);
            var serializableMap = JsonUtility.FromJson<SerializableUshortStringMap>(json);
            IdToStringMap = new Dictionary<ushort, string>();
            for (int i = 0; i < serializableMap.keys.Count; i++)
            {
                IdToStringMap[serializableMap.keys[i]] = serializableMap.values[i];
            }

            // --- Step 2: GZip圧縮されたTrieデータを読み込み、伸張する ---
            using (var fileStream = new FileStream(binGzPath, FileMode.Open))
            using (var decompressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var binReader = new BinaryReader(decompressionStream))
            {
                // 伸張ストリームから直接Trieを読み込む
                Root = ReadNode(binReader);
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