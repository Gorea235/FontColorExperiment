using System.Xml.Serialization;

namespace FontColorExperiment.Utils
{
    /// <summary>
    /// Simulates the System.Windows.Media.Color class
    /// </summary>
    public struct Color
    {
        [XmlAttribute]
        public byte A { get; set; }
        [XmlAttribute]
        public byte R { get; set; }
        [XmlAttribute]
        public byte G { get; set; }
        [XmlAttribute]
        public byte B { get; set; }

        public Color(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        // For use in the question editor program
#if !NETCOREAPP1_1
        public static implicit operator Color(System.Windows.Media.Color color)
        {
            return new Color(color.A, color.R, color.G, color.B);
        }

        public static implicit operator System.Windows.Media.Color(Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
#endif
    }
}
