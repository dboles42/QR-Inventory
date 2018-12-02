﻿using System;
using System.Collections.Generic;

namespace InventoryManagement
{
    /// <summary>
    /// Class for the inventory
    /// </summary>
    public class Inventory
    {
        public List<Asset> listOfAssets = new List<Asset>();
        public int NumberOfAssets { get; set; }

        /// <summary>
        /// Default constructor for the inventory class
        /// </summary>
        public Inventory()
        {
            NumberOfAssets = 0;
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
        public void AddAsset(string Name = "None", string Description = "None", double Price = 0, int ModelNumber = 0, int SerialNumber = 0, bool CheckIn = false){
            listOfAssets.Add(new Asset(Name, Description, Price, ModelNumber, SerialNumber, CheckIn));
            listOfAssets.Sort();
            NumberOfAssets++;
        }

        /// <summary>
        /// Removes an asset from the inventory
        /// </summary>
        /// <param name="index">Index of the asset to be removed</param>
        public void RemoveAsset(int index){
            listOfAssets.RemoveAt(index);
            NumberOfAssets--;
        }

        /// <summary>
        /// Clears the inventory.
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
    }

   
}
