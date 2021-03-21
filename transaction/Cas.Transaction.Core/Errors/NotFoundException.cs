using System;
using System.Net;

namespace Cas.Transaction.Core.Errors
{
    public class NotFoundException : Exception
    {
        public NotFoundException(object Errors = null)
        {
            this.Errors = Errors;
        }

        public object Errors { get; }
    }
}
