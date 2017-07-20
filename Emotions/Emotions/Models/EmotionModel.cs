﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotions.Models
{
    class EmotionModel
    {
        public FaceRecangle faceRectangle { get; set; }
        public Scores scores { get; set; }

        public class FaceRecangle
        {
            public int left { get; set; }
            public int top { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Scores
        {
            public double anger { get; set; }
            public double contempt { get; set; }
            public double disgust { get; set; }
            public double fear { get; set; }
            public double hapiness { get; set; }
            public double neutral { get; set; }
            public double sadness { get; set; }
            public double surprise { get; set; }
        }
    }
}
