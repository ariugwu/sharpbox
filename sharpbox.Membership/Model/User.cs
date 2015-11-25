namespace sharpbox.Membership.Model
{
    public class User : Microsoft.AspNet.Identity.IUser<int>
    {
        public int Id { get; }
        public string UserName { get; set; }
    }
}
