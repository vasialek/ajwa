using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace ConsoleApplication1
{
    class Program : ProgramMain
    {
        static int count = 0;

        /* =====================================================================================
         * ============== F U N C T I O N S  F O R  P H A S E S  O F  I M P O R T ==============
         * ===================================================================================== */

        static MySqlDataReader res_main_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_main` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader res_customers_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_customers` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader refadr_processing(string start, string count)
        {
            //query
            string query = "SELECT * FROM `aiva`.`refadr` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader uz_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `aiva`.`uz` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader turuz_charak_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `aiva`.`turuz_charak` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader turuz_tv_res_services_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_services` WHERE `sequence_no` = '1' LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);


            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader turuz_tv_res_services_processing_not_using_1(int start, int count)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_services` LIMIT  " + start + ", " + count + ";";
            MySqlCommand cmd = new MySqlCommand(query, connection);


            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        

        static MySqlDataReader turuz_tv_res_extra_services_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_services` WHERE `service_type_id` != '6' LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader turuz_processing(int start, int count)
        {
            //query
            string query = "SELECT * FROM `aiva`.`turuz` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }


        /* =====================================================================================
         * ================== G E T  V A L U E S  B Y  O T H E R  V A L U E S ==================
         * ===================================================================================== */


        static MySqlDataReader get_extra_services_by_res_no(string res_no, MySqlConnection con)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_extra_services` WHERE `res_no` = '" + res_no + "' AND `service_type_id` != '6'";
            MySqlCommand cmd = new MySqlCommand(query, con);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }


        static MySqlDataReader get_extra_services_by_res_service_id(string service_id, MySqlConnection con)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_extra_services` WHERE `res_service_id` = '" + service_id + "'";
            MySqlCommand cmd = new MySqlCommand(query, con);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        static MySqlDataReader getTuruzValuesByTid(string tid, MySqlConnection con)
        {
            //query
            string query = "SELECT * FROM `aiva`.`turuz` WHERE `tid` = '" + tid + "'";
            MySqlCommand cmd = new MySqlCommand(query, con);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        }


        static string[] getCharakByNameAndKodas(string name, string kodas)
        {

            string[] result = { "", "" };

            
            MySqlConnection con = new MySqlConnection(connectionStringAlt);
            con.Open();

            //query
            string query = "SELECT pr_z.pr_pavad as 'name',g_v.prid, g_v.g_kodas as 'kodas' FROM pg_zodynas INNER JOIN pg_seima on pg_seima.pg_id_v=pg_zodynas.pg_id INNER JOIN goods_v g_v on g_v.pgs_id = pg_seima.pgs_id INNER JOIN pr_zodynas AS pr_z ON g_v.pr_id=pr_z.pr_id AND g_v.del_date IS NULL WHERE pg_zodynas.pavaddgs = '" + name + "'";
            MySqlCommand cmd = new MySqlCommand(query, con);

            //executing query
            MySqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                if (data["kodas"].ToString() == kodas)
                {
                    result[0] = data["name"].ToString();
                    result[1] = data["prid"].ToString();
                }
            }

            data.Close();
            con.Close();

            return result;
        }

        static string getResNoByTmpId(string value, MySqlConnection con)
        {
            //MySqlConnection con = new MySqlConnection(connectionString);
            //con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "nova", "crm_tv_res_customers", "cust_no", value);

            while (data.Read())
            {
                returnval = data["res_no"].ToString();
            }
            data.Close();
            //con.Close();
            return returnval;
        }

        static string getPrIdFormGoodsvByGkodas(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string returnval = "";
            MySqlDataReader data = select(con,"aiva", "goods_v", "pgs_id", "18", "g_kodas", value);

            while (data.Read())
            {
                returnval = data["prid"].ToString();
            }
            data.Close();
            con.Close();
            return returnval;
        }

        static string getUzIdByTmpId(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "aiva", "uz", "tmpId", value);

            while (data.Read())
            {
                returnval = data["uzid"].ToString();
            }
            data.Close();
            con.Close();
            return returnval;
        }

        static string getTidByUzId(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "aiva", "turuz", "uzid", value);

            while (data.Read())
            {
                returnval = data["tid"].ToString();
            }
            data.Close();
            con.Close();
            return returnval;
        }

        static string getAltTidByUzId(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionStringDest);
            con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "turas", "turuz", "uzid", value);

            while (data.Read())
            {
                returnval = data["tid"].ToString();
            }
            data.Close();
            con.Close();
            return returnval;
        }


        static void update(MySqlConnection con, string cust_no, string id_doc_serie, string id_doc_no, string id_doc_issue_date, string id_doc_expire_date, string id_doc_given, string title, string person_id, string age, string birthaday, string adr_pav)
        {
            MySqlCommand cmd;
            string query = "UPDATE `turas`.`refadr` SET `id_doc_serie`='" + id_doc_serie + "', `id_doc_no`='" + id_doc_no + "', `id_doc_issue_date`='" + id_doc_issue_date + "', `id_doc_expire_date`='" + id_doc_expire_date + "', `id_doc_given`='" + id_doc_given + "', `cust_title`='" + title + "', `person_id`='" + person_id + "', `age`='" + age + "', `birthday`='" + birthaday + "', `adrpav`='" + adr_pav + "'   WHERE `tmpId` = '" + cust_no + "';";

            try
            {
                cmd = new MySqlCommand(query, con); cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error!");
            }
        }

        static void updateParentTid(MySqlConnection con, string tid)
        {
            MySqlCommand cmd;
            string query = "UPDATE `turas`.`turuz` SET `parentTid`='" + tid + "' WHERE `tid` = '" + tid + "';";

            try
            {
                cmd = new MySqlCommand(query, con); cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Error!");
            }
        }



       /* =====================================================================================
        * =========================== M A I N  P H A S E S ====================================
        * ===================================================================================== */

        static void Main(string[] args)
        {
            Console.WriteLine("D.W. importer 1.0");
            Console.WriteLine("----------------------");
            init();
            Console.WriteLine();

            int start_position = 0;
            int fields_count = 200000;
            int startTid = 170347;

            Console.WriteLine("Start postion - ");
            start_position = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Limit - ");
            fields_count = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Start tid - ");
            startTid = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please wait. Connecting...");

            count = start_position;

            MySqlConnection conWrite = new MySqlConnection(connectionStringDest);
            MySqlConnection conRead = new MySqlConnection(connectionString);
            MySqlConnection conRead2 = new MySqlConnection(connectionString);
            MySqlConnection conRead3 = new MySqlConnection(connectionString);
            MySqlConnection conRead4 = new MySqlConnection(connectionString);

            try { conWrite.Open(); }
            catch { Console.WriteLine("Error in connection to remote sql server!"); }

            try
            {
                conRead.Open();
                conRead2.Open();
                conRead3.Open();
                conRead4.Open();
            }
            catch { Console.WriteLine("Error in connection to local mysql server!"); }

            if (OpenConnection())
            {
                Console.WriteLine("Connection successful");
                Console.WriteLine();

                MySqlDataReader data = turuz_tv_res_services_processing_not_using_1(start_position, fields_count);

                //for random values
                Random rnd = new Random();

                //main read loop

                string value = "";
                string val_prid = "";
                string[] values = { "", "" };


                try
                {
                    while (data.Read())
                    {

                        //-------------------------- S E R V I C E S -------------------------------
                        //--------------------------------------------------------------------------


                        string res_no = data["res_no"].ToString();
                        string uz_id = getUzIdByTmpId(res_no);
                        string currentTid = getTidByUzId(uz_id);
                        string prid = "";

                        
                        string service_type_id = data["service_type_id"].ToString();  //Service types, chid - 18
                        string arrival_location_id = data["arrival_location_id"].ToString(); // Locations, chid - 20
                        string departure_location_id = data["departure_location_id"].ToString(); //Locations, chid - 19

                        string room_group_id = data["room_group_id"].ToString(); // Rooms, chid - 24
                        string accom_id = data["accom_id"].ToString(); // Accommodations, chid - 25
                        string board_group_id = data["board_group_id"].ToString(); // Boarding groups, chid - 26

                        string begin_date = data["begin_date"].ToString(); // chid - 21
                        string end_date = data["end_date"].ToString(); // chid - 22
                        string duration = data["duration"].ToString(); // chid - 23

                        values = getCharakByNameAndKodas("Service types", service_type_id);
                        value = values[0]; val_prid = values[1];
                        prid = val_prid;


                        //create in turuz record
                        insertTo(conWrite, "turas", "turuz", "uzid", uz_id, "prid", val_prid, "tkail", "0", "gcent", "0", "parentTid", "0", "tkiek", "1");
                        startTid++;


                        //input in charak using  new tid

                        insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "18", "val", value, "val_prid", val_prid);

                        values = getCharakByNameAndKodas("Locations", arrival_location_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "20", "val", value, "val_prid", val_prid);

                        values = getCharakByNameAndKodas("Locations", departure_location_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "19", "val", value, "val_prid", val_prid);


                        if (service_type_id == "6")
                        {
                            values = getCharakByNameAndKodas("Rooms", room_group_id);
                            value = values[0]; val_prid = values[1];
                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "24", "val", value, "val_prid", val_prid);

                            values = getCharakByNameAndKodas("Accommodations", accom_id);
                            value = values[0]; val_prid = values[1];
                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "25", "val", value, "val_prid", val_prid);

                            values = getCharakByNameAndKodas("Boarding groups", board_group_id);
                            value = values[0]; val_prid = values[1];
                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "26", "val", value, "val_prid", val_prid);
                        }


                        insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "21", "val", begin_date);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "22", "val", end_date);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "23", "val", duration);




                        //======================================================================================================
                        //================================ Create extra service ================================================

                        string res_service_id = data["res_service_id"].ToString();
                        MySqlDataReader data_extra_serv = get_extra_services_by_res_service_id(res_service_id, conRead4);
                        string parent = startTid.ToString();

                        while (data_extra_serv.Read())
                        {
                            string current_service_type_id = data_extra_serv["service_type_id"].ToString();
                            string current_extras_service_id = data_extra_serv["extra_service_id"].ToString();

                            //values = getCharakByNameAndKodas("Service types", current_service_type_id);
                            values = getCharakByNameAndKodas("Extra services", current_extras_service_id);
                            value = values[0]; val_prid = values[1];

                            string begin_date_ex = data_extra_serv["begin_date"].ToString(); //chid  21
                            string end_date_ex = data_extra_serv["end_date"].ToString(); //chid 22
                            string duration_ex = data_extra_serv["duration"].ToString(); //chid 23

                            string current_res_no = data_extra_serv["res_no"].ToString();

                            insertTo(conWrite, "turas", "turuz", "uzid", uz_id, "prid", val_prid, "tkail", "", "gcent", "", "parentTid", parent, "tkiek", "1");
                            startTid++;

                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "18", "val", value, "val_prid", val_prid);

                            //insert simple charackteristics
                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "21", "val", begin_date);
                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "22", "val", end_date);
                            insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "23", "val", duration);

                        }
                        data_extra_serv.Close();
                        

                        count++;
                        Console.WriteLine("record completed = " + count + "; tid = " + startTid + ";" + " uzid = " + uz_id);
                    }
                }
                catch
                {
                    Console.WriteLine("Reading error! Last count = " + count + ";" + " tid = " + startTid + ";");
                    Console.WriteLine("Please restart with this params");
                    //TODO: auo restarting program with params
                }
            }
            else
            {
                Console.WriteLine("Can't connect to local mysql server!");
            }
            conWrite.Close();
            Console.WriteLine("Finished at - " + System.DateTime.Now.ToLongTimeString());
            Console.Read();
        }
    }
}
