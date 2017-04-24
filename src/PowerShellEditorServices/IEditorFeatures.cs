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

        bool TryGetFeature(Type featureType, out object feature);

        bool HasFeature(Type featureType);
    }
}
