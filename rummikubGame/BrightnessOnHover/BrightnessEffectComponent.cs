using rummikubGame.BrightnessOnHover;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace rummikubGame.Draggable
{
    public class BrightnessEffectComponent : IBrightnessEffect
    {
        // Brightness on hover effect constants
        private const float DefaultBrightnessLevel = 1.0f;
        private const float HoverBrightnessLevel = 1.2f;

        // Varibales for brightness hovering effect
        private Image _brightnessImage;
        private Image _originalBackgroundImage;
        private float _currentBrightnessLevel = DefaultBrightnessLevel;
        private Control _control;
        private bool _isEnabled = false;

        public BrightnessEffectComponent(Control control)
        {
            _control = control;
            control.MouseEnter += ControlApplyBrightness_MouseEnter;
            control.MouseLeave += ControlRemoveBrightness_MouseLeave;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        public void ControlApplyBrightness_MouseEnter(object sender, EventArgs e)
        {
            SetEffectState(true);
        }

        public void ControlRemoveBrightness_MouseLeave(object sender, EventArgs e)
        {
            SetEffectState(false);
        }

        public void ApplyBrightness()
        {
            _originalBackgroundImage = _control.BackgroundImage;

            if (_control.BackgroundImage == null || HoverBrightnessLevel == _currentBrightnessLevel)
            {
                return;
            }

            if (_brightnessImage == null)
            {
                // Generate brightness image
                float[][] matrixItems ={
                        new float[] { HoverBrightnessLevel, 0, 0, 0, 0},
                        new float[] {0, HoverBrightnessLevel, 0, 0, 0},
                        new float[] {0, 0, HoverBrightnessLevel, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    };

                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                Bitmap bmp = new Bitmap(_originalBackgroundImage.Width, _originalBackgroundImage.Height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(_originalBackgroundImage, new Rectangle(0, 0, bmp.Width, bmp.Height),
                        0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }
                _brightnessImage = bmp;
            }
            _control.BackgroundImage = _brightnessImage;
            _currentBrightnessLevel = HoverBrightnessLevel;
        }

        public void RemoveBrightness()
        {
            if (_control.BackgroundImage == null || _originalBackgroundImage == null
                || _currentBrightnessLevel == DefaultBrightnessLevel)
            {
                return;
            }
            _control.BackgroundImage = _originalBackgroundImage;
            _currentBrightnessLevel = DefaultBrightnessLevel;
        }

        public void SetEffectState(bool value)
        {
            if(_isEnabled == value)
            {
                return;
            }
            else
            {
                _isEnabled = value;
                if (_isEnabled)
                {
                    ApplyBrightness();
                }
                else
                {
                    RemoveBrightness();
                }
            }
        }

        public async void BrightnessTime(int seconds)
        {
            SetEffectState(true);
            await Task.Delay(TimeSpan.FromSeconds(1));
            SetEffectState(false);
        }
    }
}
