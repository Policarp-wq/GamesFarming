using GamesFarming.MVVM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesFarming.DataBase
{
    public class DBAccess<T>
    {
        public string DBFileName;
        public static string FolderPath;
        public string DBPath => FolderPath + DBFileName;
        public event Action DBChanged;
        static DBAccess()
        {
            FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
        }
        public DBAccess(DBKeys key)
        {
            DBFileName = DBKeys.DBFileNames[key];
        }

        public void WriteToDB(T item)
        {
            if(item == null)
                return;
            var unSerializedItems = GetItems();
            unSerializedItems.Add(item);
            string serializedItems = JsonConvert.SerializeObject(unSerializedItems);
            Write(serializedItems);

        }
        public void WriteToDB(IEnumerable<T> items)
        {
            var unSerializedItems = GetItems();
            unSerializedItems.AddRange(items.Where(a => (a != null)));
            string serializedItems = JsonConvert.SerializeObject(unSerializedItems);
            Write(serializedItems);

        }

        public void ReuploadDB(IEnumerable<T> items)
        {
            ClearDB();
            WriteToDB(items);
        }

        public List<T> GetItems()
        {
            string serializedItems = Read();
            List<T> unSerializedItems = JsonConvert.DeserializeObject<List<T>>(serializedItems);
            if (unSerializedItems is null)
                return Enumerable.Empty<T>().ToList();
            return unSerializedItems;
        }

        public void ClearDB()
        {
            Write("");
        }

        public void DeleteFromDB(T deleting)
        {
            var unSerializedItems = GetItems();
            unSerializedItems.Remove(unSerializedItems.FirstOrDefault(item => item.Equals(deleting)));

            string serializedItems = JsonConvert.SerializeObject(unSerializedItems);
            Write(serializedItems);
        }
        public void DeleteFromDB(IEnumerable<T> deletingItems)
        {
            var unSerializedItems = GetItems();
            unSerializedItems.RemoveAll(item => deletingItems.Contains(item));

            string serializedUsers = JsonConvert.SerializeObject(unSerializedItems);
            Write(serializedUsers);
        }

        //public void DeleteFromDB(IEnumerable<T> items)
        //{
        //    var unSerializedItems = GetItems();
        //    unSerializedItems.RemoveAll(acc => logins.Contains(acc.Login));
        //    WriteToDB(unSerializedItems);
        //}
        private void Write(string text)
        {
            FileSafeAccess.WriteToFile(DBPath, text);
            DBChanged?.Invoke();
        }
        private string Read()
        {
            var str = FileSafeAccess.ReadFile(DBPath);
            if (str is null)
                return "";
            return str;
        }
    }

}
