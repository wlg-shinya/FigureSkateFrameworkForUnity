#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Wlg.FigureSkate.Editor
{
    public class LocalizeHealthChecker
    {
        public LocalizeHealthChecker(string searchRootPath)
        {
            _searchRootPath = searchRootPath;
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
            var searchTermList = allKeyIds.Select(pair => $@"{pair.Key}|Id\({pair.Value}\)").ToList();

            // Core以下のcsファイルから全キーを検索
            var searchTermsInFilesTasks = searchFileParamList.Select(param => SearchTermsInFiles(
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
            // _missingList = allSearchResults.Where(result => !searchTermList.Any(searchTerm => result.searchTerm == searchTerm)).ToList();
        }

        public void Dump()
        {
            DumpFindUnusedKeyLog();
            // DumpFindMissingKeyLog();
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

        private async Task<List<SearchResult>> SearchTermsInFiles(string targetPath, string targetFileExt, List<string> searchTermList)
        {
            return await Task.Run(async () =>
            {
                var searchResults = new List<SearchResult>();
                var pattern = new Regex(string.Join("|", searchTermList.Select(term => $"({term})")));
                string[] targetFilePaths = Directory.GetFiles(targetPath, $"*.{targetFileExt}", SearchOption.AllDirectories);
                var readAllLinesTasks = targetFilePaths.Select(path => File.ReadAllLinesAsync(path)).ToList();
                await Task.WhenAll(readAllLinesTasks);
                for (int i = 0; i < readAllLinesTasks.Count; i++)
                {
                    string filePath = targetFilePaths[i];
                    string[] lines = readAllLinesTasks[i].Result;

                    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                    {
                        string line = lines[lineIndex];

                        // 1行に対して、生成した巨大なパターンで全てのマッチを一度に検索する
                        var matches = pattern.Matches(line);

                        foreach (Match match in matches)
                        {
                            if (!match.Success) continue;

                            // マッチしたグループを特定し、どの検索単語にヒットしたかを見つける
                            // Groups[0]は全体のマッチなので、[1]から調べる
                            for (int groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
                            {
                                if (match.Groups[groupIndex].Success)
                                {
                                    // グループのインデックス(1-based)から元の検索単語リストのインデックス(0-based)を特定
                                    string foundSearchTerm = searchTermList[groupIndex - 1];

                                    var result = new SearchResult
                                    {
                                        searchTerm = foundSearchTerm,
                                        filePath = Path.GetRelativePath(_searchRootPath, filePath),
                                        lineNumber = lineIndex + 1, // 行番号は1から
                                        lineContent = line
                                    };
                                    searchResults.Add(result);

                                    // 1つのマッチに対して成功するグループは1つだけなので、
                                    // 見つかったら次のマッチの検索に移る
                                    break;
                                }
                            }
                        }
                    }
                }
                return searchResults;
            });
        }

        private class SearchResult
        {
            public string searchTerm;
            public string filePath; // Project相対パス
            public int lineNumber;
            public string lineContent;

            public string Dump => $"{searchTerm} ({filePath} L:{lineNumber} {lineContent.Trim()})";
        }

        private readonly string _searchRootPath;
        private List<string> _unusedList = null;
        private List<SearchResult> _missingList = null;
    }
}
#endif