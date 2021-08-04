using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;

namespace gdd
{    
    class Program
    {
        static readonly string names_txtfile = @"txt/names.txt";
        static readonly string surnames_txtfile = @"txt/surnames.txt"; 
        static readonly string states_txtfile = @"txt/states.txt"; 
        static string[] names ;
        static string[] surnames ;
        static string[] states ;
        static string[] postal_code ;
        static int random ;

        static void Main(string[] args)
        {

            ReadStates();
            postal_code = new string[states.Length];
            SplitString();
            ReadNames();
            ReadSurnames();

            int countofnames = names.Length ;
            int countofsurnames = surnames.Length;
            int countofstates = states.Length;

            Random rnd = new Random();

            database_access db_access = new database_access();
            db_access.ConnectMongoDb();


            int max_data = 100;

            List<User> users = new List<User>();

            for (int i = 0; i < max_data; i++)
            {
                //System.Console.WriteLine(i + " / " + max_data);

                var random_date = GenerateRandomDate();
                var age = CalculateAge(random_date);

                random = rnd.Next(countofsurnames) ;
             
                string name = names[(random % countofnames)];
                string surname = surnames[random];

                int rnds = random % countofstates ;
                string state = states[rnds];
                string postalcode = postal_code[rnds];

                var user = new User{
                Id = ObjectId.GenerateNewId(),
                UserId = i,
                Name = name,
                SurName = surname,
                Birthday = random_date,
                Age = age,
                address = new Address{
                    State = state,
                    PostalCode = postalcode
                    }
                };

                //db_access.AddUser(user);

                //Console.Clear();ü
                users.Add(user);
            }
            db_access.AddUsers(users);


        }
        static void ReadNames()
        {
            if (File.Exists(names_txtfile))
                names = File.ReadAllLines(names_txtfile);
            else
                System.Console.WriteLine("names.txt not found");
        }
        static void ReadSurnames()
        {
            if (File.Exists(surnames_txtfile))
                surnames = File.ReadAllLines(surnames_txtfile);
            else
                System.Console.WriteLine("names.txt not found");
        }
        static void ReadStates()
        {
            if (File.Exists(states_txtfile))
            {
                states = File.ReadAllLines(states_txtfile);
            }
                
            else
                System.Console.WriteLine("names.txt not found");
        }
        static void SplitString()
        {
            for (int i = 0; i < states.Length; i++)
            {
                postal_code[i] = states[i].Split(",")[1];
                states[i] = states[i].Split(",")[0];
                
            }
        }
        static DateTime GenerateRandomDate()
        {
            Random rnd = new Random();

            DateTime start = new DateTime(1930 , 1 , 1);
            DateTime today = DateTime.Today;

            int days = (today - start ).Days;

            return start.AddDays(rnd.Next(days));
        }
        static int CalculateAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year ;
            return age;
        }
        


    }
}
