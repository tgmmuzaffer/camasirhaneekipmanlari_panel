using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models.Dtos
{
    public class SearchDto
    {
        public string SearchText { get; set; }
        public List<SearchResultDto> SearchResultDtos { get; set; } = new List<SearchResultDto>();
    }

}
