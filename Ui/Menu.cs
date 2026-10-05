using cpwp_pr_1.Services;

namespace cpwp_pr_1.Ui;

public class Menu
{
    private readonly ProductService _products = new();
    private readonly OperationService _ops = new();
    private readonly ReportService _reports = new();
    private readonly CategoryService _categories = new();

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\nСкладской учет");
            Console.WriteLine("-----");
            Console.WriteLine("1. Товары");
            Console.WriteLine("2. Приход");
            Console.WriteLine("3. Расход");
            Console.WriteLine("4. Отчет");
            Console.WriteLine("5. Категории");
            Console.WriteLine("0. Выход");
            Console.Write("> ");
            var c = Console.ReadLine();

            switch (c)
            {
                case "1": ProductsMenu(); break;
                case "2": IncomeMenu(); break;
                case "3": OutcomeMenu(); break;
                case "4": ReportMenu(); break;
                case "5": CategoriesMenu(); break;
                case "0": return;
                default: Console.WriteLine("Неверный пункт."); break;
            }
        }
    }

    private void CategoriesMenu()
{
    while (true)
    {
        Console.WriteLine("\n5. Категории");
        Console.WriteLine("-----------");
        Console.WriteLine("1. Добавить");
        Console.WriteLine("2. Удалить");
        Console.WriteLine("3. Посмотреть все");
        Console.WriteLine("0. Назад");
        Console.Write("> ");
        var c = Console.ReadLine();

        switch (c)
        {
            case "1": AddCategory(); break;
            case "2": DeleteCategory(); break;
            case "3": ShowCategories(); break;
            case "0": return;
            default: Console.WriteLine("Неверный пункт."); break;
        }
    }
}

private void AddCategory()
{
    Console.Write("Название категории: ");
    var name = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Пустое имя."); return; }
    _categories.Add(name);
}

private void DeleteCategory()
{
    Console.Write("ID категории: ");
    if (int.TryParse(Console.ReadLine(), out var id)) _categories.Delete(id);
    else Console.WriteLine("Неверный ID.");
}

private void ShowCategories()
{
    Console.WriteLine("\nID | Название");
    Console.WriteLine("----------------");
    foreach (var c in _categories.GetAll())
        Console.WriteLine($"{c.Id} | {c.Name}");
}
    private void ProductsMenu()
    {
        while (true)
        {
            Console.WriteLine("\n1. Товары");
            Console.WriteLine("-----------");
            Console.WriteLine("1. Добавить");
            Console.WriteLine("2. Удалить");
            Console.WriteLine("3. Посмотреть все товары");
            Console.WriteLine("4. Поиск");
            Console.WriteLine("0. Назад");
            Console.Write("> ");
            var c = Console.ReadLine();

            switch (c)
            {
                case "1": AddProduct(); break;
                case "2": DeleteProduct(); break;
                case "3": ShowProducts(); break;
                case "4": SearchProducts(); break;
                case "0": return;
                default: Console.WriteLine("Неверный пункт."); break;
            }
        }
    }

    private void AddProduct()
{
    Console.WriteLine("\nДоступные категории:");
    foreach (var c in _categories.GetAll())
        Console.WriteLine($"  {c.Id} — {c.Name}");

    Console.Write("Название: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("ID категории (Enter — без категории): ");
    var catStr = Console.ReadLine();
    int? catId = null;
    if (!string.IsNullOrWhiteSpace(catStr) && int.TryParse(catStr, out var cid))
        catId = cid;

    Console.Write("Цена: ");
    if (!decimal.TryParse(Console.ReadLine(), out var price)) { Console.WriteLine("Неверная цена."); return; }

    Console.Write("Количество: ");
    if (!int.TryParse(Console.ReadLine(), out var qty)) { Console.WriteLine("Неверное количество."); return; }

    Console.Write("Мин. остаток: ");
    if (!int.TryParse(Console.ReadLine(), out var min)) { Console.WriteLine("Неверное число."); return; }

    _products.Add(name, catId, price, qty, min);
}

    private void DeleteProduct()
    {
        Console.Write("ID товара: ");
        if (int.TryParse(Console.ReadLine(), out var id)) _products.Delete(id);
        else Console.WriteLine("Неверный ID.");
    }

    private void ShowProducts()
{
    Console.WriteLine("\nID | Название | Категория | Кол-во | Цена | Мин");
    Console.WriteLine("---------------------------------------------------------");
    foreach (var p in _products.GetAll())
        Console.WriteLine($"{p.Id} | {p.Name} | {p.CategoryName} | {p.Quantity} | {p.Price} | {p.MinQuantity}");
}
    private void SearchProducts()
{
    Console.Write("Поиск: ");
    var q = Console.ReadLine() ?? "";
    foreach (var p in _products.Search(q))
        Console.WriteLine($"{p.Id} | {p.Name} | {p.CategoryName} | {p.Quantity} | {p.Price}");
}

    private void IncomeMenu()
    {
        Console.Write("ID товара: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Неверный ID."); return; }
        Console.Write("Количество: ");
        if (!int.TryParse(Console.ReadLine(), out var qty)) { Console.WriteLine("Неверное количество."); return; }
        _ops.Income(id, qty);
    }

    private void OutcomeMenu()
    {
        Console.Write("ID товара: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Неверный ID."); return; }
        Console.Write("Количество: ");
        if (!int.TryParse(Console.ReadLine(), out var qty)) { Console.WriteLine("Неверное количество."); return; }
        _ops.Outcome(id, qty);
    }

    private void ReportMenu()
    {
        Console.Write("Введите дату (дд.мм.гггг): ");
        var s = Console.ReadLine();
        if (DateTime.TryParse(s, out var d)) _reports.DailyReport(d);
        else Console.WriteLine("Неверная дата.");
    }
}