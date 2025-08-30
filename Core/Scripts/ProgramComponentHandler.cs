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
            public List<ElementPlaceableSetError> ElementPlaceableSetErrors { set; get; } = new();
            public List<ProgramComponentError> ProgramComponentErrors { set; get; } = new();

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

            public async Task<string> FirstMessage()
            {
                if (ElementPlaceableSetErrors.Count > 0)
                {
                    return await ElementPlaceableSetErrors.First().Condition.falseMessage.GetLocalizedStringAsync().Task;
                }
                else if (ProgramComponentErrors.Count > 0)
                {
                    return await ProgramComponentErrors.First().Condition.falseMessage.GetLocalizedStringAsync().Task;
                }
                else
                {
                    // エラーなし
                    return "";
                }
            }
        }

        public Program Program { get; private set; }
        public ProgramComponent[] ProgramComponents { get; private set; }
        public ProgramComponentRegulation[] ProgramComponentRegulationAll { get; private set; }
        public ElementPlaceableSet[] ElementPlaceableSetAll { get; private set; }
        public ElementPlaceable[] ElementPlaceableAll { get; private set; }
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
            ElementPlaceableSetAll = elementPlaceableSetAll;
            ElementPlaceableAll = elementPlaceableAll;
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
            var elementPlaceableSet = Array.Find(ElementPlaceableSetAll, x => x.id.Equals(ProgramComponents[componentIndex].elementPlaceableSetId)) ?? throw new Exception($"Not found '{ProgramComponents[componentIndex].elementPlaceableSetId}'");
            var elementPlaceable = Array.Find(ElementPlaceableAll, x => x.id.Equals(elementPlaceableSet.elementPlaceableIds[elementIndex])) ?? throw new Exception($"Not found '{elementPlaceableSet.elementPlaceableIds[elementIndex]}'");
            return elementPlaceable.elementIds.Any(id => id == elementId);
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
            var programComponentSetConditions = ProgramComponents
                .Select((component, componentIndex) => new
                {
                    Component = component,
                    ComponentIndex = componentIndex,
                    ElementPlaceableSet = Array.Find(ElementPlaceableSetAll, x => x.id.Equals(component.elementPlaceableSetId))
                })
                .Where(x => x.ElementPlaceableSet.Conditions.Count > 0)
                .SelectMany(x => x.ElementPlaceableSet.Conditions
                    .Where(condition => !condition.Condition(x.Component.elementIds.Where(id => !string.IsNullOrEmpty(id)).ToArray())),
                    (x, condition) => (Condition: condition, x.ComponentIndex)
                )
                .ToList();
            if (programComponentSetConditions.Count > 0)
            {
                errorOccurred = true;
                Error ??= new();
                Error.ElementPlaceableSetErrors = programComponentSetConditions
                    .Select(x => new ErrorData.ElementPlaceableSetError() { Condition = x.Condition, ComponentIndex = x.ComponentIndex })
                    .ToList();
            }

            // 構成全体をみて配置可能条件を満たしていないものがあればそのエラーを設定
            var regulation = ProgramUtility.GetProgramComponentRegulationById(ProgramComponentRegulationAll, Program.programComponentRegulationId);
            var programComponentConditions = regulation.Conditions.Where(condition => !condition.Condition(ProgramComponents)).ToList();
            if (programComponentConditions.Count > 0)
            {
                errorOccurred = true;
                Error ??= new();
                Error.ProgramComponentErrors = programComponentConditions
                    .Select(x => new ErrorData.ProgramComponentError() { Condition = x, ComponentIndexList = x.falseComponentIndexList })
                    .ToList();
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