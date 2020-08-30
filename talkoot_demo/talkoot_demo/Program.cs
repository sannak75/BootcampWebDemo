using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talkoot_demo
{
    class Program
    {
        static void Main(string[] args)
        {
         
            Console.WriteLine("***Hei! Tässä ohjelmassa voit syöttää talkootyötietojasi***");
            Console.WriteLine();

            Console.WriteLine("Anna pelaajanumerosi:");

            string annettupelaajanumero = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("--- Hei pelaaja numerolla: " + annettupelaajanumero + "---");
            Console.WriteLine();

            // Tietokantayhteyden luonti:

            Console.WriteLine("Aloitetaan tietokantayhteyden avaus...");

            string yhteysmerkkijono = "Server=localhost\\SQLEXPRESS;Database=talkoot;Trusted_Connection=True;";
            SqlConnection yhteys = new SqlConnection(yhteysmerkkijono);
            yhteys.Open();

            Console.WriteLine("Tietokantayhteys avattu onnistuneesti.");
            Console.WriteLine();


            string sql = "SELECT etunimi, sukunimi FROM henkilot WHERE pelaajanumero = '"+annettupelaajanumero+"'";  // haetaan pejaajan tiedot
                     
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

            string sql2 = "SELECT talkootyot.talkoo_tekopva, talkootyot.talkoo_tyo, talkootyot.talkoo_pisteet FROM henkilot, talkootyot WHERE henkilot.henkiloid = talkootyot.henkiloid AND henkilot.pelaajanumero = '" + annettupelaajanumero + "'";  // haetaan pejaajan talkoopisteet

            SqlCommand komento2 = new SqlCommand(sql2, yhteys);
            SqlDataReader lukija2 = komento2.ExecuteReader();

            while (lukija2.Read())
            {
                
                Console.WriteLine("Talkootyö: " + lukija2["talkoo_tyo"] + " " + lukija2["talkoo_pisteet"] + " talkoopistettä " + "(" + lukija2["talkoo_tekopva"] + ")" );
            }
           

            lukija2.Close(); 
            komento.Dispose();

            Console.WriteLine();
            Console.WriteLine("Valmis, suljetaan tietokantayhteys.");
            yhteys.Close();
            Console.ReadLine();

            Console.WriteLine("Olet tähän mennessä tehnyt yhteensä plaa plaa talkootuntia.");  // tässä haetaan kannasta tietoa.

            Console.WriteLine("Mitä talkootyötä olet tehnyt:");                                // tallennetaan kantaan

            Console.WriteLine("Anna vielä tuntimäärä:");                                       // tallennetaan kantaan                  

            Console.WriteLine("Tähän asti talkootuntisi ovat:");
        }
    }
}
