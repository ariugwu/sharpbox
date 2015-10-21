using sharpbox.Common.Type;

namespace sharpbox.Bootstrap.Package.Core
{
    public class LifeCycleHandlerState : EnumPattern
    {
        public static LifeCycleHandlerState Success = new LifeCycleHandlerState { Value = "Success"};
        public static LifeCycleHandlerState Warning = new LifeCycleHandlerState { Value = "Warning" };
        public static LifeCycleHandlerState Info = new LifeCycleHandlerState { Value = "Info" };
        public static LifeCycleHandlerState Error = new LifeCycleHandlerState { Value = "Error" }; 
    }
}