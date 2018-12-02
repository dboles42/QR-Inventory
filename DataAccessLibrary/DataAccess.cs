using System;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AssetObj;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        public static void InitializeDatabase()
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

        public static void InsertIntoTable(Asset asset)
        {
            using (SqliteConnection DBase =
                new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = DBase;

                insertCommand.CommandText = $"INSERT INTO AssetsInv VALUES (@Name, @Description, @IDnumber, @CheckIn, " +
                                                                           "@Price, @ModelNumber, @SerialNumber);";

                insertCommand.Parameters.AddWithValue("@Name", asset.Name);
                insertCommand.Parameters.AddWithValue("@Description", asset.Description);
                insertCommand.Parameters.AddWithValue("@IDnumber", asset.IDnumber);
                insertCommand.Parameters.AddWithValue("@CheckIn", asset.CheckIn);
                insertCommand.Parameters.AddWithValue("@Price", asset.Price);
                insertCommand.Parameters.AddWithValue("@ModelNumber", asset.ModelNumber);
                insertCommand.Parameters.AddWithValue("@SerialNumber", asset.SerialNumber);

                insertCommand.ExecuteReader();

                DBase.Close();
            }
        }

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
        public static List<Asset> listOfAssets()    
        {
            List<Asset> listOfAssets = new List<Asset>();
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

                    listOfAssets.Add(temp);
                }

                DBase.Close();
            }
            return listOfAssets;
        }
        public static ObservableCollection<Asset> RetriveAllAssets()
        {
            ObservableCollection<Asset> entries = new ObservableCollection<Asset>();

            using (SqliteConnection DBase =
                new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                //This gets called every time. add testing code in bewteen ->
                //Asset asset = new Asset();
                //RemoveWithAsset(asset); // works
                //InsertIntoTable(asset); // works

                //double a = 1000.50;
                //Asset asset1 = new Asset("Omar's phone", "iPhone 7s", a, 700, 22, true);
                //InsertIntoTable(asset1);
                //Asset asset2 = new Asset("Emilio's phone", "Samsung", a, 500, 33, true);
                //InsertIntoTable(asset2);
                //Asset asset3 = new Asset("David's phone", "Nokia", a, 250, 44, true);
                //InsertIntoTable(asset3);
                //Asset asset4 = new Asset("Chris's phone", "Pixel 2", a, 707, 55, true);
                //InsertIntoTable(asset4);
                //Asset asset5 = new Asset("Amack's phone", "iPhone 2", a, 809, 66, true);
                //InsertIntoTable(asset5);

                //List<Asset> ListOfCurr = new List<Asset>();
                //ListOfCurr = listOfAssets();
                //RemoveWithIDNum(ListOfCurr[0].IDnumber);

                //RemoveAllRows();
                // <-
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

        public static void RemoveAllRows()
        {
            using (SqliteConnection DBase =
                   new SqliteConnection("Filename=InventoryDB.db"))
            {
                DBase.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = DBase;

                insertCommand.CommandText = $"DELETE FROM AssetsInv;";
                insertCommand.ExecuteReader();

                DBase.Close();
            }
        }
    }
}
