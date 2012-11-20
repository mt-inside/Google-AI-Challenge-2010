using System;
using System.Collections.Generic;

namespace mtBot
{
    class Pair<T0, T1> : IEquatable<Pair<T0, T1>>
    {
        private readonly T0 m_Left;
        private readonly T1 m_Right;

        public Pair(T0 left, T1 right)
        {
            m_Left = left;
            m_Right = right;
        }

        public T0 Left
        {
            get { return m_Left; }
        }

        public T1 Right
        {
            get { return m_Right; }
        }

        #region IEquatable implimentation

        public bool Equals(Pair<T0, T1> other)
        {
            return EqualityComparer<T0>.Default.Equals(m_Left, other.m_Left) &&
                   EqualityComparer<T1>.Default.Equals(m_Right, other.m_Right);
        }

        public override bool Equals(object obj)
        {
            return (obj is Pair<T0, T1>) ? Equals((Pair<T0, T1>) obj) : false;
        }

        public override int GetHashCode( )
        {
            return m_Left.GetHashCode( ) ^ m_Right.GetHashCode( );
        }

        public static bool operator==(Pair<T0, T1> a, Pair<T0, T1> b)
        {
            return a.Equals(b);
        }
        public static bool operator!=(Pair<T0, T1> a, Pair<T0, T1> b)
        {
            return !a.Equals(b);
        }

        #endregion

        public override string ToString()
        {
            return String.Format("({0},{1})", m_Left, m_Right);
        }
    }
}