using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Classes;

namespace LibraryProject
{
    class Program
    {
        static Library library;

        static void Main(string[] args)
        {
            Console.WriteLine("KUTUPHANE OTOMASYONU [Created by @ogzkyr]\n");
            string password;
            do
            {
                Console.Write("Sifrenizi giriniz(cikis icin 0): ");
                password = Console.ReadLine();
                library = Library.GetLibrary(password);
                if (library == null)
                {
                    if (password != "0")
                        Console.WriteLine("Yanlis girdiniz! Tekrar deneyiniz..\n");
                }
                else
                {
                    Console.WriteLine("Basariyla giris yapildi.\n");
                    library.FillLibrary();
                    Menu();
                    break;
                }
	        } while (password != "0");
        }
        public static void Menu()
        {
            Console.WriteLine("MENU");
            Console.WriteLine("Kitap islemeleri:");
            Console.WriteLine("\t1. Kitap ekle");
            Console.WriteLine("\t2. Kitap ara");
            Console.WriteLine("\t3. Kitaplari listele");
            Console.WriteLine("Ogrenci islemleri:");
            Console.WriteLine("\t4. Ogrenci ekle");
            Console.WriteLine("\t5. Ogrenci ara");
            Console.WriteLine("\t6. Ogrencileri listele");
            Console.WriteLine("Diger islemeler:");
            Console.WriteLine("\t7. Odunc kitap ara");
            Console.WriteLine("\t8. Odunc kitaplari listele");
            Console.WriteLine("\t9. Borclari listele");
            Console.WriteLine("\t0. Cikis");
            Console.Write("Seciminizi yapiniz: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            switch (choice)
            {
                case 1:
                    library.AddBook();
                    break;
                case 2:
                    library.SearchBook();
                    break;
                case 3:
                    library.ListBook();
                    break;
                case 4:
                    library.AddStudent();
                    break;
                case 5:
                    library.SearchStudent();
                    break;
                case 6:
                    library.ListStudent();
                    break;
                case 7:
                    library.SearchLendedBook();
                    break;
                case 8:
                    library.ListStudentBook();
                    break;
                case 9:
                    library.ListDept();
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Boyle bir secenek yok! Tekrar deneyiniz.");
                    break;
            }
            Console.WriteLine("");
            if (choice != 0)
                Menu();
        }
    }
}