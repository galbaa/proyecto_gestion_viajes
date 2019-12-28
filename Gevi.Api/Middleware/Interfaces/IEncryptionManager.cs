using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IEncryptionManager
    {
        string Encryptdata(string password);
        string Decryptdata(string decryptPwd);
    }
}
