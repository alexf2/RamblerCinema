using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;

namespace Rambler.Cinema.EntFrameworkDB
{
    public static class DataExtensions
    {
        public static void RegisterDeleteOnRemove<TEntity>(this ICollection<TEntity> collection, DbContext ctx) where TEntity : class
        {
            var entityCollection = collection as EntityCollection<TEntity>;
            if (entityCollection == null)
                return;

            entityCollection.AssociationChanged += (CollectionChangeEventHandler)((sender, e) =>
            {
                if (e.Action != CollectionChangeAction.Remove)
                    return;

                var entity = e.Element as TEntity;
                if ((object)entity == null)
                    return;

                ctx.Entry<TEntity>(entity).State = EntityState.Deleted;
            });
        }

        public static void RegisterForDelete<TDbSetItem>(this DbContext ctx, Action<TDbSetItem> act) where TDbSetItem : class
        {
            ctx.Set<TDbSetItem>().Local.CollectionChanged += (sender, e) =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add)
                    return;
                foreach (TDbSetItem item in e.NewItems)
                    act(item);
            };
        }
    }
}
