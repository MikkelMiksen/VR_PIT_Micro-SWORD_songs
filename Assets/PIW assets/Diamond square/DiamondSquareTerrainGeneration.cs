using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrainGeneration
{
     private float[,] heightmap;
     private System.Random random;
     
     public void DiamondStep(int x, int y,int step, float scale, int size)
     {
          int halfStep = step / 2;
          float avg = GetAverage(new List<float?>
          {
               GetSafe(x - halfStep, y - halfStep, size),
               GetSafe(x-halfStep, y+halfStep, size),
               GetSafe(x+halfStep,y-halfStep, size),
               GetSafe(x+halfStep,y+halfStep,size)
          });
          heightmap[x,y] = avg + RandomRange(-scale,scale);
     }

     public void SquareStep(int x, int y, int step, float scale,int size)
     {
          int halfStep = step / 2;
          float avg = GetAverage(new List<float?>
          {
               GetSafe(x-halfStep, y, size),
               GetSafe(x+halfStep,y,size),
               GetSafe(x,y-halfStep,size),
               GetSafe(x,y+halfStep,size)
          });
          
          heightmap[x,y] = avg + RandomRange(-scale,scale);
     }

     public float[,] Generate(int size, float roughness, int? seed = null)
     {
          if (size - 1 <= 0 || ((size - 1) & (size - 2)) != 0)
          {
               throw new System.ArgumentException("Size must be a power of 2 greater than 1");
          }

          heightmap = new float[size, size];
          random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();

          heightmap[0, 0] = RandomRange(-1f, 1f);
          heightmap[0,size-1] = RandomRange(-1f, 1f);
          heightmap[size-1,0] = RandomRange(-1f, 1f);
          heightmap[size-1,size-1] = RandomRange(-1f, 1f);
          
          int step = size - 1;
          float scale = 1.0f;

          while (step > 1)
          {
               int halfStep = step / 2;
               for (int x = halfStep; x < size - halfStep; x += step)
               {
                    for (int y = halfStep; y < size; y += step)
                    {
                         DiamondStep(x, y, step, scale, size);
                    }
               }

               for (int x = 0; x < size; x += halfStep)
               {
                    for (int y = (x + halfStep) % step; y < size; y += step)
                    {
                         SquareStep(x, y, step, scale, size);
                    }
               }
               
               step /= 2;
               scale *= Mathf.Pow(2, -roughness);
          }

          return heightmap;
     }

     private float? GetSafe(int x, int y, int size)
     {
          if(x<0 || x>=size || y<0 || y>=size) return null;
          return heightmap[x,y];
     }

     private float GetAverage(List<float?> points)
     {
          float sum = 0;
          int count = 0;
          foreach(var point in points)
          {
               if(point.HasValue)
               {
                    sum += point.Value;
                    count++;
               }
          }
          return count > 0 ? sum/count : 0;
     }

     private float RandomRange(float min, float max)
     {
          return (float)random.NextDouble() * (max - min) + min;
     }
}