using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.BindingModels;
using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.ViewModels;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BWYou.Web.MVC.Extensions
{
    public static class CustomExtensions
    {
        public static ILog logger = LogManager.GetLogger(typeof(CustomExtensions));

        /// <summary>
        /// Deep 복사 본 만들기. DB 저장을 위해 관계 처리
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="seen"></param>
        /// <param name="bCopykey"></param>
        /// <param name="bCascade"></param>
        /// <param name="direction"></param>
        /// <param name="bClone"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source, Dictionary<object, object> seen, bool bCopykey, bool bCascade, CascadeRelationAttribute.CascadeDirection direction, bool bClone) where T : BWModel
        {
            if (source == null)
            {
                logger.Warn(string.Format("Clone Source is null return default(T): type={0}",
                                                source.GetType().FullName));
                return default(T);
            }
            if (seen.ContainsKey(source) == true)
            {
                return (T)seen[source];
            }

            //부모자식 관계 일 때는 자식이 부모를 만들지 않고 원본을 그대로 돌려주도록 해야 함
            if (direction == CascadeRelationAttribute.CascadeDirection.Up || bClone == false)
            {
                logger.Warn(string.Format("Clone Source as it is : type={0}, id={1}",
                                                source.GetType().FullName,
                                                typeof(BWModel).IsAssignableFrom(source.GetType()) ? ((BWModel)(object)source).Id.ToString() : "source not BWModel"));
                seen.Add(source, source);
                return source;
            }


            T clone = (T)Activator.CreateInstance(source.GetType());
            seen.Add(source, clone);

            //NonCopyable 하지 않고, CascadeRelation에 속하지 않은 프로퍼티 모두 복사
            var props = source.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NonCopyableAttribute), true).Length == 0
                                                                    && p.GetCustomAttributes(typeof(NonCopyableAttribute)).Count() == 0
                                                                    && p.GetCustomAttributes(typeof(CascadeRelationAttribute), true).Length == 0
                                                                    && p.GetCustomAttributes(typeof(CascadeRelationAttribute)).Count() == 0);
            foreach (var prop in props)
            {
                //Key의 이름은 Id로 한다고 본다. BWModel 사용 시 Id가 PK가 됨
                if (prop.Name == "Id")
                {
                    if (bCopykey == false)
                    {
                        continue;
                    }
                }
                prop.SetValue(clone, prop.GetValue(source, null), null);
            }

            //CascadeRelation에 속한 프로퍼티는 Cascade 복사
            if (bCascade == true)
            {
                props = source.GetType().GetProperties().Where(p => (p.GetCustomAttributes(typeof(CascadeRelationAttribute), true).Length != 0
                                                                    || p.GetCustomAttributes(typeof(CascadeRelationAttribute)).Count() != 0)
                                                                    && p.GetCustomAttributes(typeof(NonCopyableAttribute), true).Length == 0
                                                                    && p.GetCustomAttributes(typeof(NonCopyableAttribute)).Count() == 0);

                foreach (var prop in props)
                {
                    if (typeof(BWModel).IsAssignableFrom(prop.PropertyType))
                    {
                        var t = prop.GetValue(source, null);

                        CascadeRelationAttribute attr = (CascadeRelationAttribute)prop.GetCustomAttribute(typeof(CascadeRelationAttribute));
                        prop.SetValue(clone, t == null ? null : ((BWModel)t).Clone(seen, bCopykey, bCascade, attr.Direction, attr.Clonable), null);
                    }
                    else if (typeof(ICollection<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition()))
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(prop.PropertyType.GetGenericArguments()[0]);
                        var instance = (IList)Activator.CreateInstance(constructedListType);

                        var t = (IEnumerable)prop.GetValue(source, null);
                        foreach (var item in t)
                        {
                            if (typeof(BWModel).IsAssignableFrom(item.GetType()))
                            {
                                CascadeRelationAttribute attr = (CascadeRelationAttribute)prop.GetCustomAttribute(typeof(CascadeRelationAttribute));
                                instance.Add(item == null ? null : ((BWModel)item).Clone(seen, bCopykey, bCascade, attr.Direction, attr.Clonable));
                            }
                            else
                            {
                                logger.Warn(string.Format("Clone Property ICollection<T> T not BWModel type={0}, ToString={1}",
                                                                item.GetType().FullName,
                                                                item.GetType().ToString()));
                            }
                        }
                        prop.SetValue(clone, instance, null);
                    }
                    else
                    {
                        logger.Warn(string.Format("Clone Property not BWModel type={0}, ToString={1}",
                                                        prop.PropertyType.FullName,
                                                        prop.PropertyType.ToString()));
                    }
                }
            }

            return clone;
        }

        /// <summary>
        /// 관계 정보를 모두 불러와서 활성화 시키기. 지우기용.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="seen"></param>
        public static void ActivateRelation4Cascade<T>(this T source, HashSet<object> seen) where T : BWModel
        {
            if (source == null)
            {
                logger.Warn(string.Format("ActivateRelation Source is null : type={0}",
                                                source.GetType().FullName));
                return;
            }
            if (seen.Contains(source) == true)
            {
                return;
            }
            else
            {
                seen.Add(source);
            }

            var props = source.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(CascadeRelationAttribute), true).Length != 0
                                                                                || p.GetCustomAttributes(typeof(CascadeRelationAttribute)).Count() != 0);

            foreach (var prop in props)
            {
                CascadeRelationAttribute attr = (CascadeRelationAttribute)prop.GetCustomAttribute(typeof(CascadeRelationAttribute));
                if (attr.Direction == CascadeRelationAttribute.CascadeDirection.Down)
                {
                    if (typeof(BWModel).IsAssignableFrom(prop.PropertyType))
                    {
                        var t = prop.GetValue(source, null);
                        ((BWModel)t).ActivateRelation4Cascade(seen);
                    }
                    else if (typeof(ICollection<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition()))
                    {
                        var t = (IEnumerable)prop.GetValue(source, null);
                        foreach (var item in t)
                        {
                            if (typeof(BWModel).IsAssignableFrom(item.GetType()))
                            {
                                ((BWModel)item).ActivateRelation4Cascade(seen);
                            }
                            else
                            {
                                logger.Warn(string.Format("ActivateRelation Property ICollection<T> T not BWModel type={0}, ToString={1}",
                                                                item.GetType().FullName,
                                                                item.GetType().ToString()));
                            }
                        }
                    }
                    else
                    {
                        logger.Warn(string.Format("ActivateRelation Property not BWModel type={0}, ToString={1}",
                                                        prop.PropertyType.FullName,
                                                        prop.PropertyType.ToString()));
                    }
                }
            }
        }


        public static void MapFrom<TTarget, TSource>(this TTarget target, TSource source)
            where TSource : BWModel
            where TTarget : IModelLoader<TSource>
        {
            if (source == null)
            {
                logger.Warn(string.Format("MapFrom Source is null : type={0}",
                                                source.GetType().FullName));
                return;
            }

            var srcFields = (from PropertyInfo prop in source.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(CascadeRelationAttribute), true).Length == 0
                                                                    && p.GetCustomAttributes(typeof(CascadeRelationAttribute)).Count() == 0
                                                                    && p.CanRead == true)
                            select new
                            {
                                Name = prop.Name,
                                Type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
                            }).ToList();

            var targetFields = (from PropertyInfo prop in target.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(CascadeRelationAttribute), true).Length == 0
                                                                    && p.GetCustomAttributes(typeof(CascadeRelationAttribute)).Count() == 0
                                                                    && p.CanWrite == true)
                                select new
                                {
                                    Name = prop.Name,
                                    Type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
                                }).ToList();

            var commonFields = srcFields.Intersect(targetFields);

            foreach (var field in commonFields)
            {
                var value = source.GetType().GetProperty(field.Name).GetValue(source, null);
                target.GetType().GetProperty(field.Name).SetValue(target, value, null);
            }

        }

        public static void MapFromBindingModelToBaseModel<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : IBindingModel<TTarget>
            where TTarget : BWModel
        {
            if (source == null)
            {
                logger.Warn(string.Format("MapFromBindingModelToBaseModel Source is null : type={0}",
                                                source.GetType().FullName));
                return;
            }

            var srcFields = (from PropertyInfo prop in source.GetType().GetProperties().Where(p => p.CanRead == true)
                             select new
                             {
                                 Name = prop.Name,
                                 Type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
                             }).ToList();

            var targetFields = (from PropertyInfo prop in target.GetType().GetProperties().Where(p => p.CanWrite == true)
                                select new
                                {
                                    Name = prop.Name,
                                    Type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
                                }).ToList();

            var commonFields = srcFields.Intersect(targetFields);

            foreach (var field in commonFields)
            {
                var value = source.GetType().GetProperty(field.Name).GetValue(source, null);
                target.GetType().GetProperty(field.Name).SetValue(target, value, null);
            }

        }
    }
}
