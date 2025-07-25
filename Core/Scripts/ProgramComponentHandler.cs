using System;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成操作クラス
    public class ProgramComponentHandler
    {
        public Program Program { get; private set; }
        public ProgramComponent[] ProgramComponents { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        public ProgramComponentHandler(
            Program program,
            ProgramComponent[] programComponent,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            Program = program;
            ProgramComponents = programComponent;
            _programComponentRegulationAll = programComponentRegulationAll;
            _elementPlaceableSetAll = elementPlaceableSetAll;
            _elementPlaceableAll = elementPlaceableAll;
            UpdateErrorMessage();
        }

        // セットトライ
        public bool TrySet(int componentIndex, int elementIndex, string elementId)
        {
            if (!CanSet(componentIndex, elementIndex, elementId))
            {
                return false;
            }
            ProgramComponents[componentIndex].elementIds[elementIndex] = elementId;
            UpdateErrorMessage();
            return true;
        }

        // ルールに沿ってセット可能か
        private bool CanSet(int componentIndex, int elementIndex, string elementId)
        {
            // インデックス範囲チェック
            CheckIndexOutOfRange(componentIndex, elementIndex);

            // 空文字かNULLは設定クリアとして受け付ける
            if (string.IsNullOrEmpty(elementId)) return true;

            // 指定された構成要素が設定可能一覧に含まれている場合は追加可。そうでなければ追加不可
            var elementPlaceableSet = Array.Find(_elementPlaceableSetAll, x => x.id.Equals(ProgramComponents[componentIndex].elementPlaceableSetId)) ?? throw new Exception($"Not found '{ProgramComponents[componentIndex].elementPlaceableSetId}'");
            var elementPlaceable = Array.Find(_elementPlaceableAll, x => x.id.Equals(elementPlaceableSet.elementPlaceableIds[elementIndex])) ?? throw new Exception($"Not found '{elementPlaceableSet.elementPlaceableIds[elementIndex]}'");
            return elementPlaceable.elementIds.Any(id => id == elementId);
        }

        // セット解除
        public void Unset(int componentIndex, int elementIndex)
        {
            CheckIndexOutOfRange(componentIndex, elementIndex);
            ProgramComponents[componentIndex].elementIds[elementIndex] = null;
            UpdateErrorMessage();
        }

        // セット全解除
        public void UnsetAll()
        {
            for (var i = 0; i < ProgramComponents.Length; i++)
            {
                for (var j = 0; j < ProgramComponents[i].elementIds.Length; j++)
                {
                    ProgramComponents[i].elementIds[j] = null;
                }
            }
            UpdateErrorMessage();
        }

        // 入れ替え
        public void Swap(int a, int b) => (ProgramComponents[a], ProgramComponents[b]) = (ProgramComponents[b], ProgramComponents[a]);

        // エラーメッセージの更新
        // 現在の構成に不正があったらその原因となるエラーメッセージが、正常なら空文字が入る
        public void UpdateErrorMessage()
        {
            // 構成ごとの配置可能条件を満たしていないものがあればそのエラーメッセージを設定
            var programComponentSetCondition = ProgramComponents
                .Select(component => (component, elementPlaceableSet: Array.Find(_elementPlaceableSetAll, x => x.id.Equals(component.elementPlaceableSetId))))
                .Where(x => x.elementPlaceableSet.Conditions.Count > 0)
                .Select(x => x.elementPlaceableSet.Conditions.Find(condition => !condition.Condition(x.component.elementIds.Where(id => id != null).ToArray())))
                .FirstOrDefault();
            if (programComponentSetCondition != null)
            {
                ErrorMessage = programComponentSetCondition.falseMessage;
                return;
            }
            // 上記が見つからなければ構成全体をみて配置可能条件を満たしていないものがあればそのエラーメッセージを設定
            else
            {
                var regulation = ProgramUtility.GetProgramComponentRegulationById(_programComponentRegulationAll, Program.programComponentRegulationId);
                var programComponentCondition = regulation.Conditions.Find(condition => !condition.Condition(ProgramComponents));
                if (programComponentCondition != null)
                {
                    ErrorMessage = programComponentCondition.falseMessage;
                    return;
                }
            }
            // エラーが見つからなかったのでエラーメッセージなし
            ErrorMessage = "";
        }

        private void CheckIndexOutOfRange(int componentIndex, int elementIndex)
        {
            var componentIndexMax = ProgramComponents.Length - 1;
            if (0 > componentIndex || componentIndex > componentIndexMax)
            {
                throw new ArgumentOutOfRangeException($"componentIndex = {componentIndex}");
            }
            var elementIndexMax = ProgramComponents[componentIndex].elementIds.Length - 1;
            if (0 > elementIndex || elementIndex > elementIndexMax)
            {
                throw new ArgumentOutOfRangeException($"elementIndex = {elementIndex}");
            }
        }

        private readonly ProgramComponentRegulation[] _programComponentRegulationAll;
        private readonly ElementPlaceableSet[] _elementPlaceableSetAll;
        private readonly ElementPlaceable[] _elementPlaceableAll;

        // 内部で使う一時変数の事前確保。要素数は最大コンビネーション数と同値
        // private readonly string[] _placedElementIdsBuffer = new string[CoreConstant.ELEMENT_IN_COMBINATION_MAX_COUNT];
    }
}