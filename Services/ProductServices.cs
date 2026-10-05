using Npgsql;
using cpwp_pr_1.Models;

namespace cpwp_pr_1.Services;

public class ProductService
{
    public void Add(string name, int? categoryId, decimal price, int quantity, int minQty)
    {
        using var conn = Db.GetConnection();
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO products (name, category_id, price, quantity, min_quantity)
              VALUES (@n, @c, @p, @q, @m)", conn);
        cmd.Parameters.AddWithValue("n", name);
        cmd.Parameters.AddWithValue("c", (object?)categoryId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("p", price);
        cmd.Parameters.AddWithValue("q", quantity);
        cmd.Parameters.AddWithValue("m", minQty);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Товар добавлен.");
    }

    public List<Product> GetAll()
{
    var list = new List<Product>();
    using var conn = Db.GetConnection();
    using var cmd = new NpgsqlCommand(
        @"SELECT p.id, p.name, p.category_id, p.quantity, p.price, p.min_quantity, c.name
          FROM products p
          LEFT JOIN categories c ON c.id = p.category_id
          ORDER BY p.id", conn);
    using var r = cmd.ExecuteReader();
    while (r.Read())
    {
        list.Add(new Product
        {
            Id = r.GetInt32(0),
            Name = r.GetString(1),
            CategoryId = r.IsDBNull(2) ? null : r.GetInt32(2),
            Quantity = r.GetInt32(3),
            Price = r.GetDecimal(4),
            MinQuantity = r.GetInt32(5),
            CategoryName = r.IsDBNull(6) ? "-" : r.GetString(6),
        });
    }
    return list;
}

    public List<Product> Search(string q)
{
    var list = new List<Product>();
    using var conn = Db.GetConnection();
    using var cmd = new NpgsqlCommand(
        @"SELECT p.id, p.name, p.category_id, p.quantity, p.price, p.min_quantity, c.name
          FROM products p
          LEFT JOIN categories c ON c.id = p.category_id
          WHERE p.name ILIKE @q
          ORDER BY p.id", conn);
    cmd.Parameters.AddWithValue("q", $"%{q}%");
    using var r = cmd.ExecuteReader();
    while (r.Read())
    {
        list.Add(new Product
        {
            Id = r.GetInt32(0),
            Name = r.GetString(1),
            CategoryId = r.IsDBNull(2) ? null : r.GetInt32(2),
            Quantity = r.GetInt32(3),
            Price = r.GetDecimal(4),
            MinQuantity = r.GetInt32(5),
            CategoryName = r.IsDBNull(6) ? "-" : r.GetString(6),
        });
    }
    return list;
}

    public void Delete(int id)
    {
        using var conn = Db.GetConnection();
        using var cmd = new NpgsqlCommand("DELETE FROM products WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("id", id);
        var n = cmd.ExecuteNonQuery();
        Console.WriteLine(n > 0 ? "Удалено." : "Товар не найден.");
    }
}