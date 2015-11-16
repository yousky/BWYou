using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Crypt.Algorithms.Hashs
{
    public class SHA512 : Hash
    {
        public SHA512()
            : base(new SHA512CryptoServiceProvider())
        {

        }
    }
}
