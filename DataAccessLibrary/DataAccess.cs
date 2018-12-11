using System;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AssetObj;
using UserObj;

namespace DataAccessLibrary
{
    /// <summary>
    /// Database class
    /// </summary>
    public class DataAccess : IComparable<DataAccess>
    {
        private string DB { get; set; }
        private string WorkingTable { get; set; }
        private string AssetsInv { get; set; }
        private string LoginInfo { get; set; }

        public DataAccess()
        {
            DB = "InventoryDB.db";
            AssetsInv = "AssetsInventory10";
            LoginInfo = "LoginInfo";
            WorkingTable = "Default";

            InitializeDatabase();
        }

        public DataAccess(string WorkingTable)
        {
            DB = "InventoryDB.db";
            AssetsInv = "AssetsInventory10";
            LoginInfo = "LoginInfo";
            if (WorkingTable.CompareTo("Asset") == 0)
            {
                this.WorkingTable = AssetsInv;
            }
            else if (WorkingTable.CompareTo("Login") == 0)
            {
                this.WorkingTable = LoginInfo;
            }
            else
                this.WorkingTable = "Default";

            InitializeDatabase();
        }

        /// <summary>
        /// Initializes the database and creates AssetsInv table if the table does not exist
        /// </summary>
        private void InitializeDatabase()
        {
            using (SqliteConnection DBase =
                new SqliteConnection("Filename=" + DB))
            {
                DBase.Open();

                String CreateAssetTableQuery = "CREATE TABLE IF NOT " +
                    "EXISTS " + AssetsInv + " (" +
                    "AssetName VARCHAR(255)," +
                    "Description VARCHAR(255)," +
                    "Price VARCHAR(255)," +
                    "IDNumber VARCHAR(255)," +
                    "SerialNumber VARCHAR(255)," +
                    "ModelNumber INT NOT NULL," +
                    "CheckIn INT NOT NULL)";

                String CreateLoginTableQuery = "CREATE TABLE IF NOT " +
                    "EXISTS " + LoginInfo + " (" +
                    "Username VARCHAR(255)," +
                    "Password VARCHAR(255)," +
                    "ReadPermission INT NOT NULL," +
                    "WritePermission INT NOT NULL," +
                    "RemovePermission INT NOT NULL)";

                SqliteCommand createAssetTable = new SqliteCommand(CreateAssetTableQuery, DBase);
                SqliteCommand createLoginTable = new SqliteCommand(CreateLoginTableQuery, DBase);

                createAssetTable.ExecuteReader();
                createLoginTable.ExecuteReader();

                createAssetTable.Dispose();
                createLoginTable.Dispose();
                DBase.Dispose();
                DBase.Close();
            }
        }

        /// <summary>
        /// Inserts a list of Assets into the AssetsInv table of the database
        /// </summary>
        /// <param name="AssetList"></param>
        public void InsertListToTable(List<Asset> AssetList)
        {
            if (WorkingTable.CompareTo(AssetsInv) == 0)
            {
                using (SqliteConnection DBase =
                new SqliteConnection("Filename=" + DB))
                {
                    DBase.Open();

                    foreach (Asset currAsset in AssetList)
                    {
                        SqliteCommand insertCommand = new SqliteCommand();
                        insertCommand.Connection = DBase;
                        insertCommand.CommandText = $"INSERT INTO " + AssetsInv + $" VALUES (@Name, @Description, @Price, @IDnumber, " +
                                                                                   "@SerialNumber, @ModelNumber, @CheckIn);";
                        insertCommand.Parameters.AddWithValue("@Name", currAsset.Name);
                        insertCommand.Parameters.AddWithValue("@Description", currAsset.Description);
                        insertCommand.Parameters.AddWithValue("@IDnumber", currAsset.IDnumber.Replace("-","_"));
                        insertCommand.Parameters.AddWithValue("@CheckIn", currAsset.CheckIn);
                        insertCommand.Parameters.AddWithValue("@Price", currAsset.Price.Replace(".","_"));
                        insertCommand.Parameters.AddWithValue("@ModelNumber", currAsset.ModelNumber);
                        insertCommand.Parameters.AddWithValue("@SerialNumber", currAsset.SerialNumber);

                        insertCommand.ExecuteReader();
                        insertCommand.Dispose();
                    }
                    DBase.Dispose();
                    DBase.Close();
                }
            }
        }

