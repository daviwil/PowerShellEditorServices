//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;

namespace Microsoft.PowerShell.EditorServices
{
    public interface IEditorFeatures
    {
        void AddFeature(Type featureType, object feature);

        object GetFeature(Type featureType);

        bool TryGetFeature(out object feature);

        bool HasFeature(Type featureType);
    }

    public static class IEditorFeaturesExtensions
    {
        public static void AddFeature<TFeature>(this IEditorFeatures features, TFeature feature)
        {
            features.AddFeature(typeof(TFeature), feature);
        }

        public static TFeature GetFeature<TFeature>(this IEditorFeatures features)
        {
            return (TFeature)features.GetFeature(typeof(TFeature));
        }

        public static bool TryGetFeature<TFeature>(this IEditorFeatures features, out TFeature feature)
        {
            object featureObject = null;

            if (features.TryGetFeature(out featureObject) && featureObject is TFeature)
            {
                feature = (TFeature)featureObject;
                return true;
            }

            feature = default(TFeature);
            return false;
        }

        public static bool HasFeature<TFeature>(this IEditorFeatures features)
        {
            return features.HasFeature(typeof(TFeature));
        }
    }
}
