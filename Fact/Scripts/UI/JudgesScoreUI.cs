using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    public class JudgesScoreUI
    {
        public TemplateContainer Root { get; private set; }

        // 初期化
        // UIにデータを設定するため初期化タイミングで1回呼び出す必要があります
        public void Initialize(VisualTreeAsset judgesScoreUxml, VisualTreeAsset judgesScoreExecutedElementItemUxml, JudgesScore data)
        {
            // 渡されたベースUIのセットアップ
            {
                Root = InstantiateVisualTree(judgesScoreUxml);
                _itemAsset = judgesScoreExecutedElementItemUxml;
            }

            // ヘッダー部
            {
                Header().Q<Label>("Rank").text = data.rank.ToString();
                Header().Q<Label>("Name").text = data.nameOnScore;
                Header().Q<Label>("Nation").text = data.nation;
                Header().Q<Label>("StartingNumber").text = data.startingNumber.ToString();
                Header().Q<Label>("TotalSegmentScore").text = data.judgeDetail.TotalSegmentScore().ToString("N2");
                Header().Q<Label>("TotalElementScore").text = data.judgeDetail.TotalElementScore().ToString("N2");
                Header().Q<Label>("TotalProgramComponentScore").text = data.judgeDetail.TotalProgramComponentScore().ToString("N2");
                Header().Q<Label>("TotalDeductions").text = data.judgeDetail.TotalDeductions().ToString("N2");
            }

            // ボディ部
            {
                // TES
                {
                    void setupValues(int index)
                    {
                        var item = InstantiateVisualTree(_itemAsset);
                        var labelIndex = 0;
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = $"{index + 1}";
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].executedElement;
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].UnifiedInfoMark();
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].baseValue.ToString("N2");
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].lastJump ? "X" : "";
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].goeScore.ToString("N2");
                        for (var i = 0; i < Constant.REFEREE_COUNT; i++)
                        {
                            item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].refereeGoe[i].ToString();
                        }
                        labelIndex++; // Refはスキップ
                        item.Q<Label>(_executedElementItemLabels[labelIndex++]).text = data.judgeDetail.tes[index].Score().ToString("N2");
                        ExecutedElementBody().Add(item);
                        _items.Add(item);
                    }
                    // 以前の情報はクリア
                    ExecutedElementBody().Clear();
                    _items.Clear();
                    // 値のセットアップ
                    for (var i = 0; i < data.judgeDetail.tes.Length; i++)
                    {
                        setupValues(i);
                    }
                    // 末尾要素の値の設定
                    ExecutedElementFooter().Q<Label>("TotalBaseValue").text = data.judgeDetail.TotalBaseValue().ToString("N2");
                    ExecutedElementFooter().Q<Label>("TotalElementScore").text = data.judgeDetail.TotalElementScore().ToString("N2");
                }
                // PCS
                {
                    // 値の設定
                    void setupValues(int index)
                    {
                        var item = ProgramComponentBody().Q<VisualElement>(_pcsLabels[index]);
                        item.Q<Label>(_pcsContentLabels[3]).text = data.judgeDetail.pcs[index].factor.ToString("N2");
                        for (var i = 0; i < Constant.REFEREE_COUNT; i++)
                        {
                            item.Q<Label>(_pcsContentLabels[4 + i]).text = data.judgeDetail.pcs[index].refereeScore[i].ToString("N2");
                        }
                        item.Q<Label>(_pcsContentLabels[14]).text = data.judgeDetail.pcs[index].RefereeScoreAverage().ToString("N2");
                    }
                    for (var i = 0; i < _pcsLabels.Length; i++)
                    {
                        setupValues(i);
                    }
                    ProgramComponentFooter().Q<Label>("Sop").text = data.judgeDetail.TotalProgramComponentScore().ToString("N2");
                }
            }

            // フッター部
            // TODO:Deduction 対応
            {
                Footer().Q<Label>("Deductions").text = data.judgeDetail.TotalDeductions().ToString("N2");
            }

            // 初期化完了
            _initialized = true;
        }

        // 更新
        // 描画内容に影響する処理なので毎プロセス呼び出す必要があります
        public void Update()
        {
            // 初期化されていなければ何もしない
            if (!_initialized) return;

            // TESヘッダ部の各要素の幅を取得。複数指定するとその合計値を得る
            // MEMO:パディングは含み、マージンとボーダーは含まない( VisualElement.localBound の性質)
            float GetExecutedElementHeaderWidth(params string[] labels)
            {
                return labels.Length == 1
                    ? ExecutedElementHeader().Q<VisualElement>(labels[0]).localBound.width
                    : labels.Aggregate(0.0f, (a, c) => ExecutedElementHeader().Q<VisualElement>(c).localBound.width + a);
            }
            // VisualElement.style.width への値設定。特殊なのでラップしておく
            void SetStyleWidth(VisualElement element, float width)
            {
                element.style.width = new StyleLength(new Length(width, LengthUnit.Pixel));
            }

            // TESボディ部
            {
                foreach (var item in _items)
                {
                    foreach (var label in _executedElementItemLabels)
                    {
                        SetStyleWidth(item.Q<VisualElement>(label), GetExecutedElementHeaderWidth(label));
                    }
                }
            }
            // TESフッター部
            {
                SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("Adjust1"), GetExecutedElementHeaderWidth(new ArraySegment<string>(_executedElementItemLabels, 0, 3).ToArray()));
                SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("TotalBaseValue"), GetExecutedElementHeaderWidth(_executedElementItemLabels[3]));
                SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("Adjust2"), GetExecutedElementHeaderWidth(new ArraySegment<string>(_executedElementItemLabels, 4, 12).ToArray()));
                SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("TotalElementScore"), GetExecutedElementHeaderWidth(_executedElementItemLabels[16]));
            }
            // PCSヘッダー部
            {
                SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("Adjust1"), GetExecutedElementHeaderWidth(_executedElementItemLabels[0]));
                SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("ProgramComponent"), GetExecutedElementHeaderWidth(_executedElementItemLabels[1]));
                SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("Adjust2"), GetExecutedElementHeaderWidth(new ArraySegment<string>(_executedElementItemLabels, 2, 3).ToArray()));
                SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("Factor"), GetExecutedElementHeaderWidth(_executedElementItemLabels[5]));
            }
            // PCSボディ部
            {
                for (int i = 0; i < _pcsLabels.Length; i++)
                {
                    var e = ProgramComponentBody().Q<VisualElement>(_pcsLabels[i]);
                    var labelIndex = 0;
                    SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(_executedElementItemLabels[0]));
                    SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(_executedElementItemLabels[1]));
                    SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(new ArraySegment<string>(_executedElementItemLabels, 2, 3).ToArray()));
                    SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(_executedElementItemLabels[5]));
                    for (var j = 0; j < Constant.REFEREE_COUNT; j++)
                    {
                        SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(_executedElementItemLabels[6 + j]));
                    }
                    SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(_executedElementItemLabels[15]));
                    SetStyleWidth(e.Q<VisualElement>(_pcsContentLabels[labelIndex++]), GetExecutedElementHeaderWidth(_executedElementItemLabels[16]));
                }
            }
            // PCSフッター部
            {
                SetStyleWidth(ProgramComponentFooter().Q<VisualElement>("Adjust1"), GetExecutedElementHeaderWidth(_executedElementItemLabels[0]));
                SetStyleWidth(ProgramComponentFooter().Q<VisualElement>("Name"), GetExecutedElementHeaderWidth(new ArraySegment<string>(_executedElementItemLabels, 1, 5).ToArray()));
                SetStyleWidth(ProgramComponentFooter().Q<VisualElement>("Sop"), GetExecutedElementHeaderWidth(new ArraySegment<string>(_executedElementItemLabels, 6, 11).ToArray()));
            }
        }

        private TemplateContainer InstantiateVisualTree(VisualTreeAsset asset)
        {
            var templateContainer = asset.Instantiate();
            // 生成した TemplateContainer は flexGrow=0 なので自身の子要素が表示されなくなる。なので明示的に flexGrow=1
            templateContainer.style.flexGrow = 1;
            return templateContainer;
        }

        private bool _initialized = false;
        private VisualTreeAsset _itemAsset;
        private readonly List<TemplateContainer> _items = new();

        private VisualElement Top() => Root.Q<VisualElement>("Top");
        private VisualElement Header() => Top().Q<VisualElement>("Header");
        private VisualElement Body() => Top().Q<VisualElement>("Body");
        private VisualElement ExecutedElementHeader() => Body().Q<VisualElement>("ExecutedElementHeader");
        private VisualElement ExecutedElementBody() => Body().Q<VisualElement>("ExecutedElementBody");
        private VisualElement ExecutedElementFooter() => Body().Q<VisualElement>("ExecutedElementFooter");
        private VisualElement ProgramComponentHeader() => Body().Q<VisualElement>("ProgramComponentHeader");
        private VisualElement ProgramComponentBody() => Body().Q<VisualElement>("ProgramComponentBody");
        private VisualElement ProgramComponentFooter() => Body().Q<VisualElement>("ProgramComponentFooter");
        private VisualElement Footer() => Top().Q<VisualElement>("Footer");

        private readonly string[] _executedElementItemLabels = new string[] {
            "Number",
            "ExecutedElement",
            "Info",
            "BaseValue",
            "LastJump",
            "Goe",
            "J1",
            "J2",
            "J3",
            "J4",
            "J5",
            "J6",
            "J7",
            "J8",
            "J9",
            "Ref",
            "Sop",
        };
        private readonly string[] _pcsLabels = new string[] {
            "Composition",
            "Presentation",
            "SkatingSkills",
        };
        private readonly string[] _pcsContentLabels = new string[] {
            "Adjust1",
            "Name",
            "Adjust2",
            "Factor",
            "J1",
            "J2",
            "J3",
            "J4",
            "J5",
            "J6",
            "J7",
            "J8",
            "J9",
            "Adjust3",
            "Sop",
        };
    }
}