        public void InsertUserToTable(User user)
        {
            if (WorkingTable.CompareTo(LoginInfo) == 0)
            {
                using (SqliteConnection DBase =
                new SqliteConnection("Filename=" + DB))
                {
                    DBase.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = DBase;

                    insertCommand.CommandText = $"INSERT INTO " + LoginInfo + " VALUES (@Username, @Password, @Read, @Write, " +
                                                                                "@Remove);";
                    
                    insertCommand.Parameters.AddWithValue("@Username", user.Username);

                    user.encryption();
                    insertCommand.Parameters.AddWithValue("@Password", user.Password);

                    insertCommand.Parameters.AddWithValue("@Read", user.ReadPermission);
                    insertCommand.Parameters.AddWithValue("@Write", user.WritePermission);
                    insertCommand.Parameters.AddWithValue("@Remove", user.RemovePermission);

                    insertCommand.ExecuteReader();
                    insertCommand.Dispose();
                    DBase.Dispose();
                    DBase.Close();
                }
            }
        }
        /// <summary>
        /// Retrieves all the contents of a table as a list of Assets
        /// </summary>
        /// <returns>List of Assets in the database</returns>
        public List<Asset> getList()    
        {
            List<Asset> listOfAssets = new List<Asset>();
            using (SqliteConnection DBase =
                    new SqliteConnection("Filename=" + DB))
            {
                DBase.Open();

                SqliteCommand selectCommand = new SqliteCommand("SELECT * From " + AssetsInv, DBase); //SQL query

                SqliteDataReader query = selectCommand.ExecuteReader();

                //Reads All rows in the AssetsInv table 
                while (query.Read())
                {
                    Asset temp = new Asset();
                    temp.Name = query.GetString(0);
                    temp.Description = query.GetString(1);
                    temp.Price = query.GetString(2).Replace("_", ".");
                    temp.IDnumber = query.GetString(3).Replace("_", "-");
                    temp.SerialNumber = query.GetString(4);
                    temp.ModelNumber = query.GetInt32(5);
                    temp.CheckIn = query.GetBoolean(6);

                    listOfAssets.Add(temp);
                }
                selectCommand.Dispose();
                query.Close();
                DBase.Dispose();
                DBase.Close();
            }
            return listOfAssets;
        }

        public User getUser(string Username, string Password)
        {
            User temp = new User();

            using (SqliteConnection DBase =
                    new SqliteConnection("Filename=" + DB))
            {
                DBase.Open();

                SqliteCommand selectCommand = new SqliteCommand("SELECT * FROM " + LoginInfo + " WHERE Username LIKE @Username"); //SQL query
                selectCommand.Connection = DBase;
                selectCommand.Parameters.AddWithValue("@Username", Username);

                SqliteDataReader query = selectCommand.ExecuteReader();

                
                if (query.Read())
                {
                    temp.Username = query.GetString(0);
                    temp.Password = query.GetString(1);
                    temp.ReadPermission = query.GetBoolean(2);
                    temp.WritePermission = query.GetBoolean(3);
                    temp.RemovePermission = query.GetBoolean(4);

                    temp.decryption();

                    if(Password.CompareTo(temp.Password) != 0)
                    {
                        temp = null;
                    }
                }
                else
                {
                    temp = null;
                }
                query.Close();
                selectCommand.Dispose();
                DBase.Close();
                DBase.Dispose();
            }
            return temp;
        }
        /// <summary>
        /// This method removes all rows/contents of the AssetsInv table in the Database
        /// </summary>
        public void RemoveAllRows()
        {
            using (SqliteConnection DBase =
                   new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                SqliteCommand RemoveAllCommand = new SqliteCommand();
                RemoveAllCommand.Connection = DBase;

                RemoveAllCommand.CommandText = $"DELETE FROM " + AssetsInv + ";"; //SQL Query
                RemoveAllCommand.ExecuteReader();

                RemoveAllCommand.Dispose();
                DBase.Close();
                DBase.Dispose();
            }
        }
        public override string ToString()
        {
            return $"| Database: {DB} | Table: {WorkingTable} |"; 
        }

        public int CompareTo(DataAccess other)
        {
            if (other != null)
            {
                return this.WorkingTable.CompareTo(other.WorkingTable);
            }
            else
            {
                throw new ArgumentException("The object passed is invalid");
            }
        }
    }
}