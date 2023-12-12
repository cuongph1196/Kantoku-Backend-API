using System.Collections.Generic;
namespace QLKTX.Class.Dtos
{
    public class PageReponResult<T> : PagedReponBase
    {
        public IEnumerable<T> Items { set; get; }
    }
}
