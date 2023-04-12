using HandControl.App.Model;

namespace HandControl.App.Configuration
{
    public class ControlConfig
    {
        public float HorizontalNonSensitiveZone 
        { 
            get => SingleManager.MouseHandController.HorizontalNonSensitiveZone; 
            set => SingleManager.MouseHandController.HorizontalNonSensitiveZone = value; 
        }
        public float HorizontalAcseleration
        {
            get => SingleManager.MouseHandController.HorizontalAcseleration;
            set => SingleManager.MouseHandController.HorizontalAcseleration = value;
        }
        public float HorizontalOffset
        {
            get => SingleManager.MouseHandController.HorizontalOffset;
            set => SingleManager.MouseHandController.HorizontalOffset = value;
        }
        public float HorizontalLimitSpeed
        {
            get => SingleManager.MouseHandController.HorizontalLimitSpeed;
            set => SingleManager.MouseHandController.HorizontalLimitSpeed = value;
        }
        public float VerticalNonSensitiveZone
        {
            get => SingleManager.MouseHandController.VerticalNonSensitiveZone;
            set => SingleManager.MouseHandController.VerticalNonSensitiveZone = value;
        }
        public float VerticalOffset
        {
            get => SingleManager.MouseHandController.VerticalOffset;
            set => SingleManager.MouseHandController.VerticalOffset = value;
        }
        public float VerticalAcseleration
        {
            get => SingleManager.MouseHandController.VerticalAcseleration;
            set => SingleManager.MouseHandController.VerticalAcseleration = value;
        }
        public float VerticalLimitSpeed
        {
            get => SingleManager.MouseHandController.VerticalLimitSpeed;
            set => SingleManager.MouseHandController.VerticalLimitSpeed = value;
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
        public float ActivationLeftMouseZone
        {
            get => SingleManager.MouseHandController.ActivationLeftMouseZone;
            set => SingleManager.MouseHandController.ActivationLeftMouseZone = value;
        }
        public float ActivationRightMouseZone
        {
            get => SingleManager.MouseHandController.ActivationRightMouseZone;
            set => SingleManager.MouseHandController.ActivationRightMouseZone = value;
        }

        public Mark HorizontalRemote
        {
            get => SingleManager.MouseHandController.HorizontalRemote;
            set => SingleManager.MouseHandController.HorizontalRemote = value;
        }
        public Mark HorizontalMain
        {
            get => SingleManager.MouseHandController.HorizontalMain;
            set => SingleManager.MouseHandController.HorizontalMain = value;
        }
        public Mark VerticalRemote
        {
            get => SingleManager.MouseHandController.VerticalRemote;
            set => SingleManager.MouseHandController.VerticalRemote = value;
        }
        public Mark VerticalMain
        {
            get => SingleManager.MouseHandController.VerticalMain;
            set => SingleManager.MouseHandController.VerticalMain = value;
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

        public Mark LeftButtonTrigger1
        {
            get => SingleManager.MouseHandController.LeftButtonTrigger1;
            set => SingleManager.MouseHandController.LeftButtonTrigger1 = value;
        }
        public Mark LeftButtonTrigger2
        {
            get => SingleManager.MouseHandController.LeftButtonTrigger2;
            set => SingleManager.MouseHandController.LeftButtonTrigger2 = value;
        }
        public Mark RightButtonTrigger1
        {
            get => SingleManager.MouseHandController.RightButtonTrigger1;
            set => SingleManager.MouseHandController.RightButtonTrigger1 = value;
        }
        public Mark RightButtonTrigger2
        {
            get => SingleManager.MouseHandController.RightButtonTrigger2;
            set => SingleManager.MouseHandController.RightButtonTrigger2 = value;
        }
    }
}
