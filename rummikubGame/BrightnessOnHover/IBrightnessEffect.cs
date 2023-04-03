using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame.BrightnessOnHover
{
    public interface IBrightnessEffect
    {
        void ApplyBrightness();
        void RemoveBrightness();
        void SetEffectState(bool value);
    }
}
