using System;

namespace AssetObj
{
    /// <summary>
    /// Class for assets 
    /// </summary>
    public class Asset : IComparable <Asset>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IDnumber { get; set; }
        public double Price { get; set; }
        public int ModelNumber { get; set; }
        public int SerialNumber { get; set; }
        public bool CheckIn { get; set; }

        /// <summary>
        /// Default constructor that initializes a new instance of the class.
        /// </summary>
        public Asset()
        {
            this.Name = "Empty";
            this.Description = "Empty";
            this.IDnumber = (Guid.NewGuid().ToString()).Replace("-", "_"); // The char "-" is an escape sequence in SQL and not just a char so it bugs the query "_" is a char wildcard
            this.Price = 0;
            this.ModelNumber = 0;
            this.SerialNumber = 0;
            this.CheckIn = false;
        }

        /// <summary>
        /// Explicit value constructor that Initializes a new instance of the asset class.
        /// </summary>
        /// <param name="Name">Name.</param>
        /// <param name="Description">Description.</param>
        /// <param name="ModelNumber">Model number.</param>
        /// <param name="SerialNumber">Serial number.</param>
        /// <param name="CheckIn">If set to <c>true</c> check in.</param>
        public Asset(string Name, string Description, double Price, int ModelNumber, int SerialNumber, bool CheckIn)
        {
            this.Name = Name;
            this.Description = Description;
            this.IDnumber = (Guid.NewGuid().ToString()).Replace("-", "_"); // The char "-" is an escape sequence in SQL and not just a char so it bugs the query "_" is a char wildcard
            this.Price = Price;
            this.ModelNumber = ModelNumber;
            this.SerialNumber = SerialNumber;
            this.CheckIn = CheckIn;
        }

        /// <summary>
        /// Compares two assets based on their ID number
        /// </summary>
        /// <returns>Returns 1 if the current assets ID number is greater than the ID number of the asset
        /// being compared to, -1 if less than, and 0 if equal to</returns>
        /// <param name="obj">Object.</param>
        public int CompareTo(Asset that)
        {
            if (that == null)
                throw new ArgumentException("The object passed is invalid");
            //Compare the two ID numbers of the two assets
            return string.Compare(this.Name, that.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Serves as a hash function for an asset object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current asset.
        /// </summary>
        /// <returns>A string that represents the current asset.</returns>
        public override string ToString()
        {
            return $"name: {this.Name}\nDescription: {this.Description}\nID number: {this.IDnumber}\nPrice: {this.Price}\n" +
                $"Model Number: {this.ModelNumber}\nSerial Number: {this.SerialNumber}\nCheck In: {this.CheckIn}\n";
        }
    }
}