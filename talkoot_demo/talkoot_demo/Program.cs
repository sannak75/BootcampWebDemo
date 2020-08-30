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

            // Tietokantayhteyden luonti:

            Console.WriteLine("Aloitetaan tietokantayhteyden avaus...");

            string yhteysmerkkijono = "Server=localhost\\SQLEXPRESS;Database=talkoot;Trusted_Connection=True;";
            SqlConnection yhteys = new SqlConnection(yhteysmerkkijono);
            yhteys.Open();

            Console.WriteLine("Tietokantayhteys avattu onnistuneesti.");

            Console.WriteLine("Olet syöttämässä pelaajan plaa plaa tietoja.");                 // tässä haetaan kannasta tietoa.

            Console.WriteLine("Olet tähän mennessä tehnyt yhteensä plaa plaa talkootuntia.");  // tässä haetaan kannasta tietoa.

            Console.WriteLine("Mitä talkootyötä olet tehnyt:");                                // tallennetaan kantaan

            Console.WriteLine("Anna vielä tuntimäärä:");                                       // tallennetaan kantaan                  

            Console.WriteLine("Tähän asti talkootuntisi ovat:");
        }
    }
}
