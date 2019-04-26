﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ML.Auto.Test
{
    [TestClass]
    public class TransformPostTrainerInferenceTests
    {
        [TestMethod]
        public void TransformPostTrainerMulticlassNonKeyLabel()
        {
            TransformPostTrainerInferenceTestCore(TaskKind.MulticlassClassification,
                new[]
                {
                    new DatasetColumnInfo("Numeric1", NumberDataViewType.Single, ColumnPurpose.NumericFeature, new ColumnDimensions(null, null)),
                    new DatasetColumnInfo("Label", NumberDataViewType.Single, ColumnPurpose.Label, new ColumnDimensions(null, null)),
                }, @"[
  {
    ""Name"": ""KeyToValueMapping"",
    ""NodeType"": ""Transform"",
    ""InColumns"": [
      ""PredictedLabel""
    ],
    ""OutColumns"": [
      ""PredictedLabel""
    ],
    ""Properties"": {}
  }
]");
        }

        [TestMethod]
        public void TransformPostTrainerBinaryLabel()
        {
            TransformPostTrainerInferenceTestCore(TaskKind.BinaryClassification,
                new[]
                {
                    new DatasetColumnInfo("Numeric1", NumberDataViewType.Single, ColumnPurpose.NumericFeature, new ColumnDimensions(null, null)),
                    new DatasetColumnInfo("Label", NumberDataViewType.Single, ColumnPurpose.Label, new ColumnDimensions(null, null)),
                }, @"[]");
        }

        [TestMethod]
        public void TransformPostTrainerMulticlassKeyLabel()
        {
            TransformPostTrainerInferenceTestCore(TaskKind.MulticlassClassification,
                new[]
                {
                    new DatasetColumnInfo("Numeric1", NumberDataViewType.Single, ColumnPurpose.NumericFeature, new ColumnDimensions(null, null)),
                    new DatasetColumnInfo("Label", new KeyDataViewType(typeof(uint), 3), ColumnPurpose.Label, new ColumnDimensions(null, null)),
                }, @"[]");
        }

        private static void TransformPostTrainerInferenceTestCore(
            TaskKind task,
            DatasetColumnInfo[] columns,
            string expectedJson)
        {
            var transforms = TransformInferenceApi.InferTransformsPostTrainer(new MLContext(), task, columns);
            var pipelineNodes = transforms.Select(t => t.PipelineNode);
            Util.AssertObjectMatchesJson(expectedJson, pipelineNodes);
        }
    }
}