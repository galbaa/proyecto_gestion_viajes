﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IHeaderParser
    {
        string GetApiKey(NancyContext context);
    }
}
