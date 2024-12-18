using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;

namespace Cait.Excel.Ai
{
    [ComVisible(true)]
    public class Ribbon : ExcelRibbon
    {
        public override string GetCustomUI(string RibbonID)
        {
            return Resources.Ribbon;
        }

        public override object LoadImage(string imageId)
        {
            // This will return the image resource with the name specified in the image='xxxx' tag
            return Resources.ResourceManager.GetObject(imageId);
        }

        public void OnButtonPressed(IRibbonControl control)
        {
            var frm = new FrmConfiguration();
            frm.ShowDialog();
        }
    }
}
