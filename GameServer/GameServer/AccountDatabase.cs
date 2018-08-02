using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using LiteDB;

namespace GameServer
{
    public class Account
    {
        public string id;
        public string password;
    }

    public class AccountDatabase : Database
    {
        private string path = @"Account.db";
        private string accounts = "accounts";
        public void Write(Account account)
        {
            using (var db = new LiteDatabase(path))
            {
                LiteCollection<Account> col = db.GetCollection<Account>(accounts);
                col.Insert(account);
            }
        }

        public Account Find(Expression<Func<Account, bool>> condition)
        {
            Account result = null;
            using (var db = new LiteDatabase(path))
            {
                LiteCollection<Account> col = db.GetCollection<Account>(accounts);
                result = col.FindOne(condition);
            }

            return result;
        }
    }
}
