using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPageSize = 15;
    public int pageNumber { get; set; } = 1;
    private int _pageSize { get; set; } = 1;

    public int PageSize
    {
        get {  return _pageSize; }
        set { _pageSize = maxPageSize > value ? value : maxPageSize; }
    }
}
