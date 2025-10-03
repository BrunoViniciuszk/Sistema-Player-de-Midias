namespace Auth.Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }

        public AppUser(string username, string passwordHash)
        {
            Id = Guid.NewGuid(); 
            Username = username;
            PasswordHash = passwordHash;
        }

        protected AppUser() { }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}
