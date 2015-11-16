using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Crypt.Algorithms.Hashs
{
    public class SHA256 : Hash
    {
        public SHA256()
            : base(new SHA256CryptoServiceProvider())
        {

        }
    }
}
