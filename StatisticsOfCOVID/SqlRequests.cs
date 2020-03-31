using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsOfCOVID
{
    public static class SqlRequests
    {
        public const string CREATE_NEW_TABLE = "CREATE TABLE {0} " +
            "(Id INTEGER PRIMARY KEY, " +
            "Title VARCHAR(100), " +
            "Link VARCHAR(100), " +
            "Guid VARCHAR(100), " +
            "Description TEXT, " +
            "pubDate VARCHAR(30));";

        public const string ADD_NEW_ARTICLE = "INSERT INTO {0} (Id, Title, Link, Guid, Description, pubDate) " +
            "VALUES (null, '{1}', '{2}', '{3}', '{4}', '{5}')";
    }
}
