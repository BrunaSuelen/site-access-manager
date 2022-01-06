using SiteAccessManager.Models;

namespace SiteAccessManager.Adapters
{
    public class AccessAdapter
    {

        public string Acessos { get; set; }
        
        public AccessAdapter(Access data) {
            if (data.value == null) {
                this.Acessos = "0";
            } else {
                this.Acessos = data.value;
            }
        }
    }
}