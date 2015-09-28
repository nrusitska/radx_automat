using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RadXAutomat.Vision
{
    public class ImageComparer
    {
        public float SimilarityThreshold = 0.95f;

        protected IEnumerable<Bitmap> GetLearningSet()
        {
            //TODO: Lernmenge zurück geben
            return new List<Bitmap>();
        }

        public float Match(Bitmap testBitmap)
        {
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);
            List<float> matches = new List<float>();
            foreach(var bmp in GetLearningSet())
                    matches.Add(tm.ProcessImage(bmp,testBitmap).First().Similarity);

            return matches.Max();
        }
    }
}
