namespace BackendTraining.Services
{
    public class UserJwt
    {
        public UserJwt()
        {
        }

        public string Token { get; set; }
        public long ExpiresIn { get; set; }
        public int Id { get; internal set; }
    }
}