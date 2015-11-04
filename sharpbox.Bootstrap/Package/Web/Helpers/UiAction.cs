using sharpbox.Common.Type;

namespace sharpbox.WebLibrary.Helpers
{
    public class UiAction : EnumPattern
    {
        public UiAction() { }



        public UiAction(string value) : base(value) { }

        public string Name
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }
        public static UiAction Update = new UiAction("Update");
        public static UiAction Insert = new UiAction("Insert");
        public static UiAction Delete = new UiAction("Delete");
    }
}
