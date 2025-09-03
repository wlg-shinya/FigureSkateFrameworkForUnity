using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    public class ValidProgramComponentsLoader : IDisposable
    {
        private readonly FileStream _binStream;
        private readonly BinaryReader _binReader;
        private readonly List<long> _offsets = new();
        public int Count { get; private set; }

        public ValidProgramComponentsLoader(string basePath, string dataName)
        {
            var binPath = Path.Combine(basePath, $"{dataName}.bin");
            var idxPath = Path.Combine(basePath, $"{dataName}.idx");

            // ファイルの存在確認
            if (!File.Exists(binPath) || !File.Exists(idxPath))
            {
                throw new FileNotFoundException($"指定されたデータファイルまたはインデックスファイルが見つかりません: {dataName}");
            }

            // インデックスファイル(.idx)の読み込み
            byte[] idxBytes = File.ReadAllBytes(idxPath);
            using (var ms = new MemoryStream(idxBytes))
            using (var reader = new BinaryReader(ms))
            {
                // ファイルの終端に達するまで8バイト(long)ずつ読み込み、オフセットリストに追加
                while (ms.Position < ms.Length)
                {
                    _offsets.Add(reader.ReadInt64());
                }
            }

            // データファイル(.bin)を開き、ストリームを保持 
            _binStream = new FileStream(binPath, FileMode.Open, FileAccess.Read);
            _binReader = new BinaryReader(_binStream);

            // ファイルの先頭に書き込まれているデータ総数を読み込む
            Count = _binReader.ReadInt32();
        }

        public ValidProgramComponents Get(int index)
        {
            if (index < 0 || index >= _offsets.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "インデックスが範囲外です。");
            }

            // インデックスリストから目的のデータの開始位置(オフセット)を取得
            long offset = _offsets[index];

            // ストリームの位置を目的のデータの開始位置までジャンプさせる
            _binStream.Seek(offset, SeekOrigin.Begin);

            // 現在位置から1つ分のデータだけを読み込む
            var vpc = new ValidProgramComponents
            {
                programId = _binReader.ReadString(),
                totalBaseValue = _binReader.ReadSingle(),
                programComponents = new()
            };

            int componentsCount = _binReader.ReadInt32();
            var components = new ProgramComponent[componentsCount];
            for (int j = 0; j < componentsCount; j++)
            {
                var pc = new ProgramComponent { elementPlaceableSetId = _binReader.ReadString() };
                int elementIdsCount = _binReader.ReadInt32();
                var elementIds = new string[elementIdsCount];
                for (int k = 0; k < elementIdsCount; k++)
                {
                    elementIds[k] = _binReader.ReadString();
                }
                pc.elementIds = elementIds;
                components[j] = pc;
            }
            vpc.programComponents.components = components;

            return vpc;
        }

        public void Dispose()
        {
            _binReader?.Close();
            _binStream?.Close();
        }
    }
}