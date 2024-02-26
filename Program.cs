using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ELibraryManegement.Models;


public enum BookCategory
{
    Вымысел,
    Художественная,
    Биография,
    Драма,
    Фантастика,
    Технологии,
    Магический_реализм,
    Роман,
    История,
    Фэнтези
}

class Program
{
    static readonly List<Book> books = new();
    static readonly List<User> users = new();
    static readonly List<BorrowDetails> borrowDetails = new();

    static void Main()
    {
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;

        InitializeData();

        while (true)
        {
            Console.WriteLine("Добро пожаловать в систему управления библиотекой!");
            Console.WriteLine("1.Войти");
            Console.WriteLine("2.Выйти");
            Console.Write("Пожалуйста, выберите вариант: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Console.WriteLine("Спасибо за использование системы управления библиотекой.");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }
        }
    }

    static void Login()
    {
        Console.Write("\nВведите ваше имя пользователя: ");
        string? username = Console.ReadLine();
        Console.Write("Введите ваш пароль: ");
        string? password = Console.ReadLine();

        User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            Console.WriteLine($"\nДобро пожаловать, {username}!");

            if (username == "admin")
            {
                AdminMenu(user);
            }
            else
            {
                UserMenu(user);
            }
        }
        else
        {
            Console.WriteLine("Неверное имя пользователя или пароль. Пожалуйста, попробуйте снова.");
        }
    }

    static void AdminMenu(User user)
    {
        while (true)
        {
            Console.WriteLine("\nМеню администратора");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Просмотреть книги");
            Console.WriteLine("3. Удалить книгу");
            Console.WriteLine("4. Выйти");
            Console.Write("Пожалуйста, выберите вариант: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    ViewBooks(user);
                    break;
                case "3":
                    RemoveBook();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }
        }
    }

    static void UserMenu(User user)
    {
        while (true)
        {
            Console.WriteLine("\nМеню пользователя");
            Console.WriteLine("1. Поиск книг");
            Console.WriteLine("2. Взять книгу");
            Console.WriteLine("3. Вернуть книгу");
            Console.WriteLine("4. Просмотреть книги");
            Console.WriteLine("5. Выйти");
            Console.Write("Пожалуйста, выберите вариант: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SearchBooks();
                    break;
                case "2":
                    BorrowBook();
                    break;
                case "3":
                    ReturnBook();
                    break;
                case "4":
                    ViewBooks(user);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }
        }
    }

