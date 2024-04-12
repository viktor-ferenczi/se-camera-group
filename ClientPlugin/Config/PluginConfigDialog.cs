using System;
using Sandbox;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin
{
    public class PluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "Camera Group Configuration";
        public override string GetFriendlyName() => "CameraGroupConfigDialog";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel disableThirdPersonViewLabel;
        private MyGuiControlCheckbox disableThirdPersonViewCheckbox;

        private MyGuiControlButton closeButton;

        public PluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.6f, 0.4f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            var config = Plugin.Config.Data;
            CreateCheckbox(out disableThirdPersonViewLabel, out disableThirdPersonViewCheckbox, config.DisableThirdPersonView, value => config.DisableThirdPersonView = value, "Disable third person view", null);

            closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOk);
        }

        private void OnOk(MyGuiControlButton _) => CloseScreen();

        private void CreateCheckbox(out MyGuiControlLabel labelControl, out MyGuiControlCheckbox checkboxControl, bool value, Action<bool> store, string label, string tooltip, bool enabled = true)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = enabled,
            };

            checkboxControl = new MyGuiControlCheckbox(toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                IsChecked = value,
                Enabled = enabled,
                CanHaveFocus = enabled
            };
            if (enabled)
            {
                checkboxControl.IsCheckedChanged += cb => store(cb.IsChecked);
            }
            else
            {
                checkboxControl.IsCheckedChanged += cb => { cb.IsChecked = value; };
            }
        }

        private void LayoutControls()
        {
            var size = new Vector2(0.5f, 0.3f);
            layoutTable = new MyLayoutTable(this, -0.5f * size, size);
            layoutTable.SetColumnWidths(60f, 940f);
            layoutTable.SetRowHeights(200f, 60f, 250f, 150f);

            layoutTable.Add(disableThirdPersonViewCheckbox, MyAlignH.Left, MyAlignV.Center, 0, 0);
            layoutTable.Add(disableThirdPersonViewLabel, MyAlignH.Left, MyAlignV.Center, 0, 1);

            layoutTable.Add(closeButton, MyAlignH.Right, MyAlignV.Center, 2, 0, colSpan: 2);
        }
    }
}