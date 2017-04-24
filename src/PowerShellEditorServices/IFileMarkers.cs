using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices
{
    public class FileMarker
    {

    }

    public interface IFileMarkers
    {
        void SetMarkers(
            string markerSource,
            IEnumerable<FileMarker> newMarkers);

        void ClearMarkers(
            string markerSource);
    }
}
