using System;
using System.Collections.Generic;
using System.IO;

namespace Wlg.FigureSkate.Fact
{
    public class ProgramComponentsTrieNodeLoader
    {
        public ProgramComponentsTrieNode Root { get; private set; }

        public ProgramComponentsTrieNodeLoader(string binPath)
        {
            // ファイルの存在確認
            if (!File.Exists(binPath))
            {
                throw new FileNotFoundException("Trieデータファイルが見つかりません。", binPath);
            }

            // ファイルストリームを開き、Trie木を完全にメモリに読み込む
            using (var binStream = new FileStream(binPath, FileMode.Open, FileAccess.Read))
            using (var binReader = new BinaryReader(binStream))
            {
                // ルートノードから再帰的に読み込みを開始
                Root = ReadNode(binReader);
            }
        }

        private ProgramComponentsTrieNode ReadNode(BinaryReader reader)
        {
            var node = new ProgramComponentsTrieNode
            {
                // 1. 有効な構成の終点であるかどうかのフラグを読み込む
                IsEndOfValid = reader.ReadBoolean()
            };
            if (node.IsEndOfValid)
            {
                // 2. 終点の場合、合計基礎点を読み込む
                node.TotalBaseValue = reader.ReadSingle();
            }

            // 3. 子ノードの数を読み込む
            int childCount = reader.ReadInt32();
            node.Children = new Dictionary<string, ProgramComponentsTrieNode>(childCount);

            // 4. 子ノードの数だけループし、再帰的に子ノードを読み込む
            for (int i = 0; i < childCount; i++)
            {
                // 分岐のキーとなる要素IDを読み込む
                string elementId = reader.ReadString();
                // 子ノードを再帰的に読み込む
                var childNode = ReadNode(reader);
                // 読み込んだ子ノードを辞書に追加する
                node.Children.Add(elementId, childNode);
            }

            return node;
        }
    }
}