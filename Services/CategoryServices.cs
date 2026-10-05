using Npgsql;
using cpwp_pr_1.Models;

namespace cpwp_pr_1.Services;

public class CategoryService
{
    public void Add(string name)
    {
        using var conn = Db.GetConnection();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO categories (name) VALUES (@n)", conn);
        cmd.Parameters.AddWithValue("n", name);
        try
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine("Категория добавлена.");
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            Console.WriteLine("Такая категория уже существует.");
        }
    }

    public List<Category> GetAll()
    {
        var list = new List<Category>();
        using var conn = Db.GetConnection();
        using var cmd = new NpgsqlCommand(
            "SELECT id, name FROM categories ORDER BY id", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Category
            {
                Id = r.GetInt32(0),
                Name = r.GetString(1),
            });
        }
        return list;
    }

    public void Delete(int id)
    {
        using var conn = Db.GetConnection();
        using var cmd = new NpgsqlCommand("DELETE FROM categories WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("id", id);
        try
        {
            var n = cmd.ExecuteNonQuery();
            Console.WriteLine(n > 0 ? "Удалено." : "Категория не найдена.");
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            Console.WriteLine("Нельзя удалить: к категории привязаны товары.");
        }
    }
}