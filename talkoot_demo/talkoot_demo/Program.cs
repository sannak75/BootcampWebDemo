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

            // Tietokantayhteyden luonti:

            Console.WriteLine("(Aloitetaan tietokantayhteyden avaus...)");

            string yhteysmerkkijono = "Server=localhost\\SQLEXPRESS;Database=talkoot;Trusted_Connection=True;";
            SqlConnection yhteys = new SqlConnection(yhteysmerkkijono);
            yhteys.Open();

            Console.WriteLine("(Tietokantayhteys avattu onnistuneesti.)");
            Console.WriteLine();

            Console.WriteLine("***************************************************************");
            Console.WriteLine("***  Hei! Tässä ohjelmassa voit syöttää talkootyötietojasi  ***");
            Console.WriteLine("***************************************************************");
            Console.WriteLine();

            Console.WriteLine("Anna pelaajanumerosi:");
            Console.WriteLine();

            string annettupelaajanumero = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine();

            string sql = "SELECT etunimi, sukunimi " +
                "FROM henkilot " +
                "WHERE pelaajanumero = '"+annettupelaajanumero+"'";                       // haetaan pejaajan tiedot
                     
            SqlCommand komento = new SqlCommand(sql, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            while (lukija.Read())
            {
                Console.WriteLine("*** Syötät talkootietoja pelaajalle: " + lukija["etunimi"] + " " + lukija["sukunimi"] + " / pelinumerolla: " + annettupelaajanumero + " ***");
            }
            Console.WriteLine();
            lukija.Close();

           
            Console.WriteLine("Olet tähän mennessä tehnyt talkootöitä:");         // haetaan pejaajan talkoopisteet
            Console.WriteLine();

            string sql2 = "SELECT talkootyot.tyoid, talkootyot.talkoo_tekopva, talkootyot.talkoo_tyo, talkootyot.talkoo_pisteet " +
                "FROM henkilot, talkootyot " +
                "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";  

            SqlCommand komento2 = new SqlCommand(sql2, yhteys);
            SqlDataReader lukija2 = komento2.ExecuteReader();

            while (lukija2.Read())
            {
                
                Console.WriteLine("Talkootyö: " + lukija2["talkoo_tyo"] + ", " + lukija2["talkoo_pisteet"] + " talkoopistettä " + "(" + lukija2["talkoo_tekopva"] + ") työtunnus: " +lukija2["tyoid"]);
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
            Console.WriteLine("--- Haluatko kirjata lisää talkoopisteitä? ---");           // uusien pisteiden kirjaus
            Console.WriteLine();

            Console.WriteLine("Anna vastauksesti muodossa: kyllä = 1 / ei = 2");
            Console.WriteLine();

            Console.WriteLine("--- Jos haluat lisätä ohjelmaan uuden pelaajan tiedot - valitse 3 ---");
            Console.WriteLine();

            Console.WriteLine("--- Jos haluat poistaa lisäämäsi talkootyön - valitse 4 ---");
            Console.WriteLine();

            string syotettyarvo = Console.ReadLine();
            int valinta = int.Parse(syotettyarvo);

            if (valinta == 1)                                                              // tähän valinta kyllä/ei
            {
                Console.WriteLine();
                Console.WriteLine("Ole hyvä, syötä tiedot tekemästäsi talkootyöstä:");

                Console.WriteLine();
                Console.WriteLine("Mitä talkootyötä olet tehnyt:");                        // pisteiden syöttö
                string syotettytyo = Console.ReadLine();


                Console.WriteLine();
                Console.WriteLine("Anna vielä tunti-/pistemäärä:");
                string syotettypiste = Console.ReadLine();
                int tehtypiste = int.Parse(syotettypiste);


                String sql4 = "INSERT INTO dbo.talkootyot (henkiloid,talkoo_tyo,talkoo_pisteet, talkoo_tekopva) " +  // tallennetaan kantaan 
                    "VALUES (@henkiloid,@talkoo_tyo, @talkoo_pisteet, @talkoo_tekopva)";


                /* TÄMÄ VÄLI EI TOIMI -- KORJAA:   (+ LOOPIN LISÄYS)
                 * 
                      string henkilo = "SELECT henkilot.henkiloid " +
                                        "FROM henkilot" +
                                        "WHERE henkilot.pelaajanumero = '" + annettupelaajanumero + "'";

                      int henkilonumero = Convert.ToInt32(henkilo);
                      // int henkilonumero = int.Parse(henkilo);
                      SqlCommand komento4 = new SqlCommand(sql4, yhteys);
                      {
                          komento4.Parameters.Add("@henkiloid", System.Data.SqlDbType.Int).Value = henkilonumero; */


                SqlCommand komento4 = new SqlCommand(sql4, yhteys);

                {
                    komento4.Parameters.Add("@henkiloid", System.Data.SqlDbType.Int).Value = 3;          // NYT KOVAKOODATTU: Tähän pitäisi hakea alussa annetun numeron perusteella oikea id...

                    komento4.Parameters.Add("@talkoo_tyo", System.Data.SqlDbType.NChar).Value = syotettytyo;

                    komento4.Parameters.Add("@talkoo_pisteet", System.Data.SqlDbType.NChar).Value = tehtypiste;

                    komento4.Parameters.Add("@talkoo_tekopva", System.Data.SqlDbType.DateTime).Value = DateTime.Now;

                    komento4.ExecuteNonQuery();

                    komento4.Dispose();

                }
                Console.WriteLine();
                Console.WriteLine("*** Kiitos antamistasi tiedoista. Pisteesi on kirjattu järjestelmään. ***");
                Console.WriteLine();

                Console.WriteLine("*** Olet tämän lisäyksen jälkeen kirjannut seuraavat talkootyöt: ***");                 // haetaan pejaajan uudet talkoopisteet
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

                string sql6 = "SELECT SUM(talkootyot.talkoo_pisteet) " +                                    // lasketaan yhteen pejaajan uudet talkoopisteet
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

            else if (valinta == 3)
            {
                Console.WriteLine();
                Console.WriteLine("*** Uuden pelaajan lisäys. ***");                // Uuden pelaajan lisäys kantaan.

                Console.WriteLine();
                Console.WriteLine("Ole hyvä, syötä uuden pelaajan tiedot:");

                Console.WriteLine();
                Console.WriteLine("Anna pelaajan etunimi:");
                string syotettyenimi = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("Anna pelaajan sukunimi:");
                string syotettysnimi = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("Anna pelaajan emil:");
                string syotettyemail = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("Anna pelaajan tunnus:");
                string syotettytunnus = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("Anna pelaajan numero:");
                string syotettynumero = Console.ReadLine();
                int uusipelaajanumero = int.Parse(syotettynumero);

                Console.WriteLine();

                String sql7 = "INSERT INTO dbo.henkilot (sukunimi, etunimi, email, tunnus, pelaajanumero) " +  // tallennetaan kantaan 
                    "VALUES (@sukunimi, @etunimi, @email, @tunnus, @pelaajanumero)";

                SqlCommand komento7 = new SqlCommand(sql7, yhteys);

                {
                    komento7.Parameters.Add("@sukunimi", System.Data.SqlDbType.NChar).Value = syotettysnimi;

                    komento7.Parameters.Add("@etunimi", System.Data.SqlDbType.NChar).Value = syotettyenimi;

                    komento7.Parameters.Add("@email", System.Data.SqlDbType.NChar).Value = syotettyemail;

                    komento7.Parameters.Add("@tunnus", System.Data.SqlDbType.NChar).Value = syotettytunnus;

                    komento7.Parameters.Add("@pelaajanumero", System.Data.SqlDbType.Int).Value = uusipelaajanumero;

                    komento7.ExecuteNonQuery();

                    komento7.Dispose();

                }

                Console.WriteLine("*** Uuden pelaajan tiedot lisätty onnistuneesti. ***");

            }

            else if (valinta == 4)
            {
                Console.WriteLine();
                Console.WriteLine("Anna poistettavan talkootyörivin työtunnus. ");                // Rivin poisto kannasta.
                Console.WriteLine();

                string poistettavarivi = Console.ReadLine();

                string sql8 = "DELETE FROM talkootyot " +                 
                              "WHERE talkootyot.tyoid = '" + poistettavarivi + "'";
             

                SqlCommand komento8 = new SqlCommand(sql8, yhteys);

                {
                    komento8.Parameters.Add("@tyoid", System.Data.SqlDbType.NChar).Value = poistettavarivi;
                    komento8.ExecuteNonQuery();

                    komento8.Dispose();

                }               
                                    
                Console.WriteLine();
                Console.WriteLine("*** Valittu talkootyörivi poistettu onnistuneesti. ***");
                Console.WriteLine();

                Console.WriteLine("Poiston jälkeen sinulle on kirjattu seuraavia talkootöitä:");                 // haetaan pejaajan uudet talkoopisteet
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

                string sql6 = "SELECT SUM(talkootyot.talkoo_pisteet) " +                                    // lasketaan yhteen pejaajan uudet talkoopisteet
                              "FROM henkilot, talkootyot " +
                               "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                              "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";

                SqlCommand komento6 = new SqlCommand(sql6, yhteys);
                object summa2 = komento6.ExecuteScalar();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("*** Kirjatut talkootyösi poiston jälkeen ovat: " + summa2 + " pistettä. ***"); ;   // tulostetaan uusi summa

                komento6.Dispose();
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
