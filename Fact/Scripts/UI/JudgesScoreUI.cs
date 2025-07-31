using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    /// <summary>
    /// 判定結果の表示を担うUIクラス。
    /// UXMLテンプレートを利用して、データに基づいたUI要素の生成と更新を行います。
    /// </summary>
    public class JudgesScoreUI
    {
        public TemplateContainer Root { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="judgesScoreUxml">Fact/UI/JudgesScore.uxml</param>
        /// <param name="judgesScoreExecutedElementItemUxml">Fact/UI/JudgesScoreExecutedElementItem.uxml</param>
        public JudgesScoreUI(VisualTreeAsset judgesScoreUxml, VisualTreeAsset judgesScoreExecutedElementItemUxml)
        {
            // ルートUIの生成
            Root = InstantiateVisualTree(judgesScoreUxml);

            // 実行済み構成要素表示UIを最大数生成して非表示にしておく
            for (var i = 0; i < CoreConstant.TES_MAX_COUNT; i++)
            {
                var item = InstantiateVisualTree(judgesScoreExecutedElementItemUxml);
                item.style.display = DisplayStyle.None;
                _items.Add(item);
            }
        }

        /// <summary>
        /// セットアップ。
        /// </summary>
        /// <param name="judgesScore">表示したいJudgesScoreクラス</param>
        public void Setup(JudgesScore judgesScore)
        {
            // judgesScoreのデータをUIにセット
            {
                // ヘッダー部
                {
                    var header = Header();
                    header.Q<Label>("Rank").text = judgesScore.rank.ToString();
                    header.Q<Label>("Name").text = judgesScore.nameOnScore;
                    header.Q<Label>("Nation").text = judgesScore.nation;
                    header.Q<Label>("StartingNumber").text = judgesScore.startingNumber.ToString();
                    header.Q<Label>("TotalSegmentScore").text = judgesScore.judgeDetail.TotalSegmentScore().ToString("N2");
                    header.Q<Label>("TotalElementScore").text = judgesScore.judgeDetail.TotalElementScore().ToString("N2");
                    header.Q<Label>("TotalProgramComponentScore").text = judgesScore.judgeDetail.TotalProgramComponentScore().ToString("N2");
                    header.Q<Label>("TotalDeductions").text = judgesScore.judgeDetail.TotalDeductions().ToString("N2");
                }

                // ボディ部
                {
                    // TES
                    {
                        var body = ExecutedElementBody();
                        void setupValues(int index)
                        {
                            var item = _items[index];
                            var labelIndex = 0;
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = $"{index + 1}";
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].executedElement;
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].UnifiedInfoMark();
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].baseValue.ToString("N2");
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].lastJump ? "X" : "";
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].goeScore.ToString("N2");
                            for (var i = 0; i < CoreConstant.REFEREE_COUNT; i++)
                            {
                                item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].refereeGoe[i].ToString();
                            }
                            labelIndex++; // Refはスキップ
                            item.Q<Label>(EXECUTED_ELEMENT_ITEM_LABELS[labelIndex++]).text = judgesScore.judgeDetail.tes[index].Score().ToString("N2");
                            // 表示ONにして親に登録
                            item.style.display = DisplayStyle.Flex;
                            body.Add(item);
                        }
                        // 以前の情報はクリア
                        body.Clear();
                        // 値のセットアップ
                        for (var i = 0; i < CoreConstant.TES_MAX_COUNT; i++)
                        {
                            if (i < judgesScore.judgeDetail.tes.Length)
                            {
                                setupValues(i);
                            }
                            else
                            {
                                // 未使用のアイテムは非表示に
                                _items[i].style.display = DisplayStyle.None;
                            }
                        }
                        // 末尾要素の値の設定
                        var footer = ExecutedElementFooter();
                        footer.Q<Label>("TotalBaseValue").text = judgesScore.judgeDetail.TotalBaseValue().ToString("N2");
                        footer.Q<Label>("TotalElementScore").text = judgesScore.judgeDetail.TotalElementScore().ToString("N2");
                    }
                    // PCS
                    {
                        // 値の設定
                        void setupValues(int index)
                        {
                            var item = ProgramComponentBody().Q<VisualElement>(PCS_LABELS[index]);
                            item.Q<Label>(PCS_CONTENT_LABELS[3]).text = judgesScore.judgeDetail.pcs[index].factor.ToString("N2");
                            for (var i = 0; i < CoreConstant.REFEREE_COUNT; i++)
                            {
                                item.Q<Label>(PCS_CONTENT_LABELS[4 + i]).text = judgesScore.judgeDetail.pcs[index].refereeScore[i].ToString("N2");
                            }
                            item.Q<Label>(PCS_CONTENT_LABELS[14]).text = judgesScore.judgeDetail.pcs[index].RefereeScoreAverage().ToString("N2");
                        }
                        for (var i = 0; i < PCS_LABELS.Length; i++)
                        {
                            setupValues(i);
                        }
                        ProgramComponentFooter().Q<Label>("Sop").text = judgesScore.judgeDetail.TotalProgramComponentScore().ToString("N2");
                    }
                }

                // フッター部
                // TODO:Deduction 対応
                {
                    Footer().Q<Label>("Deductions").text = judgesScore.judgeDetail.TotalDeductions().ToString("N2");
                }
            }

            Root.RegisterCallback<GeometryChangedEvent>(OnLayoutReady);
        }

        private void OnLayoutReady(GeometryChangedEvent evt)
        {
            Root.UnregisterCallback<GeometryChangedEvent>(OnLayoutReady);

            // Setupで設定したデータでレイアウトが決定した後に更新することができるレイアウトのセットアップ
            {
                // TESボディ部
                {
                    foreach (var item in _items)
                    {
                        foreach (var label in EXECUTED_ELEMENT_ITEM_LABELS)
                        {
                            SetStyleWidth(item.Q<VisualElement>(label), GetExecutedElementHeaderWidth(label));
                        }
                    }
                }
                // TESフッター部
                {
                    SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("Adjust1"), GetExecutedElementHeaderWidth(new ArraySegment<string>(EXECUTED_ELEMENT_ITEM_LABELS, 0, 3).ToArray()));
                    SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("TotalBaseValue"), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[3]));
                    SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("Adjust2"), GetExecutedElementHeaderWidth(new ArraySegment<string>(EXECUTED_ELEMENT_ITEM_LABELS, 4, 12).ToArray()));
                    SetStyleWidth(ExecutedElementFooter().Q<VisualElement>("TotalElementScore"), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[16]));
                }
                // PCSヘッダー部
                {
                    SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("Adjust1"), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[0]));
                    SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("ProgramComponent"), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[1]));
                    SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("Adjust2"), GetExecutedElementHeaderWidth(new ArraySegment<string>(EXECUTED_ELEMENT_ITEM_LABELS, 2, 3).ToArray()));
                    SetStyleWidth(ProgramComponentHeader().Q<VisualElement>("Factor"), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[5]));
                }
                // PCSボディ部
                {
                    for (int i = 0; i < PCS_LABELS.Length; i++)
                    {
                        var e = ProgramComponentBody().Q<VisualElement>(PCS_LABELS[i]);
                        var labelIndex = 0;
                        SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[0]));
                        SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[1]));
                        SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(new ArraySegment<string>(EXECUTED_ELEMENT_ITEM_LABELS, 2, 3).ToArray()));
                        SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[5]));
                        for (var j = 0; j < CoreConstant.REFEREE_COUNT; j++)
                        {
                            SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[6 + j]));
                        }
                        SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[15]));
                        SetStyleWidth(e.Q<VisualElement>(PCS_CONTENT_LABELS[labelIndex++]), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[16]));
                    }
                }
                // PCSフッター部
                {
                    SetStyleWidth(ProgramComponentFooter().Q<VisualElement>("Adjust1"), GetExecutedElementHeaderWidth(EXECUTED_ELEMENT_ITEM_LABELS[0]));
                    SetStyleWidth(ProgramComponentFooter().Q<VisualElement>("Name"), GetExecutedElementHeaderWidth(new ArraySegment<string>(EXECUTED_ELEMENT_ITEM_LABELS, 1, 5).ToArray()));
                    SetStyleWidth(ProgramComponentFooter().Q<VisualElement>("Sop"), GetExecutedElementHeaderWidth(new ArraySegment<string>(EXECUTED_ELEMENT_ITEM_LABELS, 6, 11).ToArray()));
                }
            }
        }

        private TemplateContainer InstantiateVisualTree(VisualTreeAsset asset)
        {
            var templateContainer = asset.Instantiate();
            // 生成した TemplateContainer は flexGrow=0 なので自身の子要素が表示されなくなる。なので明示的に flexGrow=1
            templateContainer.style.flexGrow = 1;
            return templateContainer;
        }

        // TESヘッダ部の各要素の幅を取得。複数指定するとその合計値を得る
        // MEMO:パディングは含み、マージンとボーダーは含まない( VisualElement.localBound の性質)
        private float GetExecutedElementHeaderWidth(params string[] labels)
        {
            var header = ExecutedElementHeader();
            return labels.Length == 1
                ? header.Q<VisualElement>(labels[0]).localBound.width
                : labels.Aggregate(0.0f, (a, c) => header.Q<VisualElement>(c).localBound.width + a);
        }

        // VisualElement.style.width への値設定。特殊なのでラップしておく
        private void SetStyleWidth(VisualElement element, float width)
        {
            element.style.width = new StyleLength(new Length(width, LengthUnit.Pixel));
        }

        private readonly List<TemplateContainer> _items = new();

        private VisualElement Header() => Root.Q<VisualElement>("Header");
        private VisualElement ExecutedElementHeader() => Root.Q<VisualElement>("ExecutedElementHeader");
        private VisualElement ExecutedElementBody() => Root.Q<VisualElement>("ExecutedElementBody");
        private VisualElement ExecutedElementFooter() => Root.Q<VisualElement>("ExecutedElementFooter");
        private VisualElement ProgramComponentHeader() => Root.Q<VisualElement>("ProgramComponentHeader");
        private VisualElement ProgramComponentBody() => Root.Q<VisualElement>("ProgramComponentBody");
        private VisualElement ProgramComponentFooter() => Root.Q<VisualElement>("ProgramComponentFooter");
        private VisualElement Footer() => Root.Q<VisualElement>("Footer");

        private readonly string[] EXECUTED_ELEMENT_ITEM_LABELS = new string[] {
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
        private readonly string[] PCS_LABELS = new string[] {
            "Composition",
            "Presentation",
            "SkatingSkills",
        };
        private readonly string[] PCS_CONTENT_LABELS = new string[] {
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