    static void AddBook()
    {
        Book book = new()
        {
            BookId = books.Count + 1
        };

        Console.Write("\nВведите название книги: ");
        book.Title = Console.ReadLine();

        Console.Write("Введите имя автора: ");
        book.Author = Console.ReadLine();

        Console.Write("Введите цену книги: ");
        book.Price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Введите количество: ");
        book.Quantity = Convert.ToInt32(Console.ReadLine());


        Console.WriteLine("Выберите категорию книги:");
        foreach (var category in Enum.GetValues(typeof(BookCategory)))
        {
            Console.WriteLine($"{(int)category}. {category}");
        }
        Console.Write("Введите номер категории: ");
        int categoryNumber = Convert.ToInt32(Console.ReadLine());
        book.Category = (BookCategory)categoryNumber;

        books.Add(book);
        Console.WriteLine("Книга успешно добавлена.");
    }
    static void RemoveBook()
    {
        Console.Write("\nВведите ID книги для удаления: ");
        int bookId = Convert.ToInt32(Console.ReadLine());

        Book bookToRemove = books.FirstOrDefault(b => b.BookId == bookId);

        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            Console.WriteLine("Книга успешно удалена.");
        }
        else
        {
            Console.WriteLine("Книга с указанным ID не найдена.");
        }
    }
    static void ViewBooks(User user)
    {
        Console.WriteLine("\nДоступные книги:");
        Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-10} {4,-10} {5,-15}", "ID", "Название", "Автор", "Цена", "Количество", "Категория");

        IEnumerable<Book> accessibleBooks;

        if (user.Username == "admin")
        {
            accessibleBooks = books;
        }
        else
        {
            accessibleBooks = books.Where(b => b.Quantity > 0);
        }

        foreach (var book in accessibleBooks)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-10} {4,-10} {5,-15}", book.BookId, book.Title, book.Author, book.Price, book.Quantity, book.Category);
        }
    }
    static void BorrowBook()
    {
        Console.Write("\nВведите ID книги для аренды: ");
        int bookId = Convert.ToInt32(Console.ReadLine());

        Book book = books.FirstOrDefault(b => b.BookId == bookId);

        if (book != null && book.Quantity > 0)
        {
            book.Quantity--;

            BorrowDetails details = new BorrowDetails
            {
                UserId = 1,
                BookId = bookId,
                BorrowDate = DateTime.Now
            };

            borrowDetails.Add(details);

            Console.WriteLine("Книга успешно взята напрокат.");
        }
        else
        {
            Console.WriteLine("Книга недоступна для аренды.");
        }
    }

    static void SearchBooks()
    {
        Console.WriteLine("Введите название книги или имя автора для поиска: ");
        string search = Console.ReadLine().ToLower();

        var search_books = books.Where(b =>
            b.Title.ToLower().Contains(search) ||
            b.Author.ToLower().Contains(search) 
        );

        if (search_books.Any())
        {
            Console.WriteLine("\nРезультаты поиска:");
            Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-10} {4,-10} {5,-15}", "ID", "Название", "Автор", "Цена", "Количество", "Категория");

            foreach (var book in search_books)
            {

                Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-10} {4,-10} {5,-15}", book.BookId, book.Title, book.Author, book.Price, book.Quantity, book.Category);
            }
        }
        else
        {
            Console.WriteLine("Книги, соответствующие критериям поиска, не найдены.");
        }
    }

    static void ReturnBook()
    {
        Console.Write("\nВведите ID книги для возврата: ");
        int bookId = Convert.ToInt32(Console.ReadLine());

        BorrowDetails details = borrowDetails.FirstOrDefault(b => b.BookId == bookId);

        if (details != null)
        {
            Book book = books.FirstOrDefault(b => b.BookId == details.BookId);
            if (book != null)
            {
                book.Quantity++;
            }

            borrowDetails.Remove(details);

            Console.WriteLine("Книга успешно возвращена.");
        }
        else
        {
            Console.WriteLine("Запись о книге для аренды с ID {0} не найдена.", bookId);
        }
    }

    static void InitializeData()
    {
        users.Add(new User { Username = "admin", Password = "1234" });
        users.Add(new User { Username = "user", Password = "1234" });

        books.Add(new Book { BookId = 1, Title = "Великий Гетсби", Author = "Ф. Скотт Фицджеральд", Price = 33, Quantity = 5, Category = BookCategory.Вымысел });
        books.Add(new Book { BookId = 2, Title = "Убить пересмешника", Author = "Харпер Ли", Price = 12, Quantity = 3, Category = BookCategory.Вымысел });
        books.Add(new Book { BookId = 3, Title = "1984", Author = "Джордж Оруэлл", Price = 20, Quantity = 7, Category = BookCategory.Вымысел });
        books.Add(new Book { BookId = 4, Title = "Хоббит", Author = "Дж. Р. Р. Толкин", Price = 1, Quantity = 6, Category = BookCategory.Вымысел });
        books.Add(new Book { BookId = 5, Title = "Сапиенс: Краткая история человечества", Author = "Юваль Ной Харари", Price = 17, Quantity = 4, Category = BookCategory.История });
        books.Add(new Book { BookId = 6, Title = "Война и мир", Author = "Лев Толстой", Price = 25, Quantity = 8, Category = BookCategory.История });
        books.Add(new Book { BookId = 7, Title = "Преступление и наказание", Author = "Фёдор Достоевский", Price = 18, Quantity = 2, Category = BookCategory.Художественная });
        books.Add(new Book { BookId = 8, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Price = 22, Quantity = 6, Category = BookCategory.Фэнтези });
        books.Add(new Book { BookId = 9, Title = "Мертвые души", Author = "Николай Гоголь", Price = 16, Quantity = 3, Category = BookCategory.Художественная });
        books.Add(new Book { BookId = 10, Title = "Анна Каренина", Author = "Лев Толстой", Price = 27, Quantity = 5, Category = BookCategory.Художественная });
        books.Add(new Book { BookId = 11, Title = "Братья Карамазовы", Author = "Фёдор Достоевский", Price = 21, Quantity = 4, Category = BookCategory.Художественная });
        books.Add(new Book { BookId = 12, Title = "Три товарища", Author = "Эрих Мария Ремарк", Price = 19, Quantity = 7, Category = BookCategory.Роман });
        books.Add(new Book { BookId = 13, Title = "Маленький принц", Author = "Антуан де Сент-Экзюпери", Price = 14, Quantity = 9, Category = BookCategory.Фэнтези });
        books.Add(new Book { BookId = 14, Title = "Над пропастью во ржи", Author = "Джером Д. Сэлинджер", Price = 23, Quantity = 10, Category = BookCategory.Художественная });
        books.Add(new Book { BookId = 15, Title = "Сто лет одиночества", Author = "Габриэль Гарсиа Маркес", Price = 20, Quantity = 11, Category = BookCategory.Магический_реализм });
        books.Add(new Book { BookId = 16, Title = "Портрет Дориана Грея", Author = "Оскар Уайльд", Price = 24, Quantity = 12, Category = BookCategory.Фэнтези });
        books.Add(new Book { BookId = 17, Title = "Мастер Гарри Поттер и философский камень", Author = "Джоан Роулинг", Price = 30, Quantity = 13, Category = BookCategory.Фэнтези });
        books.Add(new Book { BookId = 18, Title = "Понедельник начинается в субботу", Author = "Аркадий и Борис Стругацкие", Price = 26, Quantity = 14, Category = BookCategory.Фантастика });
        books.Add(new Book { BookId = 19, Title = "Стоянка лебедей", Author = "Антон Чехов", Price = 15, Quantity = 15, Category = BookCategory.Художественная });
        books.Add(new Book { BookId = 20, Title = "Бойцовский клуб", Author = "Чак Паланик", Price = 28, Quantity = 16, Category = BookCategory.Драма });

    }
}
