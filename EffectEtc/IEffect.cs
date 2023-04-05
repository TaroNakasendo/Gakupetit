namespace Com.Nakasendo.Gakupetit.EffectEtc;

interface IEffect
{
    int EffectId { get; }
    string[] Names { get; }
    bool IsBackChecked { get; }
    int DefaultValue { get; }
    string[] Descriptions { get; }
    Color GetDefaultColor(Color nowColor);
    Bitmap DoEffect(int v, Color color, Bitmap srcBitmap);
}
