#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Wlg.FigureSkate.Fact.Editor
{
    public class LocalizeHealthChecker
    {
        public LocalizeHealthChecker(string searchRootPath)
        {
            _searchRootPath = searchRootPath;
            SetThisFileName();
        }

        public async Task Run(
            List<string> stringTableNameList,
            List<(string DirPath, string FileExt)> searchFileParamList
            )
        {
            // ローカライズデータのキーを収集
            var keyCollectTasks = stringTableNameList.Select(name => LocalizationSettings.StringDatabase.GetTableAsync(name).Task);
            await Task.WhenAll(keyCollectTasks);
            var allKeyIds = keyCollectTasks
                .SelectMany(task => task.Result.ToDictionary(pair => pair.Value.Key, pair => pair.Key))
                .ToList();

            // 検索単語をKey/Idのペアで検索できるように構築
            var searchTermList = allKeyIds
                .Select(pair =>
                {
                    string escapedKey = Regex.Escape(pair.Key);
                    return $@"{escapedKey}|{pair.Value}";
                })
                .OrderByDescending(x => x.Length)
                .ToList();

            // ファイルをKey/Idのペアで一行ずつ検索
            var searchTermsInFilesTasks = searchFileParamList.Select(param => SearchKeyInFiles(
                targetPath: Path.Combine(_searchRootPath, param.DirPath),
                targetFileExt: param.FileExt,
                searchTermList: searchTermList
                ))
                .ToList();
            var allSearchResultsArrays = await Task.WhenAll(searchTermsInFilesTasks);
            var allSearchResults = allSearchResultsArrays
                .SelectMany(resultsArray => resultsArray)
                .ToList();

            // 全キーで全検索結果を検索して未使用キーを判別
            _unusedList = searchTermList.Where(searchTerm => !allSearchResults.Any(result => result.searchTerm == searchTerm)).ToList();

            // 全検索結果で全キーを検索して使用しているがキーが存在していないものを判別
            _missingList = allSearchResults.Where(result => !searchTermList.Any(searchTerm => result.searchTerm == searchTerm)).ToList();
        }

        public void Dump()
        {
            DumpFindUnusedKeyLog();
            DumpFindMissingKeyLog();
        }

        public void DumpFindUnusedKeyLog()
        {
            var results = FindUnusedKey();
            var log = results.Count > 0 ? string.Join("\n", results) : "Not found";
            Debug.Log($"[FindUnusedKey]\n{log}");
        }
        public void DumpFindMissingKeyLog()
        {
            var results = FindMissingKey();
            var log = results.Count > 0 ? string.Join("\n", results) : "Not found";
            Debug.Log($"[FindMissingKey]\n{log}");
        }

        private List<string> FindUnusedKey()
        {
            return _unusedList;
        }

        private List<string> FindMissingKey()
        {
            return _missingList.Select(x => x.Dump).ToList();
        }

        private async Task<List<SearchResult>> SearchKeyInFiles(string targetPath, string targetFileExt, List<string> searchTermList)
        {
            return await Task.Run(async () =>
            {
                var searchResults = new List<SearchResult>();

                // 使用しているが未定義(missing)を見つけるためにキーのルールで検索語彙を追加
                var keyRulePattern = @"«(?:.*?)»";
                var allSearchTermList = searchTermList.Concat(new[] { keyRulePattern }).ToList();

                // これまでの検索語彙すべてを検索できるような正規表現パターンを構築
                var pattern = new Regex(string.Join("|", allSearchTermList.Select(term => $"({term})")));

                // 指定パスの指定拡張子のファイルを1行単位で正規表現パターンマッチング
                var targetFilePaths = Directory.GetFiles(targetPath, $"*.{targetFileExt}", SearchOption.AllDirectories);
                var readAllLinesTasks = targetFilePaths.Select(path => File.ReadAllLinesAsync(path)).ToList();
                await Task.WhenAll(readAllLinesTasks);
                for (int i = 0; i < readAllLinesTasks.Count; i++)
                {
                    string filePath = targetFilePaths[i];
                    string[] lines = readAllLinesTasks[i].Result;

                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        string line = lines[lineIndex];

                        // asset内にマルチバイトや特殊文字が含まれる場合はエスケープされる。それをもとに戻す
                        string normalizedLine = line.Replace(@"\xAB", "«").Replace(@"\xBB", "»");

                        // 正規表現による検索
                        var matches = pattern.Matches(normalizedLine);
                        foreach (Match match in matches)
                        {
                            if (!match.Success) continue;
                            for (int groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
                            {
                                if (match.Groups[groupIndex].Success)
                                {
                                    string foundSearchTerm = allSearchTermList[groupIndex - 1];

                                    // このファイル自身はスキップ
                                    if (foundSearchTerm == keyRulePattern && Path.GetFileName(filePath) == _thisFileName)
                                    {
                                        continue;
                                    }

                                    // 結果を記録して次の検索へ
                                    var result = new SearchResult
                                    {
                                        searchTerm = foundSearchTerm,
                                        match = match.Value,
                                        filePath = Path.GetRelativePath(_searchRootPath, filePath),
                                        lineNumber = lineIndex + 1, // 行番号は1から
                                        lineContent = line
                                    };
                                    searchResults.Add(result);
                                    break;
                                }
                            }
                        }
                    }
                }
                return searchResults;
            });
        }

        /// <summary>
        /// [CallerFilePath]を利用して、このクラスのファイル名を設定するヘルパーメソッド。
        /// </summary>
        private void SetThisFileName([CallerFilePath] string sourceFilePath = "")
        {
            _thisFileName = Path.GetFileName(sourceFilePath);
        }

        private class SearchResult
        {
            public string searchTerm;
            public string match;
            public string filePath; // Project相対パス
            public int lineNumber;
            public string lineContent;

            public string Dump => $"{match} ({filePath} L:{lineNumber} {lineContent.Trim()})";
        }

        private readonly string _searchRootPath;
        private string _thisFileName = null;
        private List<string> _unusedList = null;
        private List<SearchResult> _missingList = null;
    }
}
#endif