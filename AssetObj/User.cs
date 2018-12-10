using System;
using System.Collections.Generic;
using System.Text;

namespace UserObj
{
    public class User : IComparable<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool ReadPermission { get; set; }
        public bool WritePermission { get; set; }
        public bool RemovePermission { get; set; }

        public User()
        {
            Username = "NoUserSelected";
            Password = "****";
            ReadPermission = false;
            WritePermission = false;
            RemovePermission = false;
        }

        public User(string Username, string Password, bool ReadPermission, bool WritePermission, bool RemovePermission)
        {
            this.Username = Username;
            this.Password = Password;
            this.ReadPermission = ReadPermission;
            this.WritePermission = WritePermission;
            this.RemovePermission = RemovePermission;
        }

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
