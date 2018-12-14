using System;
using System.Collections.Generic;
using System.Text;

namespace UserObj
{
    /// <summary>
    /// User class
    /// </summary>
    public class User : IComparable<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool ReadPermission { get; set; }
        public bool WritePermission { get; set; }
        public bool RemovePermission { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            Username = "NoUserSelected";
            Password = "****";
            ReadPermission = false;
            WritePermission = false;
            RemovePermission = false;
        }
        /// <summary>
        /// Explicit constructor.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="ReadPermission"></param>
        /// <param name="WritePermission"></param>
        /// <param name="RemovePermission"></param>
        public User(string Username, string Password, bool ReadPermission, bool WritePermission, bool RemovePermission)
        {
            this.Username = Username;
            this.Password = Password;
            this.ReadPermission = ReadPermission;
            this.WritePermission = WritePermission;
            this.RemovePermission = RemovePermission;
        }
        /// <summary>
        /// Encrypts the password of the user using fibonacci sequence so the password appears different in the database.
        /// I don't think I should fully explain the way this method works for security reasons.
        /// Just kidding it just adds to the 0 index char starting at 2 then the next char in the password gets incremented by 3 then 5 then 8.
        /// </summary>
        public void encryption()
        {
            //encryption using Fibonacci sequence
            int StrLen = this.Password.Length;
            char[] PassCharArray = this.Password.ToCharArray();

            uint FibonacciA = 1;
            uint FibonacciB = 1;
            uint NextNum = 0;
            for(uint i = 0; i < StrLen; i++)
            {
                NextNum = FibonacciA + FibonacciB;
                PassCharArray[i] += (char)NextNum;
                FibonacciA = FibonacciB;
                FibonacciB = NextNum;
            }
            string encrypted = new string(PassCharArray);
            this.Password = encrypted;
        }
        /// <summary>
        /// Decrypts the password of the user using fibonacci sequence so the we can check if the password is the same as what the user enters
        /// Does encryption but backwords starts at the end of the password string and subtracts starting at the furthest fibbonacci sequence the password string ends.
        /// </summary>
        public void decryption()
        {
            //decryption using Fibonacci sequence
            int StrLen = this.Password.Length;
            char[] PassCharArray = this.Password.ToCharArray();

            uint FibonacciA = 1;
            uint FibonacciB = 1;
            uint NextNum = 0;
            for (int i = 0; i < StrLen; i++)
            {
                NextNum = FibonacciA + FibonacciB;
                FibonacciA = FibonacciB;
                FibonacciB = NextNum;
            }

            for(int i = StrLen - 1; 0 <= i; i--)
            {
                PassCharArray[i] -= (char)NextNum;
                NextNum = FibonacciA;
                FibonacciA = FibonacciB - FibonacciA;
                FibonacciB = NextNum;
            }

            string decrypted = new string(PassCharArray);
            this.Password = decrypted;
        }
        /// <summary>
        /// Turns the User object into a orderly and clear readable string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"| Username: {Username} | Password: {Password} | ReadPermission: {ReadPermission} " +
                $"| WritePermission: {WritePermission} | RemovePermission: {RemovePermission} |";
        }
        public int CompareTo(User Other)
        {
            int PermissionsThis = 0;
            int PermissionsOther = 0;

            if(this.ReadPermission == true)
            {
                PermissionsThis++;
            }
            if (Other.ReadPermission == true)
            {
                PermissionsOther++;
            }
            if (this.WritePermission == true)
            {
                PermissionsThis++;
            }
            if (Other.WritePermission == true)
            {
                PermissionsOther++;
            }
            if (this.RemovePermission == true)
            {
                PermissionsThis++;
            }
            if (Other.RemovePermission == true)
            {
                PermissionsOther++;
            }

            if (Other != null)
            {
                return PermissionsThis.CompareTo(PermissionsOther);
            }
            else
                throw new ArgumentException("The object passed is invalid");
        }
    }
}
