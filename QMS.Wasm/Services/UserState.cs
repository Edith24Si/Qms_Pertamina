namespace QMS.Services
{
    public class UserState
    {
        // Status login & role default
        public string CurrentRole { get; private set; } = "User"; // Nilai: "Administrator" atau "User"
        public string Username { get; private set; } = "Pegawai Mutu";
        public string UserEmail { get; private set; } = "pegawai@gmail.com";
        public string PhotoUrl { get; private set; } = "";

        public bool IsAdmin => 
            !string.IsNullOrEmpty(CurrentRole) && 
            (CurrentRole.Equals("Administrator", StringComparison.OrdinalIgnoreCase) || 
             CurrentRole.Equals("Admin", StringComparison.OrdinalIgnoreCase));

        // Event untuk memberitahu komponen jika status login/role/foto berubah
        public event Action? OnChange;

        public void SetUser(string role, string name, string email, string photoUrl = "")
        {
            CurrentRole = role;
            Username = name;
            UserEmail = email;
            PhotoUrl = photoUrl;
            NotifyStateChanged();
        }

        public void SetRole(string role, string name)
        {
            CurrentRole = role;
            Username = name;
            NotifyStateChanged();
        }

        public void SetPhoto(string photoUrl)
        {
            PhotoUrl = photoUrl;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
