using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace g4
{
    public class PolyLine3d : IEnumerable<Vector3d>
    {
        protected List<Vector3d> vertices;
        public int Timestamp;

        // The domain will be stored as a tuple representing (start, end)
        public (double Start, double End) Domain { get; set; }
        public PolyLine3d()
        {
            vertices = new List<Vector3d>();
            Timestamp = 0;
            Domain = (0, VertexCount - 1);
        }

        public PolyLine3d(PolyLine3d copy)
        {
            vertices = new List<Vector3d>(copy.vertices);
            Timestamp = 0;
            Domain = (0, VertexCount - 1);
        }

        // Constructor with array of Vector3d
        public PolyLine3d(Vector3d[] v, bool allowDuplicatePoints = false)
        {
            vertices = new List<Vector3d>();
            AddVertices(v, allowDuplicatePoints);
            Timestamp = 0;
            Domain = (0, VertexCount - 1);
        }

        // Constructor with VectorArray3d
        public PolyLine3d(VectorArray3d v, bool allowDuplicatePoints = false)
        {
            vertices = new List<Vector3d>();
            AddVertices(v.AsVector3d(), allowDuplicatePoints);
            Timestamp = 0;
            Domain = (0, VertexCount - 1);
        }

        // Constructor with IEnumerable<Vector3d>
        public PolyLine3d(IEnumerable<Vector3d> v, bool allowDuplicatePoints = false)
        {
            vertices = new List<Vector3d>();
            AddVertices(v, allowDuplicatePoints);
            Timestamp = 0;
            Domain = (0, VertexCount - 1);
        }

        public PolyLine3d(List<Vector3d> v, bool allowDuplicatePoints = false)
        {
            vertices = new List<Vector3d>();
            AddVertices(v, allowDuplicatePoints);
            Timestamp = 0;
            Domain = (0, VertexCount - 1);
        }


        // Method to add vertices with optional duplicate point checking
        private void AddVertices(IEnumerable<Vector3d> v, bool allowDuplicatePoints = false)
        {
            foreach (var point in v)
            {
                if (allowDuplicatePoints || vertices.Count == 0 || !vertices.Last().Equals(point))
                {
                    vertices.Add(point);
                }
            }
        }

        public Vector3d this[int key]
        {
            get { return vertices[key]; }
            set { vertices[key] = value; Timestamp++; }
        }

        public void SetDomain(double start, double end)
        {
            Domain = (start, end);
            Timestamp++;
        }

        public Vector3d Start
        {
            get { return vertices[0]; }
        }
        public Vector3d End
        {
            get { return vertices[vertices.Count - 1]; }
        }
        public ReadOnlyCollection<Vector3d> Vertices
        {
            get { return vertices.AsReadOnly(); }
        }

        public int VertexCount
        {
            get { return vertices.Count; }
        }
        public void AppendVertex(Vector3d v) 
        {
            if (vertices.Count == 0 || !vertices.Last().Equals(v))
            {
                vertices.Add(v);
                Timestamp++;
            }
        } 

        // Append a single vertex using x, y, z coordinates with optional duplicate checking
        public void AppendVertex(double x, double y, double z)
        {
            Vector3d v = new Vector3d(x, y, z);
            if (vertices.Count == 0 || !vertices.Last().Equals(v))
            {
                vertices.Add(v);
                Timestamp++;
            }
        }

        // Insert a vertex at a specific index with optional duplicate checking
        public void InsertVertex(int i, Vector3d v)
        {
            if (i < 0 || i > vertices.Count)
                throw new ArgumentOutOfRangeException(nameof(i), "Index is out of range.");

            bool isDuplicate = false;
            // Check for duplicates based on adjacent points
            if ((i > 0 && vertices[i - 1].Equals(v)) || (i < vertices.Count && vertices[i].Equals(v)))
            {
                isDuplicate = true;
            }

            if (!isDuplicate)
            {
                vertices.Insert(i, v);
                Timestamp++;
            }
        }

        public Vector3d GetTangent(int i)
        {
            if (i == 0)
                return (vertices[1] - vertices[0]).Normalized;
            else if (i == vertices.Count - 1)
                return (vertices[vertices.Count - 1] - vertices[vertices.Count - 2]).Normalized;
            else
                return (vertices[i + 1] - vertices[i - 1]).Normalized;
        }


        public AxisAlignedBox3d GetBounds()
        {
            if (vertices.Count == 0)
                return AxisAlignedBox3d.Empty;
            AxisAlignedBox3d box = new AxisAlignedBox3d(vertices[0]);
            for (int i = 1; i < vertices.Count; ++i)
                box.Contain(vertices[i]);
            return box;
        }


        public IEnumerable<Segment3d> SegmentItr()
        {
            for (int i = 0; i < vertices.Count - 1; ++i)
                yield return new Segment3d(vertices[i], vertices[i + 1]);
        }

        public IEnumerator<Vector3d> GetEnumerator()
        {
            return vertices.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }
        public void Reverse()
        {
            vertices.Reverse(); // Reverse the order of vertices
            Timestamp++;        // Update the timestamp to reflect the change
        }

        public void ReorderVertices(int newStart)
        {
            if (!IsClosed())
            {
                throw new Exception("Polyline must be closed to reorder vertices");
            }

            else
            {
                // Split the list into two parts: from newStart to end, and from beginning to newStart
                List<Vector3d> reorderedVertices = new List<Vector3d>();

                // Add the points from newStart to the end
                reorderedVertices.AddRange(vertices.GetRange(newStart, vertices.Count - newStart));

                // Add the points from the beginning to newStart (excluding newStart)
                reorderedVertices.AddRange(vertices.GetRange(0, newStart));

                vertices = new List<Vector3d>();
                AddVertices(reorderedVertices, false);
                ClosePolyline();
                Timestamp++;  // Update timestamp to reflect changes
            }

        }

        public bool IsClosed()
        {
            return vertices[0].EpsilonEqual(vertices[vertices.Count - 1], MathUtil.ZeroTolerance);
        }

        public void ClosePolyline()
        {
            if (!IsClosed())
            {
                vertices.Add(vertices[0]);
                Timestamp++;
            }
        }

        public void RemoveVertex(int i)
        {
            vertices.RemoveAt(i);
            Timestamp++;
        }

        public double GetTotalLength()
        {
            double totalLength = 0.0;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                totalLength += (vertices[i + 1] - vertices[i]).Length;
            }
            return totalLength;
        }

    }
}
