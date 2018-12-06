using System;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AssetObj;

namespace DataAccessLibrary
{
    /// <summary>
    /// Database class
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// Initializes the database and creates AssetsInv table if the table does not exist
        /// </summary>
        public void InitializeDatabase()
        {
            using (SqliteConnection DBase =
                new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                String CreateTableQuery = "CREATE TABLE IF NOT " +
                    "EXISTS AssetsInv (" +
                    "AssetName VARCHAR(255)," +
                    "Description VARCHAR(255)," +
                    "IDNumber VARCHAR(255)," +
                    "CheckIn INT NOT NULL," +
                    "Price FLOAT NOT NULL," +
                    "ModelNumber INT NOT NULL," +
                    "SerialNumber INT NOT NULL)";

                SqliteCommand createTable = new SqliteCommand(CreateTableQuery, DBase);

                createTable.ExecuteReader();
                DBase.Close();
            }
        }
        /// <summary>
        /// Inserts a list of Assets into the AssetsInv table of the database
        /// </summary>
        /// <param name="AssetList"></param>
        public void InsertIntoTable(List<Asset> AssetList)
        {
            using (SqliteConnection DBase =
                new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                foreach (Asset currAsset in AssetList)
                {
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = DBase;

                    insertCommand.CommandText = $"INSERT INTO AssetsInv VALUES (@Name, @Description, @IDnumber, @CheckIn, " +
                                                                               "@Price, @ModelNumber, @SerialNumber);";

                    insertCommand.Parameters.AddWithValue("@Name", currAsset.Name);
                    insertCommand.Parameters.AddWithValue("@Description", currAsset.Description);
                    insertCommand.Parameters.AddWithValue("@IDnumber", currAsset.IDnumber);
                    insertCommand.Parameters.AddWithValue("@CheckIn", currAsset.CheckIn);
                    insertCommand.Parameters.AddWithValue("@Price", currAsset.Price);
                    insertCommand.Parameters.AddWithValue("@ModelNumber", currAsset.ModelNumber);
                    insertCommand.Parameters.AddWithValue("@SerialNumber", currAsset.SerialNumber);

                    insertCommand.ExecuteReader();
                }

                DBase.Close();
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
                    new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                SqliteCommand selectCommand = new SqliteCommand("SELECT * from AssetsInv", DBase); //SQL query

                SqliteDataReader query = selectCommand.ExecuteReader();

                //Reads All rows in the AssetsInv table 
                while (query.Read())
                {
                    Asset temp = new Asset();
                    temp.Name = query.GetString(0);
                    temp.Description = query.GetString(1);
                    temp.IDnumber = query.GetString(2);
                    temp.Price = query.GetDouble(3);
                    temp.ModelNumber = query.GetInt32(4);
                    temp.SerialNumber = query.GetInt32(5);
                    temp.CheckIn = query.GetBoolean(6);

                    listOfAssets.Add(temp);
                }

                DBase.Close();
            }
            return listOfAssets;
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

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = DBase;

                insertCommand.CommandText = $"DELETE FROM AssetsInv;"; //SQL Query
                insertCommand.ExecuteReader();

                DBase.Close();
            }
        }

        /*
        DO NOT USE!!! REVISED DESIGN STATES WE UPDATE THE DATABASE WHEN WE WANT AND NOT CONCURRENTLY. METHODS NOT NEEDED SO UNUSED CODE.
        DO NOT ERASE WORK THAT I DID. THANKS.
        public static void RemoveWithAsset(Asset asset)
        {
            using (SqliteConnection DBase =
                    new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();
                SqliteCommand RemoveAssetCommand = new SqliteCommand();
                RemoveAssetCommand.Connection = DBase;

                RemoveAssetCommand.CommandText = $"DELETE FROM AssetsInv WHERE ModelNumber=@ModelNumber AND SerialNumber=@SerialNumber AND AssetName LIKE @Name";
                RemoveAssetCommand.Parameters.AddWithValue("@Name", asset.Name);
                RemoveAssetCommand.Parameters.AddWithValue("@ModelNumber", asset.ModelNumber);
                RemoveAssetCommand.Parameters.AddWithValue("@SerialNumber", asset.SerialNumber);
                RemoveAssetCommand.ExecuteReader();

                DBase.Close();
            }
        }
        public static void RemoveWithIDNum(string GUID)
        {
            using (SqliteConnection DBase =
                new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();
                SqliteCommand RemoveIDNumCommand = new SqliteCommand();
                RemoveIDNumCommand.Connection = DBase;

                RemoveIDNumCommand.CommandText = $"DELETE FROM AssetsInv WHERE IDNumber LIKE @IDNum";
                RemoveIDNumCommand.Parameters.AddWithValue("@IDNum", GUID);
                RemoveIDNumCommand.ExecuteReader();

                DBase.Close();
            }
        }

        public static ObservableCollection<Asset> RetriveAllAssets()
        {
            ObservableCollection<Asset> entries = new ObservableCollection<Asset>();

            using (SqliteConnection DBase =
                new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from AssetsInv", DBase);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    Asset temp = new Asset();
                    temp.Name = query.GetString(0);
                    temp.Description = query.GetString(1);
                    temp.IDnumber = query.GetString(2);
                    temp.Price = query.GetDouble(3);
                    temp.ModelNumber = query.GetInt32(4);
                    temp.SerialNumber = query.GetInt32(5);
                    temp.CheckIn = query.GetBoolean(6);

                    entries.Add(temp);
                }

                DBase.Close();
            }

            return entries;
        }
        */
    }
}
