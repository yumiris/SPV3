using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Promise.Library.Video
{
    public class VideoAdapter
    {
        public List<string> AdapterNamesList => Screen.AllScreens.Select(adapter => adapter.DeviceName).ToList();
    }
}