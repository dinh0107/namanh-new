using hailinh.Models;
using System.Collections.Generic;

namespace hailinh.ViewModel
{
    public class ListContactViewModel
    {
        public PagedList.IPagedList<Contact> Contacts { get; set; }
        public string Name { get; set; }
    }
}
