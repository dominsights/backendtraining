namespace BackendTraining.Services
{
    public class UserPayload
    {
        public UserPayload()
        {
        }

        public string Token { get; set; }
        public long ExpiresIn { get; set; }
        public int Id { get; internal set; }
    }
}