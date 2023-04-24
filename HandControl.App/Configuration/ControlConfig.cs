using HandControl.App.Model;

namespace HandControl.App.Configuration
{
    public class ControlConfig
    {
        public int NonSensitiveZone 
        { 
            get => SingleManager.MouseHandController.NonSensitiveZone; 
            set => SingleManager.MouseHandController.NonSensitiveZone = value; 
        }
        public float EdgeOffset
        {
            get => SingleManager.MouseHandController.EdgeOffset;
            set => SingleManager.MouseHandController.EdgeOffset = value;
        }

        public int Smoother
        {
            get => SingleManager.MouseHandController.Smoother;
            set => SingleManager.MouseHandController.Smoother = value;
        }



        public float ScrollNonsensitiveZone
        {
            get => SingleManager.MouseHandController.ScrollNonsensitiveZone;
            set => SingleManager.MouseHandController.ScrollNonsensitiveZone = value;
        }
        public float ScrollOffset
        {
            get => SingleManager.MouseHandController.ScrollOffset;
            set => SingleManager.MouseHandController.ScrollOffset = value;
        }
        public float ScrollAcseleration
        {
            get => SingleManager.MouseHandController.ScrollAcseleration;
            set => SingleManager.MouseHandController.ScrollAcseleration = value;
        }
        public float ScrollLimitSpeed
        {
            get => SingleManager.MouseHandController.ScrollLimitSpeed;
            set => SingleManager.MouseHandController.ScrollLimitSpeed = value;
        }

        public Mark ScrollRemote
        {
            get => SingleManager.MouseHandController.ScrollRemote;
            set => SingleManager.MouseHandController.ScrollRemote = value;
        }
        public Mark ScrollMain
        {
            get => SingleManager.MouseHandController.ScrollMain;
            set => SingleManager.MouseHandController.ScrollMain = value;
        }

        public int LeftButtonFinger
        {
            get => SingleManager.MouseHandController.LeftTrigger.SelectedFinger;
            set => SingleManager.MouseHandController.LeftTrigger.SelectedFinger = value;
        }
        public int LeftButtonTarget
        {
            get => SingleManager.MouseHandController.LeftTrigger.SelectedTarget;
            set => SingleManager.MouseHandController.LeftTrigger.SelectedTarget = value;
        }
        public int RightButtonFinger
        {
            get => SingleManager.MouseHandController.RightTrigger.SelectedFinger;
            set => SingleManager.MouseHandController.RightTrigger.SelectedFinger = value;
        }
        public int RightButtonTarget
        {
            get => SingleManager.MouseHandController.RightTrigger.SelectedTarget;
            set => SingleManager.MouseHandController.RightTrigger.SelectedTarget = value;
        }
    }
}
