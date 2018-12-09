using System;
using System.Collections.Generic;
using AssetObj;
using System.Collections.ObjectModel;
using DataAccessLibrary;
namespace InventoryManagement
{
    /// <summary>
    /// Class for the inventory
    /// </summary>
    public class Inventory : IComparable<Inventory>
    {
        public List<Asset> listOfAssets { get; set; } = new List<Asset>();
        public int NumberOfAssets { get; set; }
        DataAccess DataAccessKey = new DataAccess();
        /// <summary>
        /// Default constructor for the inventory class
        /// </summary>
        public Inventory()
        {
            NumberOfAssets = 0;
            listOfAssets = DataAccessKey.getList();
        }

        /// <summary>
        /// Sorts the inventory based on the unique ID number for each asset
        /// </summary>
        public void SortInventory()
        {
            listOfAssets.Sort();
        }

        /// <summary>
        /// Adds an asset to the inventory
        /// </summary>
        /// <param name="Name">Name.</param>
        /// <param name="Description">Description.</param>
        /// <param name="ModelNumber">Model number.</param>
        /// <param name="SerialNumber">Serial number.</param>
        /// <param name="CheckIn">If set to <c>true</c> check in.</param>
        public void AddAsset(string Name = "None", string Description = "None", double Price = 0, int ModelNumber = 0, string SerialNumber = "none", bool CheckIn = false){
            listOfAssets.Add(new Asset(Name, Description, Price, ModelNumber, SerialNumber, CheckIn));
            listOfAssets.Sort();
            NumberOfAssets++;
        }

        /// <summary>
        /// Removes an asset from the inventory
        /// </summary>
        /// <param name="index">Index of the asset to be removed</param>
        public void RemoveAsset(Asset A){
            listOfAssets.Remove(A);
            NumberOfAssets--;
        }

        /// <summary>
        /// Clears the inventory
        /// </summary>
        public void ClearInventory(){
            listOfAssets.Clear();
            NumberOfAssets = 0;
        }

        /// <summary>
        /// Finds the total value for all assets in the inventory
        /// </summary>
        /// <returns>The total value for the inventory</returns>
        public double FindTotalValue(){
            double TotalValue = 0;
            foreach (Asset A in listOfAssets){
                TotalValue += A.Price;
            }
            return TotalValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="that">The inventory we are comparing with the original</param>
        /// <returns>Returns 1 if the current inventory has more assets than the other inventory, -1 if it has less, and 0 if they have the
        /// same number of assets</returns>
        public int CompareTo(Inventory that)
        {
            if (that == null)
                throw new ArgumentException("The object passed is invalid");
            //Compare the number of assets each inventory holds
            return this.NumberOfAssets.CompareTo(that.NumberOfAssets);
        }

        /// <summary>
        /// Serves as a hash function for an inventory object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current inventory
        /// </summary>
        /// <returns>A string that represents the current inventory.</returns>
        public override string ToString()
        {
            string s = "";
            foreach (Asset A in listOfAssets)
            {
                s += A.ToString();
                s += "****************************************\n";
            }
            return s;
        }

        /// <summary>
        /// Finds an in the list asset with a specific ID
        /// </summary>
        /// <param name="assetID"></param>
        /// <returns>Returns the asset with the requested ID</returns>
        public Asset FindAsset(string assetID)
        {
            return listOfAssets.Find(item => item.IDnumber == assetID);
        }

        /// <summary>
        /// Finds the index of an asset in the list of assets
        /// </summary>
        /// <param name="A"></param>
        /// <returns>An int that is the index of the asset in the list</returns>
        public int FindIndex(Asset A)
        {
            return listOfAssets.IndexOf(A);
        }

        /// <summary>
        /// Retrieves all assets in the inventoryList and places them in an observable collection
        /// </summary>
        /// <returns>An observable collection</returns>
        public ObservableCollection<Asset> RetriveAllAssets()
        {
            ObservableCollection<Asset> entries = new ObservableCollection<Asset>();
            foreach (Asset A in listOfAssets)
            {
                entries.Add(A);
            }
            return entries;
        }
    }
}