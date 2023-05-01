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
        
        public float LeftButtonHisterezis
        {
            get => SingleManager.MouseHandController.LeftTrigger.Histerezis;
            set => SingleManager.MouseHandController.LeftTrigger.Histerezis = value;
        }

        public float RightButtonHisterezis
        {
            get => SingleManager.MouseHandController.RightTrigger.Histerezis;
            set => SingleManager.MouseHandController.RightTrigger.Histerezis = value;
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
