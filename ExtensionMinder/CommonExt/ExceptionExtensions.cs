using System;
using System.Runtime.ExceptionServices;

namespace ExtensionMinder.CommonExt
{
    public static class ExceptionExtensions
    {
        public static ExceptionDispatchInfo UnrollDynamicallyInvokedException(this Exception ex)
        {
            if (ex == null) throw new NullReferenceException("UnrollDynamicallyInvokedException given null exception");
            var inner = ex.InnerException;
            while (inner?.InnerException != null)
            {
                inner = inner.InnerException;
            }

            if (inner != null)
            {
                var dispatchInfo = ExceptionDispatchInfo.Capture(inner);
                return dispatchInfo;
            }
            return ExceptionDispatchInfo.Capture(ex);
        }

      public static Exception GetInnerMostException(this Exception ex)
      {
        Exception innerMostException = ex;
        while (innerMostException.InnerException != null)
          innerMostException = innerMostException.InnerException;

        return innerMostException;
      }
  }
}