﻿#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace FilterFramework.Model
{
    public class FilterCollection<T> : List<IFilter<T>>
    {
        private Expression<Func<T, bool>> MyPredicate { get; set; }
        private readonly Expression<Func<T, bool>> _myDefaultPredicate;
        private Expression<Func<T, bool>> MyDefaultPredicate { get { return _myDefaultPredicate; }}

        public FilterCollection()
        {
            _myDefaultPredicate = PredicateBuilderExtension.True<T>();
            MyPredicate = MyDefaultPredicate;
        }

        public new void Add(IFilter<T> f)
        {
            if(f.IsEnabled)
                MyPredicate = MyPredicate.And(f.MyExpressionFunc);
            base.Add(f);
        }

        public IEnumerable<T> ApplyFilter(IEnumerable<T> coll)
        {
            IQueryable<T> query = coll.AsQueryable();
            var v = query.Where(MyPredicate);
            return v;
        }

        public new void Clear()
        {
            MyPredicate = MyDefaultPredicate;
            base.Clear();
        }
    }
}