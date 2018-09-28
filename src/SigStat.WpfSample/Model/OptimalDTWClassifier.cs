﻿using SigStat.Common;
using SigStat.WpfSample.Common;
using SigStat.WpfSample.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigStat.WpfSample.Model
{
    public class OptimalDTWClassifier : IClassifier
    {
        public List<FeatureDescriptor> InputFeatures { get; set; }
        public List<SimilarityResult> SimilarityResults { get; private set; }

        private List<Signature> referenceSignatures;
        private List<Signature> trainSignatures;
        private double threshold;

        public OptimalDTWClassifier(List<FeatureDescriptor> inputFeatures)
        {
            InputFeatures = inputFeatures;
        }

        public double Train(List<Signature> signatures)
        {
            referenceSignatures = signatures.FindAll(s => s.Origin == Origin.Genuine).Take(10).ToList();
            trainSignatures = signatures.FindAll(s => s.Origin == Origin.Genuine);
            trainSignatures.AddRange(signatures.FindAll(s => s.Origin == Origin.Forged).Take(10).ToList());

            
            CalculateSimilarity();

            threshold = new OptimalClassifierHelper(SimilarityResults).CalculateThresholdForOptimalClassification();
            return threshold;
        }

        public bool Test(Signature signature)
        {
            return GetAvgDistFromReferences(signature) <= threshold;
        }

        private void CalculateSimilarity()
        {
            SimilarityResults = new List<SimilarityResult>(trainSignatures.Count);
            foreach (var testSig in trainSignatures)
            {
                SimilarityResults.Add(new SimilarityResult(testSig, GetAvgDistFromReferences(testSig)));
            }
        }

        private double GetAvgDistFromReferences(Signature sig)
        {
            double avgDist = 0;
            int count = referenceSignatures.Count;
            foreach (var refSig in referenceSignatures)
            {
                if (sig == refSig)
                    count--;
                else
                    avgDist += new Dtw(sig, refSig, InputFeatures).CalculateDtwScore();
            }
            avgDist /= count;

            return avgDist;
        }
    }
}