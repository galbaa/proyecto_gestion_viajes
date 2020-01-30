using Nancy.Security;
using System.Collections.Generic;

namespace Gevi.Api.Models
{
    public class UsuarioIdentity : IUserIdentity
    {
        public string UserName => "";
        public IEnumerable<string> Claims => new List<string>();
    }
}