using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace talkoot_demo
{
    class Program
    {
        static void Main(string[] args)
        {
         
            Console.WriteLine("***  Hei! Tässä ohjelmassa voit syöttää talkootyötietojasi  ***");
            Console.WriteLine();

            Console.WriteLine("Anna pelaajanumerosi:");
            Console.WriteLine();

            string annettupelaajanumero = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("--- Hei pelaaja numerolla: " + annettupelaajanumero + " ---");
            Console.WriteLine();

            // Tietokantayhteyden luonti:

            Console.WriteLine("Aloitetaan tietokantayhteyden avaus...");

            string yhteysmerkkijono = "Server=localhost\\SQLEXPRESS;Database=talkoot;Trusted_Connection=True;";
            SqlConnection yhteys = new SqlConnection(yhteysmerkkijono);
            yhteys.Open();

            Console.WriteLine("Tietokantayhteys avattu onnistuneesti.");
            Console.WriteLine();


            string sql = "SELECT etunimi, sukunimi " +
                "FROM henkilot " +
                "WHERE pelaajanumero = '"+annettupelaajanumero+"'";                       // haetaan pejaajan tiedot
                     
            SqlCommand komento = new SqlCommand(sql, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            while (lukija.Read())
            {
                Console.WriteLine("*** Syötät pelaajan: " + lukija["etunimi"] + " " + lukija["sukunimi"] + " tietoja. ***");
            }
            Console.WriteLine();
            lukija.Close();

           
            Console.WriteLine("*** Olet tähän mennessä tehnyt talkootöitä: ***");         // haetaan pejaajan talkoopisteet
            Console.WriteLine();

            string sql2 = "SELECT talkootyot.talkoo_tekopva, talkootyot.talkoo_tyo, talkootyot.talkoo_pisteet " +
                "FROM henkilot, talkootyot " +
                "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";  

            SqlCommand komento2 = new SqlCommand(sql2, yhteys);
            SqlDataReader lukija2 = komento2.ExecuteReader();

            while (lukija2.Read())
            {
                
                Console.WriteLine("Talkootyö: " + lukija2["talkoo_tyo"] + " " + lukija2["talkoo_pisteet"] + " talkoopistettä " + "(" + lukija2["talkoo_tekopva"] + ")" );
            }
           

            lukija2.Close();

            string sql3 = "SELECT SUM(talkootyot.talkoo_pisteet) " +                    // lasketaan yhteen pejaajan talkoopisteet
                "FROM henkilot, talkootyot " +
                "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";
            
           SqlCommand komento3 = new SqlCommand(sql3, yhteys);
           object summa = komento3.ExecuteScalar();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("*** Talkootyöpisteesi yhteensä ovat: " + summa + " pistettä. ***"); ;  // tulostetaan olemassa olevien pisteiden summa


            komento3.Dispose();

            

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("*** Haluatko kirjata lisää talkoopisteitä? ***");           // uusien pisteiden kirjaus
            Console.WriteLine();
            Console.WriteLine("Anna vastauksesti muodossa: kyllä = 1 / ei = 2");
            Console.WriteLine();

            string syotettyarvo = Console.ReadLine();
            int valinta = int.Parse(syotettyarvo);

            if (valinta == 1)                                                              // tähän valinta kyllä/ei
            {
                Console.WriteLine();
                Console.WriteLine("Ole hyvä, syötä tiedot tekemistäsi talkootöistä:");     

                Console.WriteLine();
                Console.WriteLine("Mitä talkootyötä olet tehnyt:");                        // pisteiden syöttö
                string syotettytyo = Console.ReadLine();
                                                                    

                Console.WriteLine();
                Console.WriteLine("Anna vielä tunti-/pistemäärä:");
                string syotettypiste = Console.ReadLine();
                int tehtypiste = int.Parse(syotettypiste);                                    

                       
                String sql4 = "INSERT INTO dbo.talkootyot (henkiloid,talkoo_tyo,talkoo_pisteet, talkoo_tekopva) " +  // tallennetaan kantaan 
                    "VALUES (@henkiloid,@talkoo_tyo, @talkoo_pisteet, @talkoo_tekopva)";

                SqlCommand komento4 = new SqlCommand(sql4, yhteys);

                {
                    komento4.Parameters.Add("@henkiloid", System.Data.SqlDbType.NChar).Value = "1";  // NYT KOVAKOODATTU: Tähän pitäisi hakea alussa annetun numeron perusteella oikea id...

                    komento4.Parameters.Add("@talkoo_tyo", System.Data.SqlDbType.NChar).Value = syotettytyo;

                    komento4.Parameters.Add("@talkoo_pisteet", System.Data.SqlDbType.NChar).Value = tehtypiste;

                    komento4.Parameters.Add("@talkoo_tekopva", System.Data.SqlDbType.DateTime).Value = DateTime.Now;

                    komento4.ExecuteNonQuery();

                    komento.Dispose();

                }
                    Console.WriteLine();
                    Console.WriteLine("*** Kiitos antamistasi tiedoista. Pisteesi on kirjattu järjestelmään. ***");
                    Console.WriteLine();

                Console.WriteLine("*** Olet tämän lisäyksen jälkeen tehnyt talkootöitä: ***");                 // haetaan pejaajan uudet talkoopisteet
                Console.WriteLine();

                string sql5 = "SELECT talkootyot.talkoo_tekopva, talkootyot.talkoo_tyo, talkootyot.talkoo_pisteet " +    
                    "FROM henkilot, talkootyot " +
                    "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                    "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";                 

                SqlCommand komento5 = new SqlCommand(sql5, yhteys);
                SqlDataReader lukija5 = komento5.ExecuteReader();

                while (lukija5.Read())
                {

                    Console.WriteLine("Talkootyö: " + lukija5["talkoo_tyo"] + " " + lukija5["talkoo_pisteet"] + " talkoopistettä " + "(" + lukija5["talkoo_tekopva"] + ")");
                }


                lukija5.Close();

                string sql6 = "SELECT SUM(talkootyot.talkoo_pisteet) " +                    // lasketaan yhteen pejaajan uudet talkoopisteet
                              "FROM henkilot, talkootyot " +
                               "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                              "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";

                SqlCommand komento6 = new SqlCommand(sql6, yhteys);
                object summa2 = komento6.ExecuteScalar();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("*** Talkootyöpisteesi lisäyksen jälkeen ovat: " + summa2 + " pistettä. ***"); ;   // tulostetaan uusi summa

                komento6.Dispose();
            }

            else if (valinta == 2)
            {
                Console.WriteLine();
                Console.WriteLine("Kiitos ohjelman käytöstä. Ohjelma suljetaan.");
            }

            else
            {
                Console.WriteLine();
                Console.WriteLine("Anna vastauksesti muodossa: kyllä = 1 / ei = 2");             // jos syöttää väärin
                                                                                                // TÄMÄ KOHTA KORJAA, LOPETTAA OHJELMAN
            }

            Console.WriteLine();
            Console.WriteLine("Valmis, suljetaan tietokantayhteys.");
            yhteys.Close();
            Console.ReadLine();
                                               
          
        }
    }
}
