# FigureSkateFrameworkForUnity

フィギュアスケートにおけるジャンプなどを構成して判定して結果を得る一連の流れを提供する [Unity](https://unity.com/) で動作する仕組みです。以下のような採点結果を表示するUIも同梱されています。[メダリスト](https://afternoon.kodansha.co.jp/c/medalist/) を応援しています！

![](https://github.com/user-attachments/assets/9ac07e5c-74f3-43b8-9633-12786301212d)

## はじめに

こちらを利用してアプリを作成したい！ という方は製作者にEメールにてご連絡ください。2025/1/27の時点、2023-24シーズンのみが利用可能だったり減点項目が実装しきれていなかったりしますので、どう利用したいかをすり合わせさせてください

## 特徴

CoreとFactに分かれています
- Coreは、フィギュアスケートにおいて必要となるデータ構造を提供します。Coreだけ利用することで、独自のフィギュアスケートルールを作成することができます
- Factは、Coreを用いて [ISU](https://current.isu.org/) や [JSF](https://www.jsfresults.com/index.htm) を参考に、可能な限り事実に即したフィギュアスケートのルールを提供します。こちらを用いることでリアルなフィギュアスケートルールを実現するアプリを作成することができます

## 動作環境

- Unity 6 (6000.0.33f1で動作確認してます)
- [Unity Addressables](https://docs.unity3d.com/Packages/com.unity.addressables@2.2/manual/index.html) (2.2.2 で動作確認してます)

## インストール方法

1. [リリースページ](https://github.com/wlg-shinya/FigureSkateFrameworkForUnity/releases) にて任意のパッケージをダウンロードします
2. Unity の Package Manager を開き、ダウンロードしたパッケージに合わせて、disk か tarball でパッケージを追加してください
3. Unity エディタのメニューから Window > Asset Management > Addressables > Groups を選択して、表示されたウィンドウに Packages/com.welovegamesinc.figureskate-framework/Fact/AddressableAssetsData/AssetGroups/FigureSkateFramework.asset を登録してください

## 使い方
### Fact
[IntegrationTest.Competition](https://github.com/wlg-shinya/FigureSkateFrameworkForUnity/blob/d38c282963d525bbf30f65f22f628cc72b6632b4/Tests/Fact/Runtime/IntegrationTest.cs#L21C28-L21C39) に選手を用意して構成要素を設定して判定する一連の実装がありますので参考にしてください。もっとわかりやすい資料は鋭意作成中です

### Core

準備中

## 各機能の説明

準備中
