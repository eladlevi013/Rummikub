namespace rummikubGame.BrightnessOnHover
{
    public interface IBrightnessEffect
    {
        void ApplyBrightness();
        void RemoveBrightness();
        void SetEffectState(bool value);
    }
}
