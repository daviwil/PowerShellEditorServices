//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.EditorServices.Host
{
    internal class FeatureManager : IEditorFeatures
    {
        private Dictionary<Type, object> features = new Dictionary<Type, object>();

        public void AddFeature(Type featureType, object feature)
        {
            this.features.Add(featureType, feature);
        }

        public object GetFeature(Type featureType)
        {
            return this.features[featureType];
        }

        public bool TryGetFeature(Type featureType, out object feature)
        {
            return this.features.TryGetValue(featureType, out feature);
        }

        public bool HasFeature(Type featureType)
        {
            return this.features.ContainsKey(featureType);
        }
    }
}
