using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成操作クラス
    public class ProgramComponentHanlder
    {
        public Program Program { get; private set; }
        public ProgramComponent[] ProgramComponents { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        public ProgramComponentHanlder(
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

            var elementPlaceableSet = Array.Find(_elementPlaceableSetAll, x => x.id.Equals(ProgramComponents[componentIndex].elementPlaceableSetId)) ?? throw new Exception($"Not found '{ProgramComponents[componentIndex].elementPlaceableSetId}'");
            var elementPlaceable = Array.Find(_elementPlaceableAll, x => x.id.Equals(elementPlaceableSet.elementPlaceableIds[elementIndex])) ?? throw new Exception($"Not found '{elementPlaceableSet.elementPlaceableIds[elementIndex]}'");

            // 今回の要素を追加したうえで一つでも設定可能条件を満たしていない場合は追加不可
            var elementIds = ProgramComponents[componentIndex].elementIds;
            for (var i = 0; i < _placedElementIdsBuffer.Length; i++) _placedElementIdsBuffer[i] = null;
            for (var i = 0; i < elementIds.Length; i++)
            {
                _placedElementIdsBuffer[i] = i == elementIndex ? elementId : elementIds[i];
            }
            if (elementPlaceableSet.Conditions.Count() > 0 && elementPlaceableSet.Conditions.Any(x => !x.Condition(_placedElementIdsBuffer.Where(x => x != null).ToArray())))
            {
                return false;
            }

            // 指定された構成要素が設定可能一覧に含まれている場合は追加可。そうでなければ追加不可
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
            // 構成全体をみて配置可能な要素としての条件を満たしていないものを探し出す
            var regulation = ProgramUtility.GetProgramComponentRegulationById(_programComponentRegulationAll, Program.programComponentRegulationId);
            var condition = regulation.Conditions.Find(condition => !condition.Condition(ProgramComponents));

            // エラーメッセージの更新
            ErrorMessage = condition != null ? condition.falseMessage : "";
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
        private readonly string[] _placedElementIdsBuffer = new string[Constant.ELEMENT_IN_COMBINATION_MAX_COUNT];
    }
}