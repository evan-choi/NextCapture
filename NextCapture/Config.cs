using System.Text;

using d = System.Drawing;

namespace NextCapture
{
    public static class Config
    {
        public const string AppName = "NextCapture";

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

        public static class Color
        {
            public static d.Color Accent = d.Color.FromArgb(239, 142, 142);
            public static d.Color Disabled = d.Color.FromArgb(55, 55, 55);
        }
    }
}