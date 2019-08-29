﻿using System;
using System.Security.Cryptography;
using LMS.Services.ModelProviders.Contracts;

namespace LMS.Services.ModelProviders
{
    public class IsbnGenerator : IIsbnGenerator
    {
        private string isbn = "978-1-940313-09-";
        public IsbnGenerator()
        {
        }
        public string GenerateISBN()
        {
            var RandomGenerator = new RNGCryptoServiceProvider();
            byte[] rno = new byte[8];
            RandomGenerator.GetBytes(rno);
            int randomNumber = BitConverter.ToInt32(rno, 0);
            return isbn + randomNumber.ToString();
        }
    }
}