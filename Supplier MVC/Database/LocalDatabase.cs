using System.Data.SQLite;
using System.Diagnostics;

namespace Supplier_MVC.Database
{
    public static class LocalDatabase
    {
        private static Random DbRandom = new Random();

        /// <summary>
        /// Table consisting of members.
        /// </summary>
        public const string SuppliersTable = "SuppliersINV";
        public const string SuppliersExtensionTable = "SuppliersExtension";

        /// <summary>
        /// Table consisting of all transactions based on members via their ID.
        /// </summary>
        public const string ProductsTable = "ProductsINV";


        /// <summary>
        /// The main SQLite connection object.
        /// </summary>
        private static SQLiteConnection Connection;
        public static async Task<bool> ConnectAsync()
        {
            try
            {
                string cs = "Data Source=Database.db";
                string stm = "SELECT SQLITE_VERSION()";

                Connection = new SQLiteConnection(cs);
                await Connection.OpenAsync();

                var cmd = new SQLiteCommand(stm, Connection);
                string version = (await cmd.ExecuteScalarAsync()).ToString() ?? "Unkown";

                Debug.WriteLine($"[localdatabase] version: {version}");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        public static bool InitializeTables() =>
             CreateTableIfNotExist(SuppliersTable,
                 "SupplierID longEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                 "CompanyName NCHAR(100), " +
                 "Address NCHAR(200), " +
                 "Representative NCHAR(100), " +
                 "ContactNo NCHAR(50)," +
                 "DateAdded DATETIME," +
                 "DateModified DATETIME") &&
             CreateTableIfNotExist(ProductsTable,
                 "ProductID longEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                 "Name NCHAR(50), " +
                 "Description NCHAR(200), " +
                 "Qty longEGER, " +
                 "Unit NCHAR(50)," +
                 "DateAdded DATETIME," +
                 "DateModified DATETIME") &&
             CreateTableIfNotExist(SuppliersExtensionTable,
                 "CompanyName TEXT, Password TEXT");

        public static bool InsertProduct(Models.Product prod)
        {
            if (prod.ProductId > 0)
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"UPDATE {ProductsTable} SET Name=@a, Description=@b, Unit=@c, DateModified=@d" +
                    $" WHERE ProductID=@e";

                cmd.Parameters.AddWithValue("a", prod.Name);
                cmd.Parameters.AddWithValue("b", prod.Description);
                cmd.Parameters.AddWithValue("c", prod.Unit);
                cmd.Parameters.AddWithValue("d", DateTime.Now);
                cmd.Parameters.AddWithValue("e", prod.ProductId);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT longO {ProductsTable}" +
                    $"(Name, Description, Qty, Unit, DateAdded, DateModified) " +
                    $"VALUES(@a, @b, @c, @d, @e, @f)";

                cmd.Parameters.AddWithValue("a", prod.Name);
                cmd.Parameters.AddWithValue("b", prod.Description);
                cmd.Parameters.AddWithValue("c", 0);
                cmd.Parameters.AddWithValue("d", prod.Unit);
                cmd.Parameters.AddWithValue("e", DateTime.Now);
                cmd.Parameters.AddWithValue("f", DateTime.Now);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            return true;
        }


        public static IEnumerable<(string Name, string Password)> SelectSuppliersMetadata()
        {
            List<(string Name, string Password)> materials = new List<(string Name, string Password)>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {SuppliersExtensionTable}", Connection).ExecuteReader();

            while (rdr.Read())
            {
                var Name = GetElseDefault<string>(rdr, "CompanyName");
                var Password = GetElseDefault<string>(rdr, "Password");

                materials.Add((Name, Password));
            }

            return materials;
        }


        public static IEnumerable<Models.Product> SelectProducts()
        {
            List<Models.Product> materials = new List<Models.Product>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {ProductsTable}", Connection).ExecuteReader();

            while (rdr.Read())
            {
                var ProductID = GetElseDefault<long>(rdr, "ProductID");
                var ProductName = GetElseDefault<string>(rdr, "Name");
                var Description = GetElseDefault<string>(rdr, "Description");
                var Quantity = GetElseDefault<long>(rdr, "Qty");
                var Unit = GetElseDefault<string>(rdr, "Unit");
                var DateAdded = GetElseDefault<DateTime>(rdr, "DateAdded");
                var DateModified = GetElseDefault<DateTime>(rdr, "DateModified");

                materials.Add(new Models.Product
                {
                    ProductId = ProductID,
                    Name = ProductName,
                    Description = Description,
                    Quantity = Quantity,
                    Unit = Unit,
                    DateAdded = DateAdded,
                    DateModified = DateModified
                });
            }

            return materials;
        }
        public static IEnumerable<Models.Supplier> SelectSuppliers()
        {
            List<Models.Supplier> materials = new List<Models.Supplier>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {SuppliersTable}", Connection).ExecuteReader();

            while (rdr.Read())
            {
                var SupplierID = GetElseDefault<long>(rdr, "SupplierID");
                var CompanyName = GetElseDefault<string>(rdr, "CompanyName");
                var Address = GetElseDefault<string>(rdr, "Address");
                var Representative = GetElseDefault<string>(rdr, "Representative");
                var ContactNo = GetElseDefault<string>(rdr, "ContactNo");
                var DateAdded = GetElseDefault<DateTime>(rdr, "ContactNo");
                var DateModified = GetElseDefault<DateTime>(rdr, "DateModified");

                materials.Add(new Models.Supplier
                {
                    SupplierID = SupplierID,
                    Address = Address,
                    CompanyName = CompanyName,
                    Representative = Representative,
                    ContactNo = ContactNo,
                    DateAdded = DateAdded,
                    DateModified = DateModified
                });
            }

            return materials;
        }
        public static T GetElseDefault<T>(SQLiteDataReader reader, string name)
        {
            if (reader.IsDBNull(reader.GetOrdinal(name)))
                return default(T);

            var objVal = reader.GetValue(reader.GetOrdinal(name));
            if (objVal is null)
                return default(T);
            try
            {
                return (T)objVal;
            }
            catch { }

            return default(T);
        }

        public static bool InsertSupplier(string name,
            string address,
            string representative,
            string number,
            string password)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT longO {SuppliersTable}" +
                    $"(CompanyName, Address, Representative, ContactNo, DateAdded, DateModified) " +
                    $"VALUES(@a, @b, @c, @d, @e, @f)";

                cmd.Parameters.AddWithValue("a", name);
                cmd.Parameters.AddWithValue("b", address);
                cmd.Parameters.AddWithValue("c", representative);
                cmd.Parameters.AddWithValue("d", number);
                cmd.Parameters.AddWithValue("e", DateTime.Now);
                cmd.Parameters.AddWithValue("f", DateTime.Now);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT longO {SuppliersExtensionTable}" +
                    $"(CompanyName, Password) " +
                    $"VALUES(@a, @b)";

                cmd.Parameters.AddWithValue("a", name);
                cmd.Parameters.AddWithValue("b", password);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();



                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        public static bool Execute(string command)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();

                Debug.WriteLine($"[localdatabase] executed: {command}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        public static bool CreateTableIfNotExist(string tableName, string typeAndNames) =>
            Execute($"CREATE TABLE IF NOT EXISTS {tableName}({typeAndNames})");
    }
}
