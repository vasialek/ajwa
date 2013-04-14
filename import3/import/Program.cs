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
            string query = "SELECT * FROM `nova`.`crm_tv_res_customers` WHERE `sequence_no` = '1' LIMIT " + start + ", " + count + "";
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

        static MySqlDataReader turuz_crm_tv_res_supplements_discounts(int start, int count)
        {
            //query
            string query = "SELECT * FROM `nova`.`crm_tv_res_supplements_discounts` LIMIT " + start + ", " + count + "";
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
                {
                    result[0] = data["name"].ToString();
                    result[1] = data["prid"].ToString();
                }
            }

            data.Close();
            con.Close();

            return result;
        }

        static string[] getCharakByNameAndPrid(string name, string prid)
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
                if (data["prid"].ToString() == prid)
                {
                    result[0] = data["name"].ToString();
                    result[1] = data["kodas"].ToString();
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

        static string[] getPrIdFormGoodsvByGkodas2(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            string[] pr_ids = {"","","","","","","","","","","","","","","","","",""};
            MySqlDataReader data = select(con, "aiva", "goods_v", "g_kodas", value);

            int curr_index = 0;
            while (data.Read())
            {
                pr_ids[curr_index] = data["prid"].ToString();
                
                curr_index++;
            }
            data.Close();
            con.Close();

            return pr_ids;
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

        static string getGKodasByResNoFromResMain(string value)
        {
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();

            string returnval = "";
            MySqlDataReader data = select(con, "nova", "crm_tv_res_main", "res_no", value);

            while (data.Read())
            {
                returnval = data["holiday_package_id"].ToString();
            }
            data.Close();
            con.Close();
            return returnval;
        }

        static void update(MySqlConnection con, string cust_no, string id_doc_serie, string id_doc_no, string id_doc_issue_date, string id_doc_expire_date, string id_doc_given, string title)
        {
            MySqlCommand cmd;
            string query = "UPDATE `turas`.`refadr` SET `id_doc_serie`='" + id_doc_serie + "', `id_doc_no`='" + id_doc_no + "', `id_doc_issue_date`='" + id_doc_issue_date + "', `id_doc_expire_date`='" + id_doc_expire_date + "', `id_doc_given`='" + id_doc_given + "', `cust_title`='" + title + "'   WHERE `tmpId` = '" + cust_no + "';";

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
            //max - 53900 (215600)
            int fields_count = 10;
            count = start_position;

            MySqlConnection conWrite = new MySqlConnection(connectionStringDest);
            conWrite.Open();
   

            if (OpenConnection()) 
            { 
                Console.WriteLine("Connection successful");
                Console.WriteLine();

                MySqlDataReader data = turuz_crm_tv_res_supplements_discounts(start_position, fields_count);

                //for random values
                Random rnd = new Random();

                //main read loop

                string[] values = { "", "" };


                while (data.Read())
                {
                    //debugvar(data);

                    //string cust_no = data["cust_no"].ToString();

                    //string id_doc_serie = data["id_doc_serie"].ToString();
                    //string id_doc_no = data["id_doc_no"].ToString();
                    //string id_doc_issue_date = data["id_doc_issue_date"].ToString();
                    //string id_doc_expire_date = data["id_doc_expire_date"].ToString();
                    //string id_doc_given = data["id_doc_given"].ToString();

                    //string title_id = data["title_id"].ToString();

                    //values = getCharakByNameAndKodas("Titles", title_id);
                    //string title = values[0];
                    //update(conWrite, cust_no, id_doc_serie, id_doc_no, id_doc_issue_date, id_doc_expire_date, id_doc_given, title);

                    /*
                     * =================================================================================
                     * ============================= M I N D F U C K  2 ================================
                     * =================================================================================
                     */

                    //debugvar(data);

                    string tid = getTidByUzId(getUzIdByTmpId(data["res_no"].ToString()));
                    string formated_string = "";
                    string g_kodas = getGKodasByResNoFromResMain(data["res_no"].ToString());
                    string[] prids = getPrIdFormGoodsvByGkodas2(g_kodas);

                    string ammount = data["amount"].ToString();
                    string percentage_value = data["percentage_value"].ToString();

                    for (int i = 0; i < prids.Length; i++)
                    {
                        if (prids[i] != "")
                        {
                            string prid_current = prids[i];
                            values = getCharakByNameAndPrid("Supplements and discounts",prid_current);
                            string name_current = values[0];

                            if (formated_string != "") { formated_string = formated_string + "/"; }
                            formated_string += name_current + " " + ammount + "(" + percentage_value + ")";

                        }
                        
                    }


                    //Console.WriteLine("tid: " + tid);
                    //Console.WriteLine("val: " + formated_string);
                    //Console.WriteLine("chid: 27");
                    //Console.WriteLine("val_prid:" + prids[i]);
                    //Console.WriteLine("========================================================");

                    insertTo(conWrite, "turas", "turuz_charak", "val", formated_string, "chid", "27", "tid", tid);


                    //string supplement_disscount_id = data["supplement_discount_id"].ToString();
                    //string res_supplement_disscount_id = data["res_supplement_discount_id"].ToString();


                    //values = getCharakByNameAndKodas("Supplements and discounts", supplement_disscount_id);
                    //string name = values[0];
                    //string prid = values[1];

                    //values = getCharakByNameAndKodas("Supplements and discounts", res_supplement_disscount_id);
                    //string name2 = values[0];
                    //string prid2 = values[1];
                    
                    //Console.WriteLine("ammount: " + ammount + "; percentage_value: " + percentage_value + "; supplement_disscount_id: " + supplement_disscount_id + "; name: " + name);

                    //string formated_string = name + " " + ammount + "(" + percentage_value + ")";

                    //Console.WriteLine("tid: " + tid);
                    //Console.WriteLine("val: " + formated_string);
                    //Console.WriteLine("chid: 27");
                    //Console.WriteLine("val_prid:" + prid);
                    //Console.WriteLine("========================================================");

                    //Console.WriteLine(prid);
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
