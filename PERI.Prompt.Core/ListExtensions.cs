﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PERI.Prompt.Core
{
    /// <summary>
    /// Helper methods for the lists.
    /// <see cref="https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size"/>
    /// </summary>
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
