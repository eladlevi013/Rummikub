namespace rummikubGame.BrightnessOnHover
{
    public interface IBrightnessEffect
    {
        /*
            Interface of the BrightnessEffectComponent class,
            which is responsible for the brightness hovering effect.
            its exists in order to make it clear using that class.
            it contains some methods:
            - ApplyBrightness
            - RemoveBrightness
            - SetEffectState
        */

        void ApplyBrightness();
        void RemoveBrightness();
        void SetEffectState(bool value);
    }
}
