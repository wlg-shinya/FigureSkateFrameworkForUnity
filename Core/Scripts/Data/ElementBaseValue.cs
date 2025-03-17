using System;

namespace Wlg.FigureSkate.Core
{
    // 構成要素の基礎点
    // 2A はダブルアクセルと表記することやGOE判定はジャンプに準ずることは Element で扱う
    // Element と異なり全減点要素(qや!など)も扱う
    [Serializable]
    public class ElementBaseValue
    {
        // 一意の記号
        public string id;
        // 基礎点(GOE-5)
        public float baseValueM5;
        // 基礎点(GOE-4)
        public float baseValueM4;
        // 基礎点(GOE-3)
        public float baseValueM3;
        // 基礎点(GOE-2)
        public float baseValueM2;
        // 基礎点(GOE-1)
        public float baseValueM1;
        // 基礎点
        public float baseValue;
        // 基礎点(GOE+1)
        public float baseValueP1;
        // 基礎点(GOE+2)
        public float baseValueP2;
        // 基礎点(GOE+3)
        public float baseValueP3;
        // 基礎点(GOE+4)
        public float baseValueP4;
        // 基礎点(GOE+5)
        public float baseValueP5;
    }
}