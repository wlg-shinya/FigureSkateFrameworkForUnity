using System;
using System.Linq;
using System.Text.RegularExpressions;
using PlasticPipe.PlasticProtocol.Messages;

namespace Wlg.FigureSkate.Core.Data
{
    // 指定パターン構成要素の中で同じ要素を指定数以内の構成で抑えているかを複数の条件の重ね合わせで判断する
    [Serializable]
    public class ProgramComponentConditionWithinInPatternMultipleConditions : ProgramComponentCondition
    {
        [Serializable]
        public class Data
        {
            // このデータが無効か
            public bool disable;
            // グルーピングする構成要素の指定パターン
            public string groupPattern;
            // 同じ構成要素の最大同時構成数
            public int maxPlacedCount;
            // 同じ構成要素の最大同時構成の許容数
            public int maxPlacedAllowCount;

            // データ単体の条件判定
            public bool Condition(ProgramComponent[] components)
            {
                // 指定パターンの構成要素のリスト作成
                var targetPlacedElementIds = components
                    .SelectMany(arg => arg.elementIds)
                    .Where(elementId => !string.IsNullOrEmpty(elementId))
                    .Where(elementId => Regex.IsMatch(elementId, groupPattern));
                var uniquePlacedElementIds = targetPlacedElementIds.Distinct();

                // 構成要素ごとに同じ構成要素が何回設定されているかのリスト作成
                var countList = uniquePlacedElementIds.Select(elementId => targetPlacedElementIds.Count(elementId2 => Equals(elementId, elementId2)));
                if (countList.Any(x => x > maxPlacedCount))
                {
                    // 最大同時構成数を超えている構成要素があるので不正なデータ
                    return false;
                }
                else if (countList.Count(x => x == maxPlacedCount) > maxPlacedAllowCount)
                {
                    // 構成要素の最大同時構成数の許容数を超えているので不正なデータ
                    return false;
                }
                else
                {
                    // すべてのチェックが通ったので正しいデータ
                    return true;
                }
            }
        }
        [Serializable]
        public class ConditionData
        {
            public Data current;
            public Data ifTrue;
            public Data ifFalse;
            public bool Condition(ProgramComponent[] components)
            {
                if (!current.disable && current.Condition(components))
                {
                    return ifTrue.disable || ifTrue.Condition(components);
                }
                else
                {
                    return !ifFalse.disable && ifFalse.Condition(components);
                }
            }

        }
        public ConditionData[] conditionDatas;

        public override bool Condition(ProgramComponent[] components)
        {
            return conditionDatas.All(x => x.Condition(components));
        }
    }
}