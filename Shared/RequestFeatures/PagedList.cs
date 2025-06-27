using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData metaData { get; set; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        metaData = new MetaData
        {
            pageSize = pageSize,
            totalCount = count,
            currentPage = pageNumber,
            totalPages = (int)Math.Ceiling((double)count / pageSize)
        };
        AddRange(items);
    }
    public static PagedList<T> ToPagedList(IEnumerable<T> source, int count, int pageSize, int pageNumber)
    {
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
