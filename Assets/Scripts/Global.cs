using Unity.Mathematics;

namespace Voxhull
{
    public class Global
    {
        public static int minChunkLength = 5;
        public static int minChunkWidth = 5;
        public static int minCHunkHeight = 5;
        public static int chunkBuffer = 4;
        public static int defaultLength = 5;
        public static int defaultWidth = 8;
        public static int defaultHeight = 5;
        public static int3 defaultChunkDimensions = new
        (
            defaultLength, 
            defaultWidth, 
            defaultHeight
        );
    }
}
