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

        static MySqlDataReader refadr_processing(int start, int count)
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
            string query = "SELECT * FROM `nova`.`crm_tv_res_services` LIMIT " + start + ", " + count + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //executing query
            MySqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader;
        } 


        /* =====================================================================================
         * ================== G E T  V A L U E S  B Y  O T H E R  V A L U E S ==================
         * ===================================================================================== */



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
                    result[0] = data["name"].ToString();
                {
                    result[1] = data["prid"].ToString();
                }
            }

            data.Close();
            con.Close();

            return result;
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


       /* =====================================================================================
        * =========================== M A I N  P H A S E S ====================================
        * ===================================================================================== */

        static string getSequenceIdByCustNo(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "nova", "crm_tv_res_customers", "cust_no", value);

            while (data.Read())
            {
                returnval = data["sequence_no"].ToString();
            }
            data.Close();
            con.Close();
            return returnval;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("D.W. importer 1.0");
            Console.WriteLine("----------------------");
            init();
            Console.WriteLine();

            int start_position = 78830;
            //max - 53900 (215600)
            int fields_count = 200000;
            count = start_position;

            MySqlConnection conWrite = new MySqlConnection(connectionStringDest);
            conWrite.Open();
   

            if (OpenConnection()) 
            { 
                Console.WriteLine("Connection successful");
                Console.WriteLine();

                //used for phases 1, 2, 3
                //MySqlDataReader data = res_main_processing(start_position, fields_count);
                
                //used for phase 4
                //MySqlDataReader data = res_customers_processing(start_position, fields_count);

                //used for phase5
                MySqlDataReader data = refadr_processing(start_position, fields_count);

                //extra phase
                //MySqlDataReader data = turuz_charak_processing(start_position, fields_count);

                //additional pahase
                //MySqlDataReader data = turuz_tv_res_services_processing(start_position, fields_count);



                //for random values
                Random rnd = new Random();

                //main read loop

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
                    if (data["sequence_no"].ToString() != "1")
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
                        //insertTo(conWrite, "turas", "refadr", "adrpav", name_surname, "adrgat", adr_gat, "adrmiest", adr_miest, "adrmail", adr_mail, "adrtel", adr_tel, "adrfax", adr_fax, "adrvar", name, "adrorg", adr_org, "tmpId", tmp_id);
                    }
                    */


                    /* 
                    * ==========================================================================
                    * ======================== U Z S A K O V A I ===============================
                    * ============================================== 5 phase (alternative) =====
                    */



                    /* 
                    * ==========================================================================
                    * ======================== U Z S A K O V A I ===============================
                    * ============================================== 5 phase ===================
                    */


                    //start addrid - 53816

                    string cust_no = data["tmpId"].ToString();
                    string adr_id = data["adrid"].ToString();
                    string username = data["adrpav"].ToString();
                    string at_e = data["adrmail"].ToString();
                    string at_t = data["adrtel"].ToString();
                    string at_v = data["adrvar"].ToString();
                    string at_p = "";

                    try
                    {
                        at_p = username.Replace(at_v, "");
                        at_p = at_p.Replace(" ", "");
                    }
                    catch
                    {
                        at_p = "";
                    }


                    string sequence_id = getSequenceIdByCustNo(cust_no);

                    string tipas = "2";

                    
                    if (sequence_id != "1")
                    {
                        //insert("aiva", "uzsakovai", "adrid", adr_id, "tipas", tipas, "username", username, "at_p", at_p, "at_e", at_e, "at_v", at_v, "at_t", at_t);
                        insertTo(conWrite, "turas", "uzsakovai", "adrid", adr_id, "tipas", tipas, "username", username, "at_p", at_p, "at_e", at_e, "at_v", at_v, "at_t", at_t);
                        Console.Write("Added - ");
                    }
                    else
                    {
                        Console.Write("Ignored - ");
                    }
                    
                    

                    //insert("aiva", "uzsakovai", "adrid", adr_id, "tipas", tipas, "username", username, "at_p", at_p, "at_e", at_e, "at_v", at_v, "at_t", at_t);


                    /* 
                    * ==========================================================================
                    * ========================== B R A I N F U C K =============================
                    * ============================================== X phase ===================
                    */

                    //debugvar(data);
                    //string res_no = data["res_no"].ToString();

                    //string tid = getTidByUzId(getUzIdByTmpId(res_no));
                    //string tid = (count + 100 + 1).ToString(); //boost

                    //string holiday_p_id = data["holiday_package_id"].ToString();
                    //string prid = getPrIdFormGoodsvByGkodas(holiday_p_id);

                    //string value = "";
                    //string val_prid = "";
                    //string[] values = { "", "" };

                    //string sale_resource_id = data["sale_resource_id"].ToString(); //chid - 5, name - Sale resources
                    //string market_pl_id = data["market_pl_id"].ToString(); // chid - 15, name - Markets
                    //string operator_pl_id = data["operator_pl_id"].ToString(); //chid - 14, name - Operators

                    //string beginDate = data["begin_date"].ToString(); //chid - 2
                    //string endDate = data["end_date"].ToString(); //chid - 3
                    //string duration = data["duration"].ToString(); //chid - 4


                    //string res_note = data["res_note"].ToString(); //chid 6
                    //string int_note = data["int_note"].ToString(); //chid 7
                    //string adults = data["adults"].ToString(); //chid 8
                    //string childs = data["childs"].ToString(); //chid 9
                    //string payment_status_code = data["payment_status_code"].ToString(); //chid 10
                    //string early_booking = data["early_booking"].ToString();//chid 11

                    //string deaprture_city_id = data["departure_city_id"].ToString(); //chid 12
                    //string arrival_city_id = data["arrival_city_id"].ToString(); //chid 13
                    
                    //string pack_type_code = data["pack_type_code"].ToString(); //chid 16



                    //values = getCharakByNameAndKodas("Locations", deaprture_city_id);
                    //value = values[0]; val_prid = values[1];
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "12", "val", value, "val_prid", val_prid);

                    //values = getCharakByNameAndKodas("Locations", arrival_city_id);
                    //value = values[0]; val_prid = values[1];
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "13", "val", value, "val_prid", val_prid);


                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "6", "val", res_note);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "7", "val", int_note);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "8", "val", adults);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "9", "val", childs);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "10", "val", payment_status_code);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "11", "val", early_booking);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "16", "val", pack_type_code);



                    //values = getCharakByNameAndKodas("Sale resources", sale_resource_id);
                    //value = values[0]; val_prid = values[1];
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "5", "val", value, "val_prid", val_prid);


                    //values = getCharakByNameAndKodas("Markets", market_pl_id);
                    //value = values[0]; val_prid = values[1];
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "15", "val", value, "val_prid", val_prid);


                    //values = getCharakByNameAndKodas("Operators", operator_pl_id);
                    //value = values[0]; val_prid = values[1];
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "14", "val", value, "val_prid", val_prid);

                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "2", "val", beginDate);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "3", "val", endDate);
                    //insertTo(conWrite, "turas", "turuz_charak", "tid", tid, "chid", "4", "val", duration);

                    

                    //Console.WriteLine("tid - " + tid + "; chid - 16; val - " + value + "; val_prid - " + val_prid + ";");

                    count++;
                    Console.WriteLine("record completed " + count);
                }

            }
            conWrite.Close();
            Console.WriteLine("Finished at - " + System.DateTime.Now.ToLongTimeString());
            Console.Read();
        }
    }
}
