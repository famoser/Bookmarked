using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Services.Interfaces;

namespace Famoser.Bookmarked.Presentation.Webpage.Platform
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// cheap random number generator
        /// may want to replace this with a cryptographically secure version like in the Universal project
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public uint GenerateRandomNumber(uint min, uint max)
        {
            return (uint)_random.Next((int)min, (int)max);
        }
    }
}
