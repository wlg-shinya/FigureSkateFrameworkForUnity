# FigureSkateFrameworkForUnity

フィギュアスケートにおけるジャンプなどを構成して判定して結果を得る一連の流れを提供する [Unity](https://unity.com/) で動作する仕組みです。以下のような採点結果を表示するUIも同梱されています。[メダリスト](https://afternoon.kodansha.co.jp/c/medalist/) を応援しています！

![](https://github.com/user-attachments/assets/9ac07e5c-74f3-43b8-9633-12786301212d)

## はじめに

こちらを利用してアプリを作成したいけど機能が足りない！ という方は製作者にEメールにてご連絡ください。2025/3/17の時点、減点項目(Deduction)や2024-25シーズンのGOE上限ルールなど明確に対応できていない要素がありますので、どう利用したいかをすり合わせさせてください

## 動作環境

- Unity 6 (6000.0.33f1で動作確認してます)
- [Unity Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@2.2/manual/index.html) (2.2.2 で動作確認してます)

## インストール方法

1. [リリースページ](https://github.com/wlg-shinya/FigureSkateFrameworkForUnity/releases) にて任意のパッケージをダウンロードします
2. 解凍したフォルダ名を **com.welovegamesinc.figureskate-framework** に書き換えてプロジェクト直下の Packages 以下に配置します
    - これを省くと、UI Builder利用時などの一部状況で不都合が出ることを確認しています
3. Unity の Package Manager を開いてパッケージを追加します
4. Unity エディタのメニューから Window > Asset Management > Addressables > Groups を選択して、表示されたウィンドウに Packages/com.welovegamesinc.figureskate-framework/Fact/AddressableAssetsData/AssetGroups/FigureSkateFramework.asset を登録して、設定をプロジェクトの都合に合わせて更新します
    - とりあえず動かすだけなら Build & Load Paths を Local に設定すると正常に Build and run できるようになります

## 特徴

CoreとFactに分かれています
- Coreは、フィギュアスケートにおいて必要となるデータ構造と判定の仕組みを提供します。Coreだけ利用することで、独自のフィギュアスケートルールを作成することができます
- Factは、Coreを用いて [ISU](https://current.isu.org/) や [JSF](https://www.jsfresults.com/index.htm) を参考に、可能な限り事実に即したフィギュアスケートのルールを提供します。こちらを用いることでリアルなフィギュアスケートルールを実現するアプリを作成することができます

## 使い方
### Fact
[IntegrationTest.Competition](https://github.com/wlg-shinya/FigureSkateFrameworkForUnity/blob/d38c282963d525bbf30f65f22f628cc72b6632b4/Tests/Fact/Runtime/IntegrationTest.cs#L21) に選手を用意して構成要素を設定して判定する一連の実装がありますので参考にしてください。もっとわかりやすい資料は鋭意作成中です

### Core

準備中

## 各機能の説明

準備中
