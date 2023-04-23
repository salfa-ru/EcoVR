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

        public float LeftTriggerHisterezis
        {
            get => SingleManager.MouseHandController.LeftTriggerHisterezis;
            set => SingleManager.MouseHandController.LeftTriggerHisterezis = value;
        }

        public float RightTriggerHisterezis
        {
            get => SingleManager.MouseHandController.RightTriggerHisterezis;
            set => SingleManager.MouseHandController.RightTriggerHisterezis = value;
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
