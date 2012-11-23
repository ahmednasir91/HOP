using System.Collections.Generic;
using HomeOnPhone.ViewModels;

namespace HomeOnPhone.Library
{
    public static class LinqExtension
    {
        public static IEnumerable<HomeDetails> AddIndex(this IEnumerable<HomeDetails> linq)
        {
            var i = 1;
            foreach (var hd in linq)
            {
                hd.Index = i++;
                yield return hd;
            }
        }

    }
}
