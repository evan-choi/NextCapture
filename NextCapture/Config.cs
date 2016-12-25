using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCapture
{
    public static class Config
    {
        public static class Version
        {
            public const int Major = 1;
            public const int Minor = 0;
            public const int Build = 0;
            public const string State = "Beta";

            public static new string ToString()
            {
                var sb = new StringBuilder();

                sb.Append(Major).Append(".")
                  .Append(Minor).Append(".")
                  .Append(Build);

                if (State.Length > 0)
                    sb.Append(" ").Append(State);

                return sb.ToString();
            }
        }
    }
}