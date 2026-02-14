using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E037_Scribble(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 37;
    public string[] Names => ["Scribble", "落書き線"];
    public bool IsBackChecked => true;
    public int DefaultValue => 30;
    public string[] Descriptions => [
        "Draws a rough, hand-drawn style frame. Adjust the messiness and line count with the slider.",
        "手描きのラフスケッチ風の枠を描きます。スライダーで線の乱れ具合と重ね書きの量を調整します。"
    ];
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        if (v == 0) return new(srcBitmap);

        Bitmap bmp = new(srcBitmap);
        var w = bmp.Width;
        var h = bmp.Height;
        
        // パラメータ調整
        // 線を引く領域（端からのマージン）
        int margin = Math.Min(w, h) / 20; 
        // 線の本数 (2本 〜 5本程度)
        int count = 2 + v / 20;
        // 乱れの大きさ
        float jitterBase = Math.Max(2, Math.Min(w, h) * v / 2000.0f); 

        // 乱数シードを固定しないと、再描画のたびに線がパラパラ動いてしまう可能性があるが
        // ここではランダム感を重視して都度生成する。
        // チラつきが気になる場合は new Random(EffectId + v) などにする。
        Random rnd = new Random(EffectId + v);

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 線の太さ
            float penWidth = Math.Max(1.0f, Math.Min(w, h) / 400.0f);
            
            // 色のアルファを少し下げて重ね書き感を出す
            int alpha = Math.Min(255, 200); 
            using var pen = new Pen(Color.FromArgb(alpha, color), penWidth);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;

            // 各レイヤー（重ね書き）
            for (int i = 0; i < count; i++)
            {
                // 四角形の頂点を定義（左上、右上、右下、左下）
                // 毎回少しずらす
                PointF p1 = Jitter(margin, margin, jitterBase, rnd);
                PointF p2 = Jitter(w - margin, margin, jitterBase, rnd);
                PointF p3 = Jitter(w - margin, h - margin, jitterBase, rnd);
                PointF p4 = Jitter(margin, h - margin, jitterBase, rnd);

                // 各辺を描画する（少しオーバーランさせる）
                DrawRoughLine(g, pen, p1, p2, jitterBase, rnd);
                DrawRoughLine(g, pen, p2, p3, jitterBase, rnd);
                DrawRoughLine(g, pen, p3, p4, jitterBase, rnd);
                DrawRoughLine(g, pen, p4, p1, jitterBase, rnd);
            }
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    // 座標にゆらぎを与える
    private static PointF Jitter(float x, float y, float amount, Random rnd)
    {
        return new PointF(
            x + (float)(rnd.NextDouble() - 0.5) * amount * 2,
            y + (float)(rnd.NextDouble() - 0.5) * amount * 2
        );
    }

    // 手書き風ライン描画（ベジェ曲線を使って揺らがせる）
    private static void DrawRoughLine(Graphics g, Pen pen, PointF start, PointF end, float amount, Random rnd)
    {
        // 始点と終点を少し外側に延長（オーバーラン）
        float overshoot = amount * 1.5f;
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;
        float length = (float)Math.Sqrt(dx * dx + dy * dy);
        
        if (length < 1) return;

        // 正規化ベクトル
        float nx = dx / length;
        float ny = dy / length;

        // 延長後の始点・終点
        PointF realStart = new PointF(start.X - nx * overshoot, start.Y - ny * overshoot);
        PointF realEnd = new PointF(end.X + nx * overshoot, end.Y + ny * overshoot);

        // 中間制御点を生成してカーブを描く
        // 単純な直線ではなく、途中でうねらせる
        PointF mid1 = new PointF(
            start.X + dx * 0.33f + (float)(rnd.NextDouble() - 0.5) * amount,
            start.Y + dy * 0.33f + (float)(rnd.NextDouble() - 0.5) * amount
        );
        PointF mid2 = new PointF(
            start.X + dx * 0.66f + (float)(rnd.NextDouble() - 0.5) * amount,
            start.Y + dy * 0.66f + (float)(rnd.NextDouble() - 0.5) * amount
        );

        g.DrawBezier(pen, realStart, mid1, mid2, realEnd);
    }
}
