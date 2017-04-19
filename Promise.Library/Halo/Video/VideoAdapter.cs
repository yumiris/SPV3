using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Promise.Library.Halo.Video
{
    public class VideoAdapter
    {
        public int Index { get; set; } = 1;

        public List<string> GetAdaptersList()
        {
            return Screen.AllScreens.Select(adapter => adapter.DeviceName).ToList();
        }
    }
}