using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Classes
{
    class Library
    {
        ObjectList Students { get; set; }
        ObjectList Books { get; set; }
        ObjectList StudentBooks { get; set; }
        static Library Instance { get; set; }

        static readonly string Password = "123456";

        private Library()
        {

        }
        static Library()
        {
            Instance = new Library();
        }
        public static Library GetLibrary(string password)
        {
            if (password == Password)
                return Instance;
            return null;
        }
        public void FillLibrary()
        {
            FillStudents();
            FillBooks();
            FillStudentBooks();
        }
        void FillStudents()
        {
            Students = new ObjectList();
            string[] text = File.ReadAllLines("../../Datas/student.txt");
            foreach (var line in text)
            {
                string[] split = line.Split('|');
                int ID = Convert.ToInt32(split[0].Trim());
                string Name = split[1].Trim();
                Students.Add(new Student() { ID = ID, Name = Name });
            }
        }
        void FillBooks()
        {
            Books = new ObjectList();
            string[] lines = File.ReadAllLines("../../Datas/book.txt");

            foreach (var line in lines)
            {
                string[] split = line.Split('|');
                int ID = Convert.ToInt32(split[0].Trim());
                string Name = split[1].Trim();
                string Category = split[2].Trim();
                Books.Add(new Book() { ID = ID, Name = Name, Category = Category });
            }
        }
        void FillStudentBooks()
        {
            StudentBooks = new ObjectList();
            string[] lines = File.ReadAllLines("../../Datas/studentbook.txt");
            foreach (var line in lines)
            {
                string[] split = line.Split('|');
                int id = Convert.ToInt32(split[0].Trim());
                int studentID = Convert.ToInt32(split[1].Trim());
                int bookID = Convert.ToInt32(split[2].Trim());
                DateTime takeDate = Convert.ToDateTime(split[3].Trim());
                DateTime giveDate = Convert.ToDateTime(split[4].Trim());
                Student student = (Student)Students.GetObject(studentID);
                Book book = (Book)Books.GetObject(bookID);
                StudentBooks.Add(new StudentBook() { ID = id, Student = student, Book = book, TakeDate = takeDate, GiveDate = giveDate });
            }
        }
        void UpdateStudents()
        {
            string[] lines = new string[Students.Count];
            BaseObject current = Students == null ? null : Students.Head;
            for (int i=0; current != null; i++)
            {
                Student student = (Student)current;
                lines[i] = string.Format("{0} | {1}", student.ID, student.Name);
                current = current.Next;
            }
            File.WriteAllLines("../../Datas/student.txt", lines);
        }
        void UpdateBooks()
        {
            string[] lines = new string[Books.Count];
            BaseObject current = Books == null ? null : Books.Head;
            for (int i = 0; current != null; i++)
            {
                Book book = (Book)current;
                lines[i] = string.Format("{0} | {1} | {2}", book.ID, book.Name, book.Category);
                current = current.Next;
            }
            File.WriteAllLines("../../Datas/book.txt", lines);
        }
        void UpdateStudentBooks()
        {
            string[] lines = new string[StudentBooks.Count];
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            for (int i = 0; current != null; i++)
            {
                StudentBook studentBook = (StudentBook)current;
                lines[i] = string.Format("{0} | {1} | {2} | {3} | {4}", studentBook.ID, studentBook.Student.ID, studentBook.Book.ID, studentBook.TakeDate, studentBook.GiveDate);
                current = current.Next;
            }
            File.WriteAllLines("../../Datas/studentbook.txt", lines);
        }

        public void AddBook()
        {
            Console.WriteLine("KITAP EKLE");
            Book newBook = new Book();
            Console.Write("Kitap ID: ");
            newBook.ID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Kitap adi: ");
            newBook.Name = Console.ReadLine();
            Console.Write("Kitap kategorisi: ");
            newBook.Category = Console.ReadLine();
            if (Books.Add(newBook))
            {
                Console.WriteLine("Kitap basariyla eklendi.");
                UpdateBooks();
            }
            else
                Console.WriteLine("Kitap daha onceden eklenmis.");
        }
        public void SearchBook()
        {
            Console.WriteLine("KITAP ARA");
            Console.Write("Kitap adi: ");
            string bookName = Console.ReadLine();
            Console.WriteLine("");
            BaseObject current = Books == null ? null : Books.Head;
            while (current != null)
            {
                if (((Book)current).Name.ToLower() == bookName.ToLower())
                    break;
                current = current.Next;
            }
            if (current == null)
                Console.WriteLine("Aradiginiz kitap bulunamadi!");
            else
                BookDetails((Book)current);
        }
        void BookDetails(Book book)
        {
            Console.WriteLine("KITAP DETAYLARI");
            Console.WriteLine("ID: " + book.ID);
            Console.WriteLine("Adi: " + book.Name);
            Console.WriteLine("Kategorisi: " + book.Category);
            Console.WriteLine("\nYapabileceginiz islemler: ");
            Console.WriteLine("\t1. Kitabi odunc ver");
            Console.WriteLine("\t2. Kitabi alan ogrenciler");
            Console.WriteLine("\t3. Kitabi sil");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    LendBook(book);
                    break;
                case 2:
                    ListStudentBook(book);
                    break;
                case 3:
                    DeleteBook(book);
                    break;
            }
        }
        public void ListBook()
        {
            Console.WriteLine("KITAPLAR");
            BaseObject current = Books == null ? null : Books.Head;
            if (current == null)
            {
                Console.WriteLine("Henuz eklenen kitap yok.");
            }
            else
            {
                while (current != null)
                {
                    Console.WriteLine(((Book)current).ID + " - " + ((Book)current).Category + " - " + ((Book)current).Name);
                    current = current.Next;
                }
                Console.Write("\nDetaylar icin ID girin (menu icin 0): ");
                BaseObject obj = Books.GetObject(Convert.ToInt32(Console.ReadLine()));
                if (obj != null)
                {
                    Console.WriteLine("");
                    BookDetails((Book)obj);
                }
            }
        }
        void DeleteBook(Book book)
        {
            Console.WriteLine("KITAP SIL");
            Console.WriteLine("{0} adli kitabi silmek istediginizden emin misiniz? ", book.Name);
            Console.WriteLine("\t1. Evet, sil");
            Console.WriteLine("\t2. Hayir, silme");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                Books.Delete(book.ID);
                DeleteStudentBook(book); // Kitabı alan ogrencileri silme
                Console.WriteLine("Kitap basariyla silindi.");
                UpdateBooks();
            }
            else
            {
                Console.WriteLine("Kitap silme islemi iptal edildi.");
            }
        }
        public void AddStudent()
        {
            Console.WriteLine("OGRENCI EKLE");
            Student newStudent = new Student();
            Console.Write("Ogrenci numarasi: ");
            newStudent.ID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ogrenci adi: ");
            newStudent.Name = Console.ReadLine();
            Students.Add(newStudent);
            UpdateStudents();
        }
        public void SearchStudent()
        {
            Console.WriteLine("OGRENCI ARA");
            Console.Write("Ogrenci numarasi: ");
            int studentID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            BaseObject current = Students == null ? null : Students.Head;
            while (current != null)
            {
                if (((Student)current).ID == studentID)
                    break;
                current = current.Next;
            }
            if (current == null)
                Console.WriteLine("Aradiginiz ogrenci bulunamadi!");
            else
                StudentDetails((Student)current);
        }
        void StudentDetails(Student student)
        {
            Console.WriteLine("OGRENCI DETAYLARI");
            Console.WriteLine("ID: " + student.ID);
            Console.WriteLine("Adi: " + student.Name);
            Console.WriteLine("\nYapabileceginiz islemler: ");
            Console.WriteLine("\t1. Aldigi kitaplar");
            Console.WriteLine("\t2. Borc hesapla");
            Console.WriteLine("\t3. Ogrenciyi sil");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    ListStudentBook(student);
                    break;
                case 2:
                    double totalDept = CalculateDept(student);
                    if (totalDept > 0)
                        Console.WriteLine("{0} Krs. borc hesaplandi", totalDept);
                    else
                        Console.WriteLine("Bu ogrenci icin borc bulunamadi.");
                    break;
                case 3:
                    DeleteStudent(student);
                    break;
            }
        }
        public void ListStudent()
        {
            Console.WriteLine("OGRENCILER");
            BaseObject current = Students == null ? null : Students.Head;
            if (current == null)
            {
                Console.WriteLine("Henuz eklenen ogrenci yok.");
            }
            else
            {
                while (current != null)
                {
                    Console.WriteLine(((Student)current).ID + " - " + ((Student)current).Name);
                    current = current.Next;
                }
                Console.Write("\nDetaylar icin ID girin (menu icin 0): ");
                BaseObject obj = Students.GetObject(Convert.ToInt32(Console.ReadLine()));
                if (obj != null)
                {
                    Console.WriteLine("");
                    StudentDetails((Student)obj);
                }
            }
        }
        void DeleteStudent(Student student)
        {
            Console.WriteLine("OGRENCI SIL");
            Console.WriteLine("{0} adli ogrenciyi silmek istediginizden emin misiniz?", student.Name);
            Console.WriteLine("\t1. Evet, sil");
            Console.WriteLine("\t2. Hayir, silme");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                Students.Delete(student.ID);
                DeleteStudentBook(student); // Ogrencinin aldığı kitapları silme
                Console.WriteLine("Ogrenci basariyla silindi.");
                UpdateStudents();
            }
            else
            {
                Console.WriteLine("Ogrenci silme islemi iptal edildi.");
            }
        }
        void LendBook(Book book)
        {
            Console.WriteLine("ODUNC VERME");
            Student lendedStudent = whoLendedBook(book);
            if (lendedStudent != null)
            {
                Console.WriteLine("Bu kitap suan '{0}' adli ogrencide.", lendedStudent.Name);
            }
            else
            {
                Console.Write("Ogrenci numarasi: ");
                Student student = (Student)Students.GetObject(Convert.ToInt32(Console.ReadLine()));
                if (student != null)
                {
                    if (student.bookCount == 3)
                    {
                        Console.WriteLine("Bir ogrenci en fazla 3 kitap alabilir!");
                    }
                    else
                    {
                        StudentBook newStudentBook = new StudentBook();
                        newStudentBook.Book = book;
                        newStudentBook.Student = student;
                        newStudentBook.TakeDate = DateTime.Now;
                        newStudentBook.GiveDate = null;
                        student.bookCount++;
                        StudentBooks.Add(newStudentBook);
                        Console.WriteLine("Kitap basariyla odunc verildi.");
                        UpdateStudentBooks();
                    }
                }
                else
                {
                    Console.WriteLine("Bu numaraya ait ogrenci bulunmuyor!");
                }
            }
        }
        void TakeBackBook(StudentBook studentBook)
        {
            Console.WriteLine("IADE ETME");
            double studentDept = CalculateDept(studentBook);
            if (studentDept > 0)
            {
                Console.WriteLine("Iade icin {0} Krs. borc odenmesi gerekiyor.", studentDept);
            }
            Console.WriteLine("Onayliyor musunuz?");
            Console.WriteLine("\t1. Evet");
            Console.WriteLine("\t2. Hayir");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                studentBook.GiveDate = DateTime.Now;
                studentBook.Student.bookCount--;
                Console.WriteLine("Iade islemi basariyla gereceklesti.");
                UpdateStudentBooks();
            }
            else
            {
                Console.WriteLine("Iade islemi iptal edildi.");
            }
        }
        public void SearchLendedBook()
        {
            Console.WriteLine("ODUNC KITAP ARA");
            Console.Write("Ogrenci numarasi: ");
            int studentID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Kitap ID: ");
            int bookID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            while (current != null)
            {
                BaseObject student = Students.GetObject(studentID);
                BaseObject book = Books.GetObject(bookID);
                StudentBook cur = (StudentBook)current;
                if (cur.Student == student && cur.Book == book)
                    break;
                current = current.Next;
            }
            if (current == null)
                Console.WriteLine("Aradiginiz islem bulunamadi!");
            else
                StudentBookDetails((StudentBook)current);
        }
        public void ListStudentBook()
        {
            Console.WriteLine("ODUNC VERILENLER");
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            if (current == null)
            {
                Console.WriteLine("Henuz odunc verilen kitap yok.");
            }
            else
            {
                while (current != null)
                {
                    StudentBook cur = (StudentBook)current;
                    if (cur.GiveDate == null)
                        Console.WriteLine("{0} - {1} - {2}", cur.ID, cur.Student.Name, cur.Book.Name);
                    current = current.Next;
                }
                Console.Write("\nDetaylar icin ID girin (menu icin 0): ");
                BaseObject obj = StudentBooks.GetObject(Convert.ToInt32(Console.ReadLine()));
                if (obj != null)
                {
                    Console.WriteLine("");
                    StudentBookDetails((StudentBook)obj);
                }
            }
        }
        public void ListStudentBook(Student student)
        {
            Console.WriteLine("'{0}' ALDIGI KITAPLAR", student.Name);
            BaseObject current = StudentBooks.Head;
            while (current != null)
            {
                StudentBook cur = (StudentBook)current;
                if (cur.Student == student)
                    Console.WriteLine("{0} - {1} [{2}]", cur.ID, cur.Book.Name, cur.GiveDate == null ? "ogrencide" : "iade edildi");
                current = current.Next;
            }
            Console.Write("\nDetaylar icin ID girin (menu icin 0): ");
            BaseObject obj = StudentBooks.GetObject(Convert.ToInt32(Console.ReadLine()));
            if (obj != null)
            {
                Console.WriteLine("");
                StudentBookDetails((StudentBook)obj);
            }
        }
        public void ListStudentBook(Book book)
        {
            Console.WriteLine("'{0}' ALAN OGRENCILER", book.Name);
            BaseObject current = StudentBooks.Head;
            while (current != null)
            {
                StudentBook cur = (StudentBook)current;
                if (cur.Book == book)
                    Console.WriteLine("{0} - {1} [{2}]", cur.ID, cur.Student.Name, cur.GiveDate == null ? "ogrencide" : "iade edildi");
                current = current.Next;
            }
            Console.Write("\nDetaylar icin ID girin (menu icin 0): ");
            BaseObject obj = StudentBooks.GetObject(Convert.ToInt32(Console.ReadLine()));
            if (obj != null)
            {
                Console.WriteLine("");
                StudentBookDetails((StudentBook)obj);
            }
        }
        public void StudentBookDetails(StudentBook studentBook)
        {
            Console.WriteLine("ODUNC DETAYLARI");
            Console.WriteLine("ID: " + studentBook.ID);
            Console.WriteLine("Ogrenci adi: " + studentBook.Student.Name);
            Console.WriteLine("Kitap adi: " + studentBook.Book.Name);
            Console.WriteLine("Alindigi tarih: " + studentBook.TakeDate);
            Console.WriteLine("Iade tarihi: " + studentBook.GiveDate);
            Console.WriteLine("\nYapabileceginiz islemler: ");
            Console.WriteLine("\t1. Kitap iade");
            Console.WriteLine("\t2. Borc hesapla");
            Console.WriteLine("\t3. Kitap detaylari");
            Console.WriteLine("\t4. Ogrenci detaylari");
            Console.WriteLine("\t5. Islemi sil");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    TakeBackBook(studentBook);
                    break;
                case 2:
                    double dept = CalculateDept(studentBook);
                    if (dept > 0)
                        Console.WriteLine("{0} Krs. borc hesaplandi", dept);
                    else
                        Console.WriteLine("Bu odunc verme isleminde borc bulunamadi");
                    break;
                case 3:
                    BookDetails(studentBook.Book);
                    break;
                case 4:
                    StudentDetails(studentBook.Student);
                    break;
                case 5:
                    DeleteStudentBook(studentBook);
                    break;
            }
        }
        public double CalculateDept(Student student)
        {
            double totalDept = 0;
            BaseObject current = StudentBooks.Head;
            while (current != null)
            {
                if (((StudentBook)current).Student == student && ((StudentBook)current).GiveDate == null)
                {
                    TimeSpan ts = DateTime.Now - ((StudentBook)current).TakeDate.AddDays(15);
                    if (ts.Days >= 15)
                        totalDept += (ts.Days - 15) * 0.5;
                }
                current = current.Next;
            }
            return totalDept;
        }        
        public double CalculateDept(StudentBook studentBook)
        {
            if (studentBook.GiveDate == null)
            {
                TimeSpan ts = DateTime.Now - studentBook.TakeDate.AddDays(15);
                return (ts.Days - 15) * 0.5;
            }
            return 0;
        }
        public void ListDept()
        {
            Console.WriteLine("BORCLULAR");
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            int i = 0;
            while (current != null)
            {
                StudentBook cur = (StudentBook)current;
                if (cur.GiveDate == null)
                {
                    TimeSpan ts = DateTime.Now - cur.TakeDate.AddDays(15);
                    if (ts.Days >= 15)
                        Console.WriteLine("{0} - {1} - {2}", ++i, cur.Student.Name, (ts.Days - 15) * 0.5 + " Krs.");
                }
                current = current.Next;
            }
            if (i == 0)
                Console.WriteLine("Henuz borclu ogrenci yok.");
        }
        public void DeleteStudentBook(StudentBook studentBook)
        {
            Console.WriteLine("ODUNC ISLEMI SIL");
            Console.WriteLine("Bu islemi silmek istediginizden emin misiniz?");
            Console.WriteLine("\t1. Evet, sil");
            Console.WriteLine("\t2. Hayir, silme");
            Console.Write("Seciminizi yapiniz (menu icin 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                StudentBooks.Delete(studentBook.ID);
                Console.WriteLine("Odunc islemi basariyla silindi.");
                UpdateStudentBooks();
            }
            else
            {
                Console.WriteLine("Odunc islemi silme iptal edildi.");
            }
        }
        public void DeleteStudentBook(Student student)
        {
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            while (current != null)
            {
                if (((StudentBook)current).Student == student)
                {
                    StudentBooks.Delete(current.ID);
                    UpdateStudentBooks();
                }
                current = current.Next;
            }
        }
        public void DeleteStudentBook(Book book)
        {
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            while (current != null)
            {
                if (((StudentBook)current).Book == book)
                {
                    StudentBooks.Delete(current.ID);
                    UpdateStudentBooks();
                }
                current = current.Next;
            }
        }
        Student whoLendedBook(Book book)
        {
            BaseObject current = StudentBooks == null ? null : StudentBooks.Head;
            while (current != null)
            {
                StudentBook cur = (StudentBook)current;
                if (cur.Book.ID == book.ID)
                    if (cur.GiveDate == null)
                        return cur.Student;
                    else
                        return null;
                current = current.Next;
            }
            return null;
        }
    }
}