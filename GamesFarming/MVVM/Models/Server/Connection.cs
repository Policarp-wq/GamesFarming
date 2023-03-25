namespace GamesFarming.MVVM.Models
{
    public class Connection
    { 
        public string IP { get; set; }
        public string Password { get; set; }
        public Connection(string ip, string password)
        {
            IP = ip;
            Password = password;
        }
        public override string ToString()
        {
            return $"+connect \"connect {IP};" + (Password.Length > 0 ? $"password {Password};\"" : "\"");
        }
        public override bool Equals(object obj)
        {
            var connect = obj as Connection;
            if (connect == null)
                return false;
            return GetHashCode() == connect.GetHashCode();
        }
        public override int GetHashCode()
        {
            return IP.GetHashCode();
        }
    }
}
