using System;
using System.Collections.Generic;

namespace mtBot
{
    class Memoiser<TArg, TRet>
    {
        private readonly Func<TArg, TRet> m_InnerFunction;
        private readonly IDictionary<TArg, TRet> m_Results = new Dictionary<TArg, TRet>();

        public Memoiser( Func<TArg, TRet> innerFunction )
        {
            m_InnerFunction = innerFunction;
        }

        public TRet this[ TArg arg ]
        {
            get
            {
                TRet ret;

                if (!m_Results.TryGetValue(arg, out ret))
                {
                    ret = m_InnerFunction(arg);
                    m_Results.Add(arg, ret);
                }

                return ret;
            }
        }
    }
}
