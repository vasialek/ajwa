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
            string query = "SELECT * FROM `nova`.`crm_tv_res_services` LIMIT " + start + ", " + count + "";
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

        static string getUzIdByTmpId(string value, MySqlConnection con)
        {
            //MySqlConnection con = new MySqlConnection(connectionString);
            //con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "aiva", "uz", "tmpId", value);

            while (data.Read())
            {
                returnval = data["uzid"].ToString();
            }
            data.Close();
            //con.Close();
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

            int start_position = 13834; //12197 //12743 //13289 //13834
            //max - 260484 
            int fields_count = 100000;
            int startTid = 189506; //187274 //188027 //188776 //189506

            count = start_position;

            MySqlConnection conWrite = new MySqlConnection(connectionStringDest);
            MySqlConnection conRead = new MySqlConnection(connectionString);
            MySqlConnection conRead2 = new MySqlConnection(connectionString);
            MySqlConnection conRead3 = new MySqlConnection(connectionString);
            MySqlConnection conRead4 = new MySqlConnection(connectionString);

            conWrite.Open();
            conRead.Open();
            conRead2.Open();
            conRead3.Open();
            conRead4.Open();

        
            
           
            if (OpenConnection()) 
            { 
                Console.WriteLine("Connection successful");
                Console.WriteLine();

                //used for phases 1, 2, 3
                //MySqlDataReader data = res_main_processing(start_position, fields_count);
                
                //used for phase 4
                //MySqlDataReader data = res_customers_processing(start_position, fields_count);

                //used for phase5
                //MySqlDataReader data = refadr_processing(start_position.ToString(), fields_count.ToString());

                //extra phase
                //MySqlDataReader data = turuz_charak_processing(start_position, fields_count);

                //turuz processing
                //MySqlDataReader data = turuz_processing(start_position, fields_count);

                //additional pahase
                //MySqlDataReader data = turuz_tv_res_services_processing(start_position, fields_count);

                //extra services
                //MySqlDataReader data = turuz_tv_res_extra_services_processing(start_position, fields_count);

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

                        /*
                         * ===========================================================================
                         * =========================== U Z ===========================================
                         * ================================================ 1 phase ==================
                         */

                        /*
                        string tmp_id = data["res_no"].ToString();
                        DateTime uz_dat = (DateTime)data["res_created"];
                        string uz_protect = "F" + rnd.Next(10, 99).ToString() + rnd.Next(1000, 9999).ToString();
                        insert("aiva", "uz", "uznum", count.ToString(), "uzdat", uz_dat.ToString("yyyy-MM-dd HH:mm"), "tmpId", tmp_id, "protect", uz_protect);
                         */


                        /*
                         * ===========================================================================
                         * ========================= T U R U Z =======================================
                         * ================================================ 2 phase ==================
                         */

                        /*
                        string tkail = (double.Parse(data["sale_price"].ToString()) * 10000).ToString();
                        string gcent = (double.Parse(data["net_price"].ToString()) * 10000).ToString();
                        string uz_id = getUzIdByTmpId(data["res_no"].ToString());
                        string pr_id = getPrIdFormGoodsvByGkodas(data["holiday_package_id"].ToString());
                    
                        insert("aiva", "turuz", "uzid", uz_id, "prid", pr_id, "tkail", tkail, "gcent", gcent);
                         */



                        /* 
                         * ==========================================================================
                         * ================== T U R U Z  C H A R A K ================================
                         * ============================================== 3 phase ===================
                         */

                        /*
                        string holiday_p_id = data["holiday_package_id"].ToString();
                        string res_no = data["res_no"].ToString();

                        string prid = getPrIdFormGoodsvByGkodas(holiday_p_id);
                        string tid = getTidByUzId(getUzIdByTmpId(res_no));

                        string beginDate = data["begin_date"].ToString();
                        string endDate = data["end_date"].ToString();
                        string duration = data["duration"].ToString();
                        string saleResourceId = data["sale_resource_id"].ToString();

                        insert("aiva", "turuz_charak", "tid", tid, "chid", "2", "val", beginDate);
                        insert("aiva", "turuz_charak", "tid", tid, "chid", "3", "val", endDate);
                        insert("aiva", "turuz_charak", "tid", tid, "chid", "4", "val", duration);
                        insert("aiva", "turuz_charak", "tid", tid, "chid", "5", "val", saleResourceId);
                         */

                        /* 
                         * ==========================================================================
                         * ========================== R E F A D R ===================================
                         * ============================================== 4 phase ===================
                         */

                        /*
                        if (data["sequence_no"].ToString() == "1")
                        {
                            string tmp_id = data["cust_no"].ToString();
                            string name = data["name"].ToString();
                            string surname = data["surname"].ToString();
                            string adr_gat = data["home_address"].ToString();
                            string adr_miest = data["home_city"].ToString();
                            string adr_tel = data["home_phone"].ToString();
                            string adr_fax = data["home_fax"].ToString();
                            string adr_mail = data["home_email"].ToString();
                            string adr_org = data["work_company"].ToString();

                            string name_surname = name + " " + surname;

                            insert("aiva", "refadr", "adrpav", name_surname, "adrgat", adr_gat, "adrmiest", adr_miest, "adrmail", adr_mail, "adrtel", adr_tel, "adrfax", adr_fax, "adrvar", name, "adrorg", adr_org, "tmpId", tmp_id);
                        }
                        */


                        // -- U P D A T I N G  R E F A D R --

                        /*
                        string cust_no = data["cust_no"].ToString();

                        string adr_pav = data["surname"].ToString();
                        string id_doc_serie = data["id_doc_serie"].ToString();
                        string id_doc_no = data["id_doc_no"].ToString();
                        string id_doc_issue_date = data["id_doc_issue_date"].ToString();
                        string id_doc_expire_date = data["id_doc_expire_date"].ToString();
                        string id_doc_given = data["id_doc_given"].ToString();
                        string person_id = data["person_id"].ToString();
                        string age = data["age"].ToString();

                        string title_id = data["title_id"].ToString();
                        values = getCharakByNameAndKodas("Titles", title_id);
                        string title = values[0];

                        DateTime birthday;
                        try
                        {
                            birthday = (DateTime)data["birthday"];
                            update(conWrite, cust_no, id_doc_serie, id_doc_no, id_doc_issue_date, id_doc_expire_date, id_doc_given, title, person_id, age, birthday.ToString("yyyy-MM-dd HH:mm"), adr_pav);
                        }
                        catch
                        {
                            update(conWrite, cust_no, id_doc_serie, id_doc_no, id_doc_issue_date, id_doc_expire_date, id_doc_given, title, person_id, age, "", adr_pav);
                        }
                        */


                        //Console.WriteLine(title);

                        /* 
                        * ==========================================================================
                        * ======================== U Z S A K O V A I ===============================
                        * ============================================== 5 phase ===================
                        */

                        /*
                        string adr_id = data["adrid"].ToString();
                        string username = data["addrpav"].ToString();
                        string at_e = data["adrmail"].ToString();
                        string at_t = data["adrtel"].ToString();
                        string at_v = data["adrvar"].ToString();

                        string at_p = username.Replace(at_v, "");
                        at_p = at_p.Replace(" ", "");

                        string tipas = "2";

                        insert("aiva", "uzsakovai", "adrid", adr_id, "tipas", tipas, "username", username, "at_p", at_p, "at_e", at_e, "at_v", at_v, "at_t", at_t);
                        */



                        /*
                         * =========================================================================
                         * ===================== R E S  M A I N  C H A R A C T =====================
                         * =========================================================================
                         */

                        /*
                         * 
                        string res_no = data["res_no"].ToString();

                        string tid = getTidByUzId(getUzIdByTmpId(res_no,conRead));
                        //string tid = (count + 100 + 1).ToString(); //boost (shity but quick solution)

                        string holiday_p_id = data["holiday_package_id"].ToString();
                        string prid = getPrIdFormGoodsvByGkodas(holiday_p_id);

                        string sale_resource_id = data["sale_resource_id"].ToString(); //chid - 5, name - Sale resources
                        string market_pl_id = data["market_pl_id"].ToString(); // chid - 15, name - Markets
                        string operator_pl_id = data["operator_pl_id"].ToString(); //chid - 14, name - Operators

                        string beginDate = data["begin_date"].ToString(); //chid - 2
                        string endDate = data["end_date"].ToString(); //chid - 3
                        string duration = data["duration"].ToString(); //chid - 4

                        string res_note = data["res_note"].ToString(); //chid 6
                        string int_note = data["int_note"].ToString(); //chid 7
                        string adults = data["adults"].ToString(); //chid 8
                        string childs = data["childs"].ToString(); //chid 9

                        string payment_status_code = data["payment_status_code"].ToString(); //chid 10
                        string early_booking = data["early_booking"].ToString();//chid 11

                        string deaprture_city_id = data["departure_city_id"].ToString(); //chid 12
                        string arrival_city_id = data["arrival_city_id"].ToString(); //chid 13

                        string pack_type_code = data["pack_type_code"].ToString(); //chid 16


                        // complex charact
                        values = getCharakByNameAndKodas("Sale resources", sale_resource_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "5", "val", value, "val_prid", val_prid);

                        values = getCharakByNameAndKodas("Markets", market_pl_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "15", "val", value, "val_prid", val_prid);

                        values = getCharakByNameAndKodas("Operators", operator_pl_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "14", "val", value, "val_prid", val_prid);

                        values = getCharakByNameAndKodas("Locations", deaprture_city_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "12", "val", value, "val_prid", val_prid);

                        values = getCharakByNameAndKodas("Locations", arrival_city_id);
                        value = values[0]; val_prid = values[1];
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "13", "val", value, "val_prid", val_prid);

                        // simly charact
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "6", "val", res_note);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "7", "val", int_note);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "8", "val", adults);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "9", "val", childs);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "10", "val", payment_status_code);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "11", "val", early_booking);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "16", "val", pack_type_code);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "2", "val", beginDate);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "3", "val", endDate);
                        insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "4", "val", duration);
                    
                         */

                        /*
                         * =========================================================================
                         * ============= M O S T  P O W E R F U L L  B R A I N F U C K =============
                         * =========================================== extra services ==============
                         */


                        //---------------------- E X T R A  S E R V I C E S ------------------------
                        //--------------------------------------------------------------------------


                        /*
                        string res_no = data["res_no"].ToString();
                        string res_service_id = data["res_service_id"].ToString();

                        string old_uzid = getUzIdByTmpId(res_no, conRead);
                        string parentTid = getAltTidByUzId(old_uzid);

                        //string old_prid = "";
                        string old_tkail = "";
                        string old_gcent = "";

                        // get all extra services
                        //MySqlDataReader data_extra_serv = get_extra_services_by_res_no(res_no,conRead2);
                        
                        MySqlDataReader data_extra_serv = get_extra_services_by_res_service_id(res_service_id, conRead2);

                        Console.WriteLine("current res_service_id: " + res_service_id);
                        Console.WriteLine("-------------------------");

                        while (data_extra_serv.Read())
                        {
                            string current_service_type_id = data_extra_serv["service_type_id"].ToString();
                            string current_extras_service_id = data_extra_serv["extra_service_id"].ToString();
                       
                            //values = getCharakByNameAndKodas("Service types", current_service_type_id);
                            values = getCharakByNameAndKodas("Extra services", current_extras_service_id);
                            value = values[0]; val_prid = values[1];

                            string begin_date = data_extra_serv["begin_date"].ToString(); //chid  21
                            string end_date = data_extra_serv["end_date"].ToString(); //chid 22
                            string duration = data_extra_serv["duration"].ToString(); //chid 23

                            string current_res_no = data_extra_serv["res_no"].ToString();
   
                            //insertTo(conWrite, "turas", "turuz", "uzid", old_uzid, "prid", val_prid, "tkail", old_tkail, "gcent", old_gcent, "parentTid", parentTid, "tkiek", "1");
                            startTid++;

                            Console.WriteLine("found width res_service_id: " + res_service_id);
                            Console.WriteLine("Parent tid : " + parentTid);
                            Console.WriteLine("New tid : " + startTid);

                            //insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "18", "val", value, "val_prid", val_prid);

                            //insert simple charackteristics
                            //insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "21", "val", begin_date);
                            //insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "22", "val", end_date);
                            //insertTo(conWrite, "turas", "turuz_charak", "tid", startTid.ToString(), "chid", "23", "val", duration);

                        }

                         
 
                        data_extra_serv.Close();
                         */




                        /* 
                        * ==========================================================================
                        * ========================== B R A I N F U C K =============================
                        * ============================================== X phase ===================
                        */

                        //-------------------------- S E R V I C E S -------------------------------
                        //--------------------------------------------------------------------------


                        string res_no = data["res_no"].ToString();
                        string uz_id = getUzIdByTmpId(res_no, conRead);
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
                        //=============================== Create extra services ================================================

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


                        //Console.WriteLine("Res_no - " + res_no);
                        //Console.WriteLine("Uz_id - " + uz_id);
                        //Console.WriteLine("Old tid - " + currentTid);
                        //Console.WriteLine("New tid - " + startTid);
                        //Console.WriteLine("Parent Tid - 0");
                        //Console.WriteLine("Prid - " + prid);
                        //Console.WriteLine("========================================");




                        count++;
                        Console.WriteLine("record completed - " + count + " tid = " + startTid);
                    }
                }
                catch (Exception ex)
                {
                    string s = string.Format("TID: {0}, count: {1}", startTid, count);
                    s += ex.ToString();
                    System.IO.File.WriteAllText("D:\\turasimport.log", s);
                    Console.Beep();
                    Console.Beep();
                    Console.Beep();
                    throw ex;
                }
            }
            conWrite.Close();
            Console.WriteLine("Finished at - " + System.DateTime.Now.ToLongTimeString());
            Console.Read();
        }
    }
}
