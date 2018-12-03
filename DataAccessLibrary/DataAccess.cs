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
