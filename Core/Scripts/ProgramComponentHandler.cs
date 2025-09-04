using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成操作クラス
    public class ProgramComponentHandler
    {
        public class ErrorData
        {
            public List<ElementPlaceableSetError> ElementPlaceableSetErrors { set; get; }
            public List<ProgramComponentError> ProgramComponentErrors { set; get; }

            public class ElementPlaceableSetError
            {
                public ElementPlaceableSetCondition Condition;
                public int ComponentIndex { set; get; }
            }

            public class ProgramComponentError
            {
                public ProgramComponentCondition Condition;
                public List<int> ComponentIndexList { set; get; }
            }
        }

        public Program Program { get; private set; }
        public ProgramComponent[] ProgramComponents { get; private set; }
        public ProgramComponentRegulation[] ProgramComponentRegulationAll { get; private set; }
        public Dictionary<string, ElementPlaceableSet> ElementPlaceableSetMap { get; private set; }
        public Dictionary<string, ElementPlaceable> ElementPlaceableMap { get; private set; }
        public ErrorData Error { get; private set; } = new(); // nullなら正常。初期時はエラーとするためインスタンス化

        public void Initialize(
            Program program,
            ProgramComponent[] programComponent,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            Program = program;
            ProgramComponents = programComponent;
            ProgramComponentRegulationAll = programComponentRegulationAll;
            ElementPlaceableSetMap = elementPlaceableSetAll.ToDictionary(x => x.id, x => x);
            ElementPlaceableMap = elementPlaceableAll.ToDictionary(x => x.id, x => x); ;
            UpdateError();
        }

        // セットトライ
        public bool TrySet(int componentIndex, int elementIndex, string elementId)
        {
            if (!CanSet(componentIndex, elementIndex, elementId))
            {
                return false;
            }
            ProgramComponents[componentIndex].elementIds[elementIndex] = elementId;
            UpdateError();
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
            if (ElementPlaceableSetMap.TryGetValue(ProgramComponents[componentIndex].elementPlaceableSetId, out var elementPlaceableSet))
            {
                if (ElementPlaceableMap.TryGetValue(elementPlaceableSet.elementPlaceableIds[elementIndex], out var elementPlaceable))
                {
                    return elementPlaceable.elementIds.Any(id => id == elementId);
                }
            }
            return false;
        }

        // セット解除
        public void Unset(int componentIndex, int elementIndex)
        {
            CheckIndexOutOfRange(componentIndex, elementIndex);
            ProgramComponents[componentIndex].elementIds[elementIndex] = null;
            UpdateError();
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
            UpdateError();
        }

        // 入れ替え
        public void Swap(int a, int b) => (ProgramComponents[a], ProgramComponents[b]) = (ProgramComponents[b], ProgramComponents[a]);

        // エラー発生状況の更新
        public void UpdateError()
        {
            bool errorOccurred = false;
            // 構成ごとの配置可能条件を満たしていないものがあればそのエラーを設定
            var elementPlaceableSetConditions = ProgramComponents
                .Select((component, componentIndex) => new
                {
                    Component = component,
                    ComponentIndex = componentIndex,
                    ElementPlaceableSet = ElementPlaceableSetMap[component.elementPlaceableSetId]
                })
                .Where(x => x.ElementPlaceableSet.Conditions.Count > 0)
                .SelectMany(x => x.ElementPlaceableSet.Conditions
                    .Where(condition => !condition.Condition(x.Component.elementIds.Where(id => !string.IsNullOrEmpty(id)).ToArray())),
                    (x, condition) => (Condition: condition, x.ComponentIndex)
                )
                .ToList();
            if (elementPlaceableSetConditions.Count > 0)
            {
                errorOccurred = true;
                Error ??= new();
                Error.ElementPlaceableSetErrors = elementPlaceableSetConditions
                    .Select(x => new ErrorData.ElementPlaceableSetError() { Condition = x.Condition, ComponentIndex = x.ComponentIndex })
                    .ToList();
            }
            else
            {
                // エラーなし
                if (Error != null) Error.ElementPlaceableSetErrors = new();
            }

            // 構成全体をみて配置可能条件を満たしていないものがあればそのエラーを設定
            var regulation = ProgramUtility.GetProgramComponentRegulationById(ProgramComponentRegulationAll, Program.programComponentRegulationId);
            var programComponentConditions = regulation.Conditions
                .Select(condition => new { Condition = condition, IsMet = condition.Condition(ProgramComponents, out var indices), FalseIndices = indices })
                .Where(x => !x.IsMet)
                .ToList();
            if (programComponentConditions.Count > 0)
            {
                errorOccurred = true;
                Error ??= new();
                Error.ProgramComponentErrors = programComponentConditions
                    .Select(x => new ErrorData.ProgramComponentError() { Condition = x.Condition, ComponentIndexList = x.FalseIndices }) // outで受け取ったリストを使用
                    .ToList();
            }
            else
            {
                // エラーなし
                if (Error != null) Error.ProgramComponentErrors = null;
            }

            // エラーが発生していないのでエラーなし
            if (!errorOccurred)
            {
                Error = null;
            }
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
    }
}