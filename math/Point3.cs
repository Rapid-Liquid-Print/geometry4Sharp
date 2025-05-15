using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g4
{
    public struct Point3d : IEquatable<Point3d>
    {
        public double x, y, z;

        public Point3d(double f) { x = y = z = f; }
        public Point3d(double x, double y, double z) { this.x = x; this.y = y; this.z = z; }
        public Point3d(double[] v) { x = v[0]; y = v[1]; z = v[2]; }
        public Point3d(Vector3d v) { x = v.x; y = v.y; z = v.z; }
        public Point3d(Vector3f v) { x = v.x; y = v.y; z = v.z; }

        public static readonly Point3d Origin = new Point3d(0, 0, 0);
        public static readonly Point3d MaxValue = new Point3d(double.MaxValue, double.MaxValue, double.MaxValue);
        public static readonly Point3d MinValue = new Point3d(double.MinValue, double.MinValue, double.MinValue);

        public double this[int index]
        {
            get => (index == 0) ? x : (index == 1) ? y : z;
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else z = value;
            }
        }

        public double DistanceTo(Point3d other)
        {
            double dx = x - other.x;
            double dy = y - other.y;
            double dz = z - other.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public double SquaredDistanceTo(Point3d other)
        {
            double dx = x - other.x;
            double dy = y - other.y;
            double dz = z - other.z;
            return dx * dx + dy * dy + dz * dz;
        }

        public Point3d Lerp(Point3d other, double t)
        {
            double s = 1 - t;
            return new Point3d(s * x + t * other.x, s * y + t * other.y, s * z + t * other.z);
        }

        public Point3d Midpoint(Point3d other)
        {
            return new Point3d((x + other.x) * 0.5, (y + other.y) * 0.5, (z + other.z) * 0.5);
        }

        public Vector3d ToVector3d()
        {
            return new Vector3d(x, y, z);
        }

        // Operators
        public static Vector3d operator -(Point3d a, Point3d b)
        {
            return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Point3d operator +(Point3d p, Vector3d v)
        {
            return new Point3d(p.x + v.x, p.y + v.y, p.z + v.z);
        }

        public static Point3d operator -(Point3d p, Vector3d v)
        {
            return new Point3d(p.x - v.x, p.y - v.y, p.z - v.z);
        }

        public static bool operator ==(Point3d a, Point3d b) => a.Equals(b);
        public static bool operator !=(Point3d a, Point3d b) => !a.Equals(b);

        public bool Equals(Point3d other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj)
        {
            return obj is Point3d other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                hash = hash * 23 + z.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"({x:F6}, {y:F6}, {z:F6})";
        }

        public string ToString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)})";
        }
    }

    public struct Point3f : IEquatable<Point3f>
    {
        public float x, y, z;

        public Point3f(float f) { x = y = z = f; }
        public Point3f(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public Point3f(float[] v) { x = v[0]; y = v[1]; z = v[2]; }
        public Point3f(Vector3f v) { x = v.x; y = v.y; z = v.z; }

        public static readonly Point3f Origin = new Point3f(0, 0, 0);
        public static readonly Point3f MaxValue = new Point3f(float.MaxValue, float.MaxValue, float.MaxValue);
        public static readonly Point3f MinValue = new Point3f(float.MinValue, float.MinValue, float.MinValue);

        public float this[int index]
        {
            get => (index == 0) ? x : (index == 1) ? y : z;
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else z = value;
            }
        }

        public float DistanceTo(Point3f other)
        {
            float dx = x - other.x;
            float dy = y - other.y;
            float dz = z - other.z;
            return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public float SquaredDistanceTo(Point3f other)
        {
            float dx = x - other.x;
            float dy = y - other.y;
            float dz = z - other.z;
            return dx * dx + dy * dy + dz * dz;
        }
        public Point3f Lerp(Point3f other, float t)
        {
            float s = 1 - t;
            return new Point3f(s * x + t * other.x, s * y + t * other.y, s * z + t * other.z);
        }
        public Point3f Midpoint(Point3f other)
        {
            return new Point3f((x + other.x) * 0.5f, (y + other.y) * 0.5f, (z + other.z) * 0.5f);
        }
        public Vector3f ToVector3f()
        {
            return new Vector3f(x, y, z);
        }
        // Operators
        public static Vector3f operator -(Point3f a, Point3f b)
        {
            return new Vector3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Point3f operator +(Point3f p, Vector3f v)
        {
            return new Point3f(p.x + v.x, p.y + v.y, p.z + v.z);
        }
        public static Point3f operator -(Point3f p, Vector3f v)
        {
            return new Point3f(p.x - v.x, p.y - v.y, p.z - v.z);
        }
        public static bool operator ==(Point3f a, Point3f b) => a.Equals(b);
        public static bool operator !=(Point3f a, Point3f b) => !a.Equals(b);
        public bool Equals(Point3f other)
        {
            return x == other.x && y == other.y && z == other.z;
        }
        public override bool Equals(object obj)
        {
            return obj is Point3f other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                hash = hash * 23 + z.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"({x:F6}, {y:F6}, {z:F6})";
        }

        public string ToString(string format)
        {
            return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)})";
        }
    }

}
