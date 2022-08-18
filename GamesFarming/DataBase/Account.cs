using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.DataBase
{
    internal class Account
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int GameCode { get; set; }
        public int Optimize { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }

        public Account() { }

        public Account(string login, string password, int gameCode, int optimize, int resX, int resY)
        {
            Login = login;
            Password = password;
            GameCode = gameCode;
            Optimize = optimize;
            ResX = resX;
            ResY = resY;
        }
    }
}
