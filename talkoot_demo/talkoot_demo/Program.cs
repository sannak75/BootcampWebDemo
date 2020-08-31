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
                "WHERE pelaajanumero = '"+annettupelaajanumero+"'";  // haetaan pejaajan tiedot
                     
            SqlCommand komento = new SqlCommand(sql, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            while (lukija.Read())
            {
                Console.WriteLine("*** Syötät pelaajan: " + lukija["etunimi"] + " " + lukija["sukunimi"] + " tietoja. ***");
            }
            Console.WriteLine();
            lukija.Close();

           
            Console.WriteLine("*** Olet tähän mennessä tehnyt talkootöitä: ***");
            Console.WriteLine();

            string sql2 = "SELECT talkootyot.talkoo_tekopva, talkootyot.talkoo_tyo, talkootyot.talkoo_pisteet " +
                "FROM henkilot, talkootyot " +
                "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";  // haetaan pejaajan talkoopisteet

            SqlCommand komento2 = new SqlCommand(sql2, yhteys);
            SqlDataReader lukija2 = komento2.ExecuteReader();

            while (lukija2.Read())
            {
                
                Console.WriteLine("Talkootyö: " + lukija2["talkoo_tyo"] + " " + lukija2["talkoo_pisteet"] + " talkoopistettä " + "(" + lukija2["talkoo_tekopva"] + ")" );
            }
           

            lukija2.Close();

            string sql3 = "SELECT SUM(talkootyot.talkoo_pisteet) " +
                "FROM henkilot, talkootyot " +
                "WHERE henkilot.henkiloid = talkootyot.henkiloid " +
                "AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";  // lasketaan yhteen pejaajan talkoopisteet
            /*
            SqlCommand komento3 = new SqlCommand(sql3, yhteys);
            SqlDataReader lukija3 = komento3.ExecuteReader();

            while (lukija3.Read())
            {

                Console.WriteLine("Talkootyöpisteesi yhteensä ovat: " + lukija3[SUM(talkootyot.talkoo_pisteet)]); ;  // EI TOIMI, MITEN SUMMA TULOSTETAAN?
            }

            lukija3.Close();
            */

            komento.Dispose();

            

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("*** Haluatko kirjata lisää talkoopisteitä? ***");           // uusien pisteiden kirjaus
            Console.WriteLine();
            Console.WriteLine("Anna vastauksesti muodossa: kyllä = 1 / ei = 2");
            Console.WriteLine();

            string syotettyarvo = Console.ReadLine();
            int valinta = int.Parse(syotettyarvo);

            if (valinta == 1)                                                              //tähän valinta kyllä/ei
            {
                Console.WriteLine();
                Console.WriteLine("Ole hyvä, syötä tiedot tekemistäsi talkootöistä:");
            }
            else if (valinta == 2)
            {
                Console.WriteLine();
                Console.WriteLine("Kiitos ohjelman käytöstä. Ohjelma suljetaan.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Anna vastauksesti muodossa: kyllä = 1 / ei = 2");       // jos syöttää väärin
            }

            Console.WriteLine();
            Console.WriteLine("Valmis, suljetaan tietokantayhteys.");
            yhteys.Close();
            Console.ReadLine();
           

              
            Console.WriteLine();

            Console.WriteLine("Mitä talkootyötä olet tehnyt:");                                // tallennetaan kantaan

            Console.WriteLine();
            Console.WriteLine("Anna vielä tunti-/pistemäärä:");                                // tallennetaan kantaan                  

            Console.WriteLine("Kiitos antamistasi tiedoista. Pisteesi on kirjattu järjestelmään.");
            Console.WriteLine();
            Console.WriteLine("Tähän asti kirjaamasi talkootyöt ovat:");
        }
    }
}
