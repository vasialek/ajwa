using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ConsoleApplication1
{
    class ProgramMain
    {
        static public MySqlConnection connection;

        static public string server = "localhost";
        static public string uid = "root";
        static public string password = "";

        static public string server_to = "";
        static public string uid_to = "";
        static public string password_to = "";

        static public string connectionString = "";
        static public string connectionStringDest = "";
        static public string connectionStringAlt = "";

        static public void init()
        {

            Console.WriteLine("Initialisation...");
            Console.WriteLine();
            Console.WriteLine("Host - " + server);
            Console.WriteLine("User - " + uid);
            Console.WriteLine("Password - " + password);

            Console.WriteLine();
            Console.WriteLine("Destination:");
            Console.WriteLine("Host - " + server_to);
            Console.WriteLine("User - " + uid_to);
            Console.WriteLine("Password - " + password_to);


            connectionStringAlt = "SERVER=" + server + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" +"DATABASE=aiva;"; 
            connectionString = "SERVER=" + server + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connectionStringDest = "SERVER=" + server_to + ";" + "UID=" + uid_to + ";" + "PASSWORD=" + password_to + ";";


            connection = new MySqlConnection(connectionString);

        }

        static public bool OpenConnection()
        {
            try
            {
                connection.Open();
				// Setting tiimeout on mysqlServer
                MySqlCommand cmd = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", connection);
				cmd.ExecuteNonQuery();

                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public static void debugvar(MySqlDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Console.WriteLine("[" + dataReader.GetName(i) + "] = " + dataReader[i]);
            }
            Console.WriteLine("====================================================");
        }


        public static MySqlDataReader select(MySqlConnection con, string db, string table, string field, string value, string field2 = "", string value2 = "", string field3 = "", string value3 = "", string field4 = "", string value4 = "")
        {
            MySqlDataReader dataReader = null;
            string query = "SELECT * FROM `" + db + "`.`" + table + "` WHERE `" + field + "` = '" + value + "'";
            if (field2 != "") { query += " AND `" + field2 + "` = '" + value2 + "'"; }
            if (field3 != "") { query += " AND `" + field3 + "` = '" + value3 + "'"; }
            if (field4 != "") { query += " AND `" + field4 + "` = '" + value4 + "'"; }
            
            
            //executing query
            try 
            {
                MySqlCommand cmd = new MySqlCommand(query, con); 
                dataReader = cmd.ExecuteReader();
            }
            catch 
            {
                Console.WriteLine("Error!");
            }


            return dataReader;
        }

        public static void update(string db, string table, string field, string value, string field2 = "", string value2 = "", string field3 = "", string value3 = "", string field4 = "", string value4 = "", string field5 = "", string value5 = "", string field6 = "", string value6 = "", string field7 = "", string value7 = "", string field8 = "", string value8 = "", string field9 = "", string value9 = "")
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd;

            string query = "";

            //executing query
            try
            {
                cmd = new MySqlCommand(query, con); cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                Console.WriteLine("Error!");
            }
        }

        public static void insertTo(MySqlConnection con, string db, string table, string field, string value, string field2 = "", string value2 = "", string field3 = "", string value3 = "", string field4 = "", string value4 = "", string field5 = "", string value5 = "", string field6 = "", string value6 = "", string field7 = "", string value7 = "", string field8 = "", string value8 = "", string field9 = "", string value9 = "")
        {

            MySqlCommand cmd;
            string query = "INSERT INTO `" + db + "`.`" + table + "` (`" + field + "`";
            if (field2 != "") { query += ", `" + field2 + "`"; }
            if (field3 != "") { query += ", `" + field3 + "`"; }
            if (field4 != "") { query += ", `" + field4 + "`"; }
            if (field5 != "") { query += ", `" + field5 + "`"; }
            if (field6 != "") { query += ", `" + field6 + "`"; }
            if (field7 != "") { query += ", `" + field7 + "`"; }
            if (field8 != "") { query += ", `" + field8 + "`"; }
            if (field9 != "") { query += ", `" + field9 + "`"; }
            query += ") VALUES ( '" + value + "'";
            if (field2 != "") { query += ", '" + value2 + "'"; }
            if (field3 != "") { query += ", '" + value3 + "'"; }
            if (field4 != "") { query += ", '" + value4 + "'"; }
            if (field5 != "") { query += ", '" + value5 + "'"; }
            if (field6 != "") { query += ", '" + value6 + "'"; }
            if (field7 != "") { query += ", '" + value7 + "'"; }
            if (field8 != "") { query += ", '" + value8 + "'"; }
            if (field9 != "") { query += ", '" + value9 + "'"; }
            query += ");";

            //executing query
            try
            {
                cmd = new MySqlCommand(query, con); cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error!");
            }



        }

        public static void insert(string db, string table, string field, string value, string field2 = "", string value2 = "", string field3 = "", string value3 = "", string field4 = "", string value4 = "", string field5 = "", string value5 = "", string field6 = "", string value6 = "", string field7 = "", string value7 = "", string field8 = "", string value8 = "", string field9 = "", string value9 = "")
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd;

            string query = "INSERT INTO `" + db + "`.`" + table + "` (`" + field + "`";
            if (field2 != "") { query += ", `" + field2 + "`"; }
            if (field3 != "") { query += ", `" + field3 + "`"; }
            if (field4 != "") { query += ", `" + field4 + "`"; }
            if (field5 != "") { query += ", `" + field5 + "`"; }
            if (field6 != "") { query += ", `" + field6 + "`"; }
            if (field7 != "") { query += ", `" + field7 + "`"; }
            if (field8 != "") { query += ", `" + field8 + "`"; }
            if (field9 != "") { query += ", `" + field9 + "`"; }
            query += ") VALUES ( '" + value + "'";
            if (field2 != "") { query += ", '" + value2 + "'"; }
            if (field3 != "") { query += ", '" + value3 + "'"; }
            if (field4 != "") { query += ", '" + value4 + "'"; }
            if (field5 != "") { query += ", '" + value5 + "'"; }
            if (field6 != "") { query += ", '" + value6 + "'"; }
            if (field7 != "") { query += ", '" + value7 + "'"; }
            if (field8 != "") { query += ", '" + value8 + "'"; }
            if (field9 != "") { query += ", '" + value9 + "'"; }
            query += ");";

            //executing query
            try
            {
                cmd = new MySqlCommand(query, con); cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                Console.WriteLine("Error!");
            }



        }
    }
}
