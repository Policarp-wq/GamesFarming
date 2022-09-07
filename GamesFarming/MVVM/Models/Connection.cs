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
            return $"\"connect {IP}; password {Password};\"";
        }
    }
}
