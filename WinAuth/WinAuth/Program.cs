using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiUser = "Q36V";
            bool isAuthenticated = AuthenticateUser(apiUser);
            if (isAuthenticated)
            {
                Console.WriteLine("User is Aunthenticated");
            }
            else
            {
                Console.WriteLine("User is not Aunthenticated");
            }
            Console.ReadLine();
        }
        public static bool AuthenticateUser(string apiUser)
        {
            string username = ConfigurationManager.AppSettings["AllowedUsers"].ToString();
            string[] users = username.Split(';');

            if (users.Contains(apiUser))
            {
                var context = new System.DirectoryServices.AccountManagement.PrincipalContext(ContextType.Domain, "ES-Area1");

                var searchPrinciple = new UserPrincipal(context);
                searchPrinciple.SamAccountName = apiUser;

                PrincipalSearcher pS = new PrincipalSearcher();
                pS.QueryFilter = searchPrinciple;       //searches based on SamAccountName or DisplayName etc.

                //Perform the search
                PrincipalSearchResult<Principal> results = pS.FindAll();
                if (results.ToList().Count > 0)
                {
                    Principal pc = results.ToList()[0];
                    DirectoryEntry direEn = (DirectoryEntry)pc.GetUnderlyingObject();

                    Console.WriteLine("Email ID: " + direEn.Properties["mail"].Value.ToString());
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("First Name: " + direEn.Properties["givenName"].Value);
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("Last Name : " + direEn.Properties["sn"].Value);
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("SAM account name   : " + direEn.Properties["samAccountName"].Value);
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine("User principal name: " + direEn.Properties["userPrincipalName"].Value);
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine();
                    return true;
                }
            }
            return false;
        }
    }
}
