using System;

namespace Cas.Account.Core.Errors
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
